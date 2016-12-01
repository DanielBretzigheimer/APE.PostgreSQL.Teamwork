// <copyright file="Database.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using log4net;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Represents a Postgres SQL Database with its SQL Files and contains a
    /// name and a path.
    /// </summary>
    [NotifyProperty(typeof(string), "Name")]
    [NotifyProperty(typeof(string), "Path")]
    [NotifyProperty(typeof(DatabaseVersion), "CurrentVersion")]
    [NotifyProperty(typeof(DatabaseVersion), "LastApplicableVersion")]
    [NotifyProperty(typeof(ObservableCollection<SQLFile>), "UndoDiffFiles")]
    [NotifyProperty(typeof(ObservableCollection<SQLFile>), "DiffFiles")]
    [NotifyProperty(AccessModifier.Public, typeof(double), "Progress", 100, "The progress of the current action. Not all actions are modifing it.")]
    [NotifyProperty(AccessModifier.Public, typeof(string), "ProgressInfo", "", "This will be shown to the user as additional info to the current progress.")]
    [CtorParameter(AccessModifier.Private, typeof(string), "name")]
    [CtorParameter(AccessModifier.Private, typeof(string), "path")]
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(AccessModifier.Private, typeof(ISQLFileTester), "sqlFileTester")]
    [CtorParameter(AccessModifier.Private, typeof(bool), "initializeData", false, "Indicates if the data should be initialized")]
    [NotifyPropertySupport]
    public partial class Database : IDatabase
    {
        /// <summary>
        /// The name of the default postgres database.
        /// </summary>
        public const string PostgresDefaultDatabaseName = "postgres";

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object updateLock = new object();

        /// <summary>
        /// This is used to know if the files have changed and the properties for the DiffFiles and
        /// UndoDiffFiles should be changed.
        /// </summary>
        private string[] cachedFiles = new string[0];

        private bool exporting = false;

        /// <summary>
        /// Initializes a new database with an empty name and no path.
        /// </summary>
        private Database(IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager)
        {
            if (connectionManager == null)
                throw new ArgumentNullException("connectionManager", "connectionManager == null");
            this.connectionManager = connectionManager;
            if (fileSystemAccess == null)
                throw new ArgumentNullException("file", "file == null");
            this.fileSystemAccess = fileSystemAccess;
            if (processManager == null)
                throw new ArgumentNullException("process", "process == null");
            this.processManager = processManager;
        }

        /// <summary>
        /// Creates a dump file for the given database at the given path.
        /// </summary>
        public SQLFile CreateDump(string path, string dumpCreatorPath, string host, string id, string password)
        {
            this.UpdateVersion();
            var startInfo = new ProcessStartInfo(dumpCreatorPath, "-s -h " + host + " -U " + id + " -w -f \"" + path + "\" " + this.Name);

            // set the password (this is mandatory for postgres 9.5)
            startInfo.UseShellExecute = false;
            startInfo.EnvironmentVariables.Add("PGPASSWORD", password);

            processManager.Execute(startInfo);
            return new SQLFile(path, this, this.fileSystemAccess);
        }

        /// <summary>
        /// Creates a dump file for the given database in its directory path.
        /// </summary>
        public SQLFile CreateDump(string dumpCreatorPath, string host, string id, string password)
        {
            string dumpfileLocation = this.GenerateFileLocation(this.CurrentVersion.Main + 1, SQLTemplates.DumpFile);
            return this.CreateDump(dumpfileLocation, dumpCreatorPath, host, id, password);
        }

        /// <summary>
        /// Execute the given SQL command without a return value.
        /// </summary>
        /// <remarks>
        /// Max time is 10 minutes, after that a timeout exception is thrown.
        /// </remarks>
        public void ExecuteCommandNonQuery(string sql)
        {
            this.connectionManager.ExecuteCommandNonQuery(this, sql);
        }

        /// <summary>
        /// Execute the given SQL command.
        /// </summary>
        public List<T> ExecuteCommand<T>(string sql)
        {
            return this.connectionManager.ExecuteCommand<T>(this, sql);
        }

        /// <summary>
        /// Updates the version and the not applied SQL files.
        /// </summary>
        public void UpdateData(bool force = false)
        {
            lock (this.updateLock)
            {
                this.UpdateVersion();
                this.SearchFiles(force);
            }
        }

        /// <summary>
        /// Updates the database to the given main version by executing diffs or undo diffs.
        /// </summary>
        /// <param name="version">The target version.</param>
        /// <param name="afterFileExecution">Action which is called after a file was executed with a List of all SQLFiles 
        /// (<see cref="IEnumerable{SQLFile}"/>) and the currently executed one.</param>
        /// <exception cref="TeamworkConnectionException">Is thrown when an error occurred while executing the SQL Statements.</exception>
        public void UpdateToVersion(DatabaseVersion version, Action<IEnumerable<SQLFile>, SQLFile> afterFileExecution = null)
        {
            var files = new List<SQLFile>(this.GetToBeAppliedSQLFiles(version));
            Log.Info(string.Format("Upgrade database from version {0} to version {1} with {2} sql files", this.CurrentVersion, version, files.Count()));

            // execute diffs
            for (int fileIndex = 0; fileIndex < files.Count(); fileIndex++)
            {
                var file = files.ElementAt(fileIndex);
                file.ExecuteInTransaction();
                this.UpdateVersion();

                afterFileExecution?.Invoke(files, file);
            }
        }

        /// <summary>
        /// Gets a <see cref="IEnumerable{SQLFile}"/> which needs to be executed to get to the given <see cref="DatabaseVersion"/>.
        /// </summary>
        public IEnumerable<SQLFile> GetToBeAppliedSQLFiles(DatabaseVersion version)
        {
            lock (this.updateLock)
            {
                this.UpdateData();

                var executedFiles = this.GetExecutedFiles();

                if (version >= this.CurrentVersion)
                    return this.DiffFiles
                                    .Where(f => (f.Version > this.CurrentVersion
                                    || executedFiles.FirstOrDefault(execFile => execFile.DatabaseVersion == f.Version) == null)
                                    && f.Version <= version);
                else
                    return this.UndoDiffFiles
                                    .Where(f => f.Version <= this.CurrentVersion
                                    && f.Version > version
                                    && executedFiles.FirstOrDefault(execFile => execFile.DatabaseVersion == f.Version) != null);
            }
        }

        /// <summary>
        /// Removes the entries in executed statements.
        /// </summary>
        /// <remarks>IMPORTANT: This will NOT execute any undo diff files.</remarks>
        public void ReduceVersion()
        {
            // ensure that the version is 
            this.UpdateVersion();
            var sql = SQLTemplates.RemoveVersion(this.CurrentVersion);
            this.ExecuteCommandNonQuery(sql);
            this.UpdateVersion();
        }

        /// <summary>
        /// Creates the schema for the teamwork including a table for all executed files.
        /// </summary>
        public void CreateTeamworkSchema()
        {
            // check if schema exists
            var schemas = this.ExecuteCommand<int>(SQLTemplates.GetTeamworkSchemaSQL(this.Name))[0];
            var schemaExists = schemas > 0;

            // create schema and table if not already there
            if (!schemaExists)
                this.ExecuteCommandNonQuery(SQLTemplates.CreateTeamworkSchemaSQL());
        }

        /// <summary>
        /// Create a new temporary database and execute all SQL statements to verify they don't contain any error.
        /// </summary>
        public void TestSQLFiles()
        {
            this.TestSQLFiles(0, 100);
        }

        /// <summary>
        /// Undoes all changes which were made to this database by creating an undo diff and executing it.
        /// </summary>
        public void UndoChanges(string dumpCreatorPath, string host, string id, string password)
        {
            Log.Info($"Start undoing Database {this.Name} to version {this.CurrentVersion}");

            // create undo diff
            this.SetProgress(0, "Start undoing changes");
            this.UpdateVersion();

            // file paths
            string previousDump = this.GenerateFileLocation(this.CurrentVersion.Main, SQLTemplates.DumpFile);
            string dump = this.GenerateFileLocation($"{DatabaseVersion.TempUndoDumpName}{this.CurrentVersion}", SQLTemplates.DumpFile);
            string undoDiff = this.GenerateFileLocation($"{DatabaseVersion.TempUndoDiffName}{this.CurrentVersion}", SQLTemplates.UndoDiffFile);

            try
            {
                // update version before creating dump, so the new dump contains the next version 
                this.SetProgress(10, "Creating dump with changes which should be undone");
                this.CreateDump(dump, dumpCreatorPath, host, id, password);

                if (this.fileSystemAccess.GetFileSize(previousDump) == this.fileSystemAccess.GetFileSize(dump))
                {
                    // check files byte by byte
                    var oldDump = this.fileSystemAccess.ReadAllLines(previousDump);
                    var newDump = this.fileSystemAccess.ReadAllLines(dump);

                    if (oldDump.SequenceEqual(newDump))
                        throw TeamworkException.NoChanges(previousDump, dump);
                }

                // diff only original dumps
                this.SetProgress(20, "Finding differences and creating files");
                this.CreateDiffFile(dump, previousDump, undoDiff);

                // execute undo diff
                var file = new SQLFile(undoDiff, this, this.fileSystemAccess);
                file.ExecuteInTransaction();
            }
            catch (TeamworkConnectionException ex)
            {
                Log.Warn(string.Format("Error occured while testing exported files."), ex);

                var file = "unknown";
                if (ex.File != null)
                    file = ex.File.Path;

                // do not delete files if only the test did not work => can be manually fixed by the user
                throw new Exception(string.Format("Error occured in file {0} while testing exported files. Diff and Dump files will not be deleted and can be edited manually. Error: {1}", file, ex.Message), ex);
            }
            catch (Exception ex)
            {
                Log.Error("Error occured while undoing changes", ex);
                throw;
            }
            finally
            {
                // remove undo diff and dump
                if (this.fileSystemAccess.FileExists(dump))
                    this.fileSystemAccess.DeleteFile(dump);
                if (this.fileSystemAccess.FileExists(undoDiff))
                    this.fileSystemAccess.DeleteFile(undoDiff);

                this.SetProgress(100);
            }
        }

        /// <summary>
        /// Sets the database to an empty state (Version 0).
        /// </summary>
        public void Reset()
        {
            var managementDatabase = new Database(this.connectionManager, this.fileSystemAccess, this.processManager) { Name = PostgresDefaultDatabaseName };

            try
            {
                // disconnect old database and delete it afterwards
                managementDatabase.ExecuteCommandNonQuery(SQLTemplates.DisconnectDatabase(this.Name));
                managementDatabase.ExecuteCommandNonQuery(SQLTemplates.DropDatabase(this.Name));
                managementDatabase.ExecuteCommandNonQuery(SQLTemplates.CreateDatabase(this.Name));

                // clear the old pool so no connection is reused!
                this.connectionManager.ClearPools();

                this.CreateTeamworkSchema();
                this.UpdateVersion();
            }
            catch (NpgsqlException ex)
            {
                Log.Error(ex.Message, ex);
                throw new Exception(string.Format("Error while resetting database .", ex));
            }
        }

        /// <summary>
        /// Creates a dump file and compares it to the dump file of the old version.
        /// all changes will be written to a diff and undo diff file.
        /// </summary>
        public void Export(string dumpCreatorPath, string host, string id, string password)
        {
            Log.Info(string.Format("Start exporting Database {0}", this.Name));
            this.SearchFiles(true);

            this.SetProgress(0, "Start the export");
            if (this.DiffFiles.Where(f => f.Version > CurrentVersion).Count() > 0)
                throw new TeamworkConnectionException(null, "Newer Versions found which must be imported before an export.");
            else
            {
                this.UpdateVersion();

                // file paths
                int newVersion = this.CurrentVersion.Main + 1;
                Log.Debug(string.Format("New Version for exported files is {0}", newVersion));
                string previousDump = this.GenerateFileLocation(this.CurrentVersion.Main, SQLTemplates.DumpFile);
                string dump = this.GenerateFileLocation(newVersion, SQLTemplates.DumpFile);
                string diff = this.GenerateFileLocation(newVersion, SQLTemplates.DiffFile);
                string undoDiff = this.GenerateFileLocation(newVersion, SQLTemplates.UndoDiffFile);

                this.exporting = true;

                try
                {
                    // update version before creating dump, so the new dump contains the next version 
                    this.SetProgress(10, "Creating a new dump");
                    this.CreateDump(dumpCreatorPath, host, id, password);

                    if (this.fileSystemAccess.GetFileSize(previousDump) == this.fileSystemAccess.GetFileSize(dump))
                    {
                        // check files byte by byte
                        var oldDump = this.fileSystemAccess.ReadAllLines(previousDump);
                        var newDump = this.fileSystemAccess.ReadAllLines(dump);

                        if (oldDump.SequenceEqual(newDump))
                            throw TeamworkException.NoChanges(previousDump, dump);
                    }

                    // diff only original dumps
                    this.SetProgress(20, "Finding differences and creating files");
                    this.CreateDiffs(previousDump, dump, diff, undoDiff);

                    // test the new diff with all previous
                    this.TestSQLFiles(40, 100);
                    Log.Info(string.Format("Finished exporting version {0}", newVersion));
                }
                catch (TeamworkConnectionException ex)
                {
                    Log.Warn(string.Format("Error occured while testing exported files."), ex);

                    var file = "unknown";
                    if (ex.File != null)
                        file = ex.File.Path;

                    // do not delete files if only the test did not work => can be manually fixed by the user
                    throw new Exception(string.Format("Error occured in file {0} while testing exported files. Diff and Dump files will not be deleted and can be edited manually. Error: {1}", file, ex.Message), ex);
                }
                catch (Exception ex)
                {
                    Log.Error("Error occured while exporting new version", ex);

                    // delete new files if an error occured
                    if (this.fileSystemAccess.FileExists(dump))
                        this.fileSystemAccess.DeleteFile(dump);
                    if (this.fileSystemAccess.FileExists(diff))
                        this.fileSystemAccess.DeleteFile(diff);
                    if (this.fileSystemAccess.FileExists(undoDiff))
                        this.fileSystemAccess.DeleteFile(undoDiff);

                    if (newVersion == this.CurrentVersion.Main)
                        this.ReduceVersion();

                    throw;
                }
                finally
                {
                    this.exporting = false;
                    this.SetProgress(100);
                }

                this.UpdateData();
            }
        }

        /// <summary>
        /// Create a diff file between the given first and second dump.
        /// </summary>
        /// <param name="firstDump">The first dump which is compared.</param>
        /// <param name="secondDump">The second dump which is compared.</param>
        /// <param name="diff">Location of the file.</param>
        public void CreateDiffFile(string firstDump, string secondDump, string diff)
        {
            if (!this.fileSystemAccess.FileExists(firstDump))
                throw new FileNotFoundException(string.Format("Dump not found ({0})", firstDump));
            if (!firstDump.Contains(SQLTemplates.DumpFile)
                || !secondDump.Contains(SQLTemplates.DumpFile))
            {
                throw new TeamworkException(string.Format(
                        "Wrong File type of dumps (previous: {0}) (actual: {1}). Need to be \"{2}\"",
                        firstDump,
                        secondDump,
                        SQLTemplates.DumpFile));
            }

            // create diff file and throw exception if nothing changed
            if (!differenceCreator.Create(diff, this.Name, firstDump, secondDump))
                throw TeamworkException.NoChanges(firstDump, secondDump);
        }

        /// <summary>
        /// Gets the last applied version.
        /// </summary>
        private void UpdateVersion()
        {
            Log.Debug(string.Format("Start updating last applicable version"));

            if (this.DiffFiles != null && this.DiffFiles.Count > 0)
                this.LastApplicableVersion = this.DiffFiles.Last().Version;
            else
                this.LastApplicableVersion = DatabaseVersion.StartVersion;

            Log.Debug(string.Format("Start updating current version (old: {0})", this.CurrentVersion));
            DatabaseVersion highestVersion = DatabaseVersion.StartVersion;
            foreach (var file in this.GetExecutedFiles())
            {
                if (file.DatabaseVersion > highestVersion)
                    highestVersion = file.DatabaseVersion;
            }

            Log.Debug(string.Format("New current Version = {0}", highestVersion));
            this.CurrentVersion = highestVersion;
        }

        private void TestSQLFiles(int progressStart, int progressEnd)
        {
            var databaseName = $"APE.PostgreSQL.Teamwork.Test.{this.Name}";
            var progressDifference = progressEnd - progressStart;
            var progressStepSmall = (double)progressDifference / 100 * 5; // 5 %
            var progressStepBig = (double)progressDifference / 100 * 40; // 40 %

            try
            {
                this.SetProgress(progressStart, "Testing the new files");

                // create temp database
                this.ExecuteCommandNonQuery(SQLTemplates.CreateDatabase(databaseName));
                this.SetProgress(this.Progress + progressStepSmall);
                Database managementDatabase = new Database(databaseName, this.Path, this.connectionManager, this.fileSystemAccess, this.processManager, this.differenceCreator, this.sqlFileTester, true);
                managementDatabase.UpdateData();

                var updateProgress = new Action<IEnumerable<SQLFile>, SQLFile>((files, file) =>
                {
                    // todo db file tester implement
                    ////this.sqlFileTester.CreateData(managementDatabase, file);
                    ////this.sqlFileTester.TestEmptyMethods(managementDatabase, file);

                    this.SetProgress(this.Progress + (progressStepBig / files.Count()), string.Format("Testing File {0}", file.FileName));
                });

                // execute all statements
                managementDatabase.UpdateToVersion(managementDatabase.LastApplicableVersion, updateProgress);

                if (managementDatabase.UndoDiffFiles.Count > 0)
                    managementDatabase.UpdateToVersion(managementDatabase.UndoDiffFiles.Last().Version, updateProgress);
            }
            finally
            {
                this.SetProgress(this.Progress + progressStepSmall, "Disconnecting Database");
                this.ExecuteCommandNonQuery(SQLTemplates.DisconnectDatabase(databaseName));
                this.SetProgress(this.Progress + progressStepSmall, "Dropping old Database");
                this.ExecuteCommandNonQuery(SQLTemplates.DropDatabase(databaseName));
                this.SetProgress(progressEnd);
            }
        }

        /// <summary>
        /// Creates a new instance of the database class and creates the
        /// teamwork schema (if it does not already exist) and updates 
        /// the data from it.
        /// </summary>
        partial void DatabaseCtor()
        {
            this.Name = this.name;
            this.Path = this.path;

            this.CreateTeamworkSchema();

            if (this.initializeData)
                this.UpdateData();
        }

        /// <summary>
        /// Gets a list of all applied versions.
        /// </summary>
        private List<ExecutedFile> GetExecutedFiles()
        {
            var sql = SQLTemplates.GetAppliedVersions();
            var files = this.ExecuteCommand<ExecutedFile>(sql);
            return files;
        }

        /// <summary>
        /// Creates the diff and undo diff file.
        /// </summary>
        /// <param name="previousDump">The previous original dump file which is used to find the differences.</param>
        /// <param name="currentDump">The original dump file which is used to find the differences.</param>
        /// <param name="diff">The path to the diff file.</param>
        /// <param name="undoDiff">The path to the undo diff file.</param>
        /// <exception cref="TeamworkConnectionException">Is thrown when no changes are found.</exception>
        private void CreateDiffs(string previousDump, string currentDump, string diff, string undoDiff)
        {
            this.CreateDiffFile(previousDump, currentDump, diff);

            new SQLFile(diff, this, this.fileSystemAccess).MarkAsExecuted();

            // create undo diff file
            this.CreateDiffFile(currentDump, previousDump, undoDiff);
        }

        /// <summary>
        /// Gets all not applied files including the first dump if no one is executed.
        /// </summary>
        private void SearchFiles(bool force)
        {
            // do not reload files while exporting
            if (this.exporting)
                return;

            var diffFiles = new ObservableCollection<SQLFile>();
            var undoDiffFiles = new ObservableCollection<SQLFile>();

            Log.Info(string.Format("Start searching files in path {0} for database {1}", this.Path, this.Name));
            if (fileSystemAccess.DirectoryExists(this.Path))
            {
                // first dump is added others are ignored
                bool dumpNeeded = true;

                // return if no files changed
                var files = fileSystemAccess.GetFiles(this.Path);
                if (!force
                        && this.cachedFiles.Count() == files.Count()
                        && this.cachedFiles.Any(f => files.Contains(f)))
                    return;

                this.cachedFiles = files;
                foreach (string file in files)
                {
                    try
                    {
                        // check if file is a dump file
                        if (dumpNeeded)
                        {
                            if (file.Contains(SQLTemplates.DumpFile))
                            {
                                diffFiles.Add(new SQLFile(file, this, this.fileSystemAccess));
                                dumpNeeded = false;
                            }
                        }
                        else if (file.Contains(SQLTemplates.DiffFile))
                            diffFiles.Add(new SQLFile(file, this, this.fileSystemAccess));
                        else if (file.Contains(SQLTemplates.UndoDiffFile))
                            undoDiffFiles.Add(new SQLFile(file, this, this.fileSystemAccess));
                    }
                    catch (ArgumentException ex)
                    {
                        // file has no version
                        Log.Warn(string.Format("File {0} has not the right version format.", file), ex);
                    }
                }
            }

            Log.Info(string.Format("Found {0} undo diff files", undoDiffFiles.Count));
            if (this.UndoDiffFiles == null
                            || undoDiffFiles.Count != this.UndoDiffFiles.Count
                            || undoDiffFiles.Any(f => this.UndoDiffFiles.FirstOrDefault((oldFile) => oldFile.Version == f.Version) == null))
                this.UndoDiffFiles = new ObservableCollection<SQLFile>(undoDiffFiles.OrderByDescending(f => f.Version.Main));

            Log.Info(string.Format("Found {0} diff files", diffFiles.Count));
            if (this.DiffFiles == null
                            || diffFiles.Count != this.DiffFiles.Count
                            || diffFiles.Any(f => this.DiffFiles.FirstOrDefault((oldFile) => oldFile.Version == f.Version) == null))
                this.DiffFiles = new ObservableCollection<SQLFile>(diffFiles.OrderBy(f => f.Version.Full));

            var executedStatements = this.GetExecutedFiles();
            var notAppliedFiles = diffFiles.Where(f => executedStatements.FirstOrDefault(s => s.DatabaseVersion == f.Version) == null).ToList();

            Log.Info(string.Format("Found {0} not applied diff files", notAppliedFiles.Count));

            Log.Debug("*** DIFF FILES ***");
            foreach (SQLFile sqlFile in this.DiffFiles)
                Log.DebugFormat(sqlFile.Path);

            Log.Debug("*** UNDO DIFF FILES ***");
            foreach (SQLFile sqlFile in this.UndoDiffFiles)
                Log.DebugFormat(sqlFile.Path);

            Log.Debug("*** NOT APPLIED FILES ***");
            foreach (SQLFile sqlFile in notAppliedFiles)
                Log.DebugFormat(sqlFile.Path);
        }

        /// <summary>
        /// Generates the file path.
        /// </summary>
        /// <param name="version">The version of the file.</param>
        /// <param name="extension">The filename behind the version (should be a constant from SQL Path).</param>
        private string GenerateFileLocation(int version, string extension)
        {
            return this.GenerateFileLocation(version.ToString().PadLeft(4, '0'), extension);
        }

        /// <summary>
        /// Generates the file path.
        /// </summary>
        /// <param name="prefix">The version of the file (or an additional prefix).</param>
        /// <param name="extension">The filename behind the version (should be a constant from SQL Path).</param>
        private string GenerateFileLocation(string prefix, string extension)
        {
            var directory = this.Path;
            if (!directory.EndsWith("\\"))
                directory += "\\";

            directory += prefix + extension;

            return directory;
        }

        /// <summary>
        /// Sets the progress and its message to the given values. The given message is also logged as debug.
        /// </summary>
        /// <param name="progress">The current progress.</param>
        /// <param name="message">The current progress message.</param>
        private void SetProgress(double progress, string message = null)
        {
            if (message != null)
                Log.Debug(message);

            this.Progress = progress;
            this.ProgressInfo = message;
        }
    }
}
