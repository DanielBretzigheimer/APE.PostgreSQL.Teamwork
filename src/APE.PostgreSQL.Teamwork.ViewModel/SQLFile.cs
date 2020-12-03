// <copyright file="SQLFile.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using log4net;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains Properties for SQL Files and the SQL Statements from it.
    /// </summary>
    [NotifyPropertySupport]
    public partial class SQLFile : ISQLFile
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IDatabase database;
        private IFileSystemAccess file;

        /// <summary>
        /// Creates a new SQL File which gets the version from the file name.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="database">The database for which the file is.</param>
        /// <param name="fileSystemAccess">The file system access.</param>
        public SQLFile(string path, IDatabase database, IFileSystemAccess fileSystemAccess)
        {
            this.file = fileSystemAccess ?? throw new ArgumentNullException("file", "file == null");
            this.database = database ?? throw new ArgumentNullException("database", "database == null");

            if (this.file.FileExists(path))
            {
                this.FileName = System.IO.Path.GetFileName(path);

                if (this.FileName.Contains(SQLTemplates.DiffFile))
                    this.FileType = FileType.Diff;
                else if (this.FileName.Contains(SQLTemplates.DumpFile))
                    this.FileType = FileType.Dump;
                else if (this.FileName.Contains(SQLTemplates.UndoDiffFile))
                    this.FileType = FileType.UndoDiff;
                else
                    throw new ArgumentException("The path directs to an unkown file which should not be opened as an SQL File!");

                this.Path = path;
                this.Version = new DatabaseVersion(path);
                this.SQLStatements = this.GetSQLStatements();
            }
            else
            {
                throw new FileNotFoundException("The file " + path + " was not found");
            }
        }

        public string Path { get; private set; }

        public string FileName { get; private set; }

        public DatabaseVersion Version { get; private set; }

        public FileType FileType { get; private set; }

        public IEnumerable<IStatement> SQLStatements { get; set; }

        /// <summary>
        /// Refreshes the SQL Statements of the file.
        /// </summary>
        /// <remarks>This must only be called when the file was changed while the tool was opened.</remarks>
        public void Refresh()
        {
            this.SQLStatements = this.GetSQLStatements();
        }

        /// <summary>
        /// Executes all statements in an transaction.
        /// </summary>
        /// <exception cref="TeamworkConnectionException">Is thrown when an error occurred while executing the SQL Statements.</exception>
        public void ExecuteInTransaction()
        {
            try
            {
                // execute statements which dont support transaction at the beginning
                foreach (var statement in this.SQLStatements.Where(s => !s.SupportsTransaction && !s.IsTeamworkSchema))
                    statement.Execute();

                var sb = new StringBuilder();
                foreach (var statement in this.SQLStatements.Where(s => s.SupportsTransaction && !s.IsTeamworkSchema))
                    sb.AppendLine(statement.SQL);

                // execute other statements in transaction
                var sql = sb.ToString();
                this.database.ExecuteCommandNonQuery(sql);

                if (this.FileType == FileType.UndoDiff)
                    this.database.ExecuteCommandNonQuery(SQLTemplates.RemoveVersion(this.Version));

                this.database.ExecuteCommandNonQuery(SQLTemplates.AddExecutedFileSql(this.Version, this.FileType));
            }
            catch (NpgsqlException ex)
            {
                try
                {
                    this.database.ExecuteCommandNonQuery(SQLTemplates.AddExecutionHistorySql(this.Version, this.FileType, ex.Message));
                }
                catch (Exception innerEx)
                {
                    Log.Warn("Error while inserting execution history", innerEx);
                }

                Log.Warn(string.Format("File {0} contains errors", this.FileName));
                throw new TeamworkConnectionException(this, ex.Message, ex);
            }

            Log.Info(string.Format("File {0} executed successfully", this.FileName));
        }

        public void MarkAsExecuted()
        {
            this.database.ExecuteCommandNonQuery(SQLTemplates.AddExecutedFileSql(this.Version, this.FileType, "Exported from this database"));
            this.database.UpdateData();
        }

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            return string.Format("{0} Path: {1} Statements {2}", this.GetType().Name.ToString(), this.Path, this.SQLStatements.Count());
        }

        /// <summary>
        /// Scans the file at the given path for SQL Statements and returns them in a list.
        /// </summary>
        /// <param name="ignoreTeamworkExecution">Ignores if no Teamwork Execution is found => can result in no version change or other behavior.</param>
        private List<IStatement> GetSQLStatements(bool ignoreTeamworkExecution = false)
        {
            var retVal = new List<IStatement>();

            var isFunction = false;
            var isTransaction = false;
            var statement = new StringBuilder();

            try
            {
                var lines = this.file.ReadAllLines(this.Path);
                var currentSearchPath = string.Empty;
                foreach (var line in lines)
                {
                    if (line.StartsWith("SET search_path = "))
                    {
                        currentSearchPath = line;
                    }

                    // checks if a function begins in this line
                    if (new Regex("CREATE.+FUNCTION").Matches(line.ToUpper()).Count != 0)
                    {
                        isFunction = true;
                    }

                    if (!isTransaction && line.Contains("BEGIN;"))
                    {
                        isTransaction = true;
                    }
                    else if (isTransaction && line.Contains("COMMIT;"))
                    {
                        isTransaction = false;
                    }

                    // no possible end in line
                    var endPosition = line.IndexOf(";");
                    var commentStart = line.IndexOf(SQLTemplates.Comment);

                    // checks if the line is in a function
                    if (isFunction)
                    {
                        endPosition = -1;

                        // possible function endings
                        if (line.Contains("$$;"))
                        {
                            endPosition = line.IndexOf("$$;") + 3;
                        }
                        else if (line.Contains("$_$;"))
                        {
                            endPosition = line.IndexOf("$_$;") + 4;
                        }
                        else if (line.Contains("OWNER TO") && line.Contains(";"))
                        {
                            // OWNER TO ...; marks the end of an function if its copied from the pgadmin
                            endPosition = line.IndexOf(";") + 1;
                        }
                    }

                    statement.AppendLine(line);

                    // checks if statment ends
                    if (endPosition > -1
                            && (commentStart == -1 || endPosition < commentStart))
                    {
                        isFunction = false;

                        if (!isTransaction)
                        {
                            var sqlStatement = new Statement(currentSearchPath, statement.ToString().Trim(), this.database);
                            retVal.Add(sqlStatement);
                            statement.Clear();
                        }
                    }
                }

                // add ending even if its not a statement
                if (statement.ToString().Trim() != string.Empty)
                {
                    var sqlStatement = new Statement(currentSearchPath, statement.ToString().Trim(), this.database);
                    retVal.Add(sqlStatement);
                    statement.Clear();
                }

                return retVal;
            }
            catch (Exception)
            {
                // file could not be read maybe because its already opened
                return new List<IStatement>();
            }
        }
    }
}
