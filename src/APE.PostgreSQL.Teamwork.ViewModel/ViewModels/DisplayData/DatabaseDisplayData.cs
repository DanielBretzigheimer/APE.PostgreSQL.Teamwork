// <copyright file="DatabaseDisplayData.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using log4net;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Contains one database and the corresponding commands and
    /// notify properties.
    /// </summary>
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Enabled", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "EditMode", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "ShowDetails", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Exporting", false, "Indicates that the database is exporting at the moment")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Importing", false, "Indicates that the database is importing at the moment")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Testing", false, "Indicates that the database is testing at the moment")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Resetting", false, "Indicates that the database is resetting at the moment")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Undoing", false, "Indicates that the database is undoing changes at the moment")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Error", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "ImportableFilesFound", false)]
    [NotifyProperty(typeof(string), "ErrorMessage")]
    [AllowNullNotifyProperty(typeof(Database), "Database")]
    [NotifyProperty(AccessModifier.Public, typeof(DatabaseVersion), "TargetVersion")]
    [AllowNullNotifyProperty(AccessModifier.Public, typeof(List<DatabaseVersion>), "Versions")]
    [AllowNullNotifyProperty(typeof(ObservableCollection<SQLFileDisplayData>), "ApplicableSQLFiles")]
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(typeof(ISQLFileTester))]
    [CtorParameter(AccessModifier.Public, typeof(int), "Id", false, "id of the database for which the name and path are loaded")]
    [NotifyPropertySupport]
    public partial class DatabaseDisplayData
    {
        private const string DumpExtension = "SQL Dump (*" + SQLTemplates.DumpFile + ")|*" + SQLTemplates.DumpFile;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool dataInitialized = false;

        /// <summary>
        /// Design time constructor.
        /// </summary>
        public DatabaseDisplayData()
        {
            this.ShowDetails = true;
            this.ErrorMessage = "Test error message!";
        }

        /// <summary>
        /// Event is called when the <see cref="DatabaseDisplayData"/> is removed.
        /// </summary>
        public event EventHandler<EventArgs> Removed;

        // todo db make name notify property and make Database.Name/Path a normal property and use only these in GUI
        public string Name
        {
            get
            {
                if (this.Database == null)
                {
                    return DatabaseSetting.GetDatabaseSetting(this.Id).Name;
                }

                return this.Database.Name;
            }
        }

        public string Path
        {
            get
            {
                if (this.Database == null)
                {
                    return DatabaseSetting.GetDatabaseSetting(this.Id).Path;
                }

                return this.Database.Path;
            }
        }

        /// <summary>
        /// Gets or sets a bool which indicates if the <see cref="Database"/> was auto expanded (not expanded by the user). Databases get
        /// auto expanded if the GUI has enough room or get shrunk if the GUI gets smaller.
        /// </summary>
        public bool AutoExpanded { get; set; }

        // Database Commands
        public ICommand ExportCommand { get; private set; }

        public ICommand OpenImportWindowCommand { get; private set; }

        public ICommand CreateDumpCommand { get; private set; }

        public ICommand ReduceVersionCommand { get; private set; }

        public ICommand TestCommand { get; private set; }

        public ICommand OpenPathCommand { get; private set; }

        public ICommand EditCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand EditPathCommand { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public ICommand UndoCommand { get; set; }

        public ICommand ImportCommand { get; set; }

        public ICommand CreateDatabaseCommand { get; set; }

        // common commands
        public ICommand ExpandCommand { get; private set; }

        /// <summary>
        /// Updates the version and the not applied files.
        /// </summary>
        /// <param name="force">Forces a refresh of the SQL Files on directory level.</param>
        public void UpdateData(bool force = false)
        {
            if (this.Resetting || this.Error)
            {
                return;
            }

            try
            {
                this.Database.UpdateData(force);
                this.Error = false;
                this.UpdateApplicableSQLFiles();

                // add all available versions to which the user can up or downgrade
                var versions = new List<DatabaseVersion>
                {
                    DatabaseVersion.StartVersion, // undo all
                };

                versions.AddRange(this.Database.DiffFiles.Select(f => f.Version));

                // only update versions if they are changed
                if (this.Versions == null || versions == null || !versions.SequenceEqual(this.Versions))
                {
                    this.Versions = versions;
                }

                if (this.Database.CurrentVersion < this.Database.LastApplicableVersion)
                {
                    this.ImportableFilesFound = true;
                }
                else
                {
                    this.ImportableFilesFound = false;
                }

                // activate after ImportableFilesFound is set
                if (!this.dataInitialized)
                {
                    this.TargetVersion = this.Database.LastApplicableVersion;
                    this.dataInitialized = true;
                    this.Enabled = true;
                }
            }
            catch (NpgsqlException ex)
            {
                // could not connect to the database
                this.Error = true;
                this.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Updates the database to the given main version by executing diffs or undo diffs.
        /// </summary>
        /// <param name="targetVersion">The target version.</param>
        /// <exception cref="TeamworkConnectionException">Is thrown when an error occurred while executing the SQL Statements.</exception>
        public void UpdateToVersion(DatabaseVersion targetVersion)
        {
            foreach (var applicableFile in this.ApplicableSQLFiles)
            {
                applicableFile.ExecuteInTransaction();
                this.UpdateData();
            }
        }

        /// <summary>
        /// Resets the target version to its default value (max applicable version).
        /// </summary>
        public void ResetTargetVersion()
        {
            this.TargetVersion = this.Database.LastApplicableVersion;
        }

        public void ToggleExpansion(bool autoExpand = false)
        {
            // user has shown the detail so we do not automatically close it
            if (autoExpand && this.AutoExpanded == false)
            {
                return;
            }

            this.AutoExpanded = autoExpand;
            this.ShowDetails = !this.ShowDetails;
        }

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        [return: NullGuard.AllowNull]
        public override string ToString()
        {
            return this.Database.Name;
        }

        partial void DatabaseDisplayDataCtor()
        {
            this.targetVersion = DatabaseVersion.StartVersion;
            this.InitializeCommands();
            this.AutoExpanded = true;
            this.ConnectDatabase();
        }

        private void ConnectDatabase()
        {
            // first check if database can be reached
            if (this.connectionManager.CheckConnection(this.Name))
            {
                this.Database = new Database(
                    this.Name,
                    DatabaseSetting.GetDatabaseSetting(this.Id).Path,
                    this.connectionManager,
                    this.fileSystemAccess,
                    this.processManager,
                    this.differenceCreator,
                    this.sQLFileTester,
                    initializeData: false);
                this.Error = false;
                this.ErrorMessage = string.Empty;
            }
            else
            {
                // could not connect to the database
                this.ErrorMessage = $"Connection to Database '{this.Name}' cannot be established!";
                this.Error = true;
            }
        }

        partial void EnabledAfterSet()
        {
            // cant set enabled to true if an error occured
            if (this.Enabled && this.Error)
                this.Enabled = false;
        }

        partial void ErrorAfterSet()
        {
            // disable if error occured
            if (this.Error)
                this.Enabled = false;
        }

        partial void TargetVersionAfterSet()
        {
            this.UpdateApplicableSQLFiles();
        }

        /// <summary>
        /// Sets the applicable SQL files.
        /// </summary>
        private void UpdateApplicableSQLFiles()
        {
            var applicableSQLFiles = new ObservableCollection<SQLFileDisplayData>();
            foreach (var file in this.Database.GetToBeAppliedSQLFiles(this.TargetVersion))
            {
                applicableSQLFiles.Add(new SQLFileDisplayData(file));
            }

            if (this.ApplicableSQLFiles == null
                || this.ApplicableSQLFiles.Count != applicableSQLFiles.Count
                || applicableSQLFiles.Any(f => this.ApplicableSQLFiles.FirstOrDefault((oldFile) => oldFile.SQLFile.Version.Full == f.SQLFile.Version.Full) == null))
            {
                this.ApplicableSQLFiles = applicableSQLFiles;
            }
        }

        /// <summary>
        /// Removes the given database and all of its settings.
        /// </summary>
        /// <remarks>Does not delete the Postgres SQL database.</remarks>
        private async void Remove()
        {
            var messageBox = MainWindowViewModel.GetMessageBox(
                "Are you sure you want to remove the database? This will not delete the Database but will remove it from the list of the APE.PostgreSQL.Teamwork tool",
                "Remove the database",
                MessageBoxButton.YesNo);
            var result = await MainWindowViewModel.ShowDialog(messageBox);

            if (result == MaterialMessageBoxResult.Yes)
            {
                DatabaseSetting.Remove(this.Id);
                this.Removed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Saves the properties of the database in the settings file.
        /// </summary>
        private void Save()
        {
            var newDatabases = DatabaseSetting.GetDatabaseSettings();
            newDatabases.Single(d => d.Id == this.Id).Name = this.Database.Name;
            newDatabases.Single(d => d.Id == this.Id).Path = this.Database.Path;
            SettingsManager.Get().Setting.DatabaseSettings = newDatabases;
            SettingsManager.Get().Save();
            this.EditMode = false;
        }

        /// <summary>
        /// Shows a <see cref="FolderBrowserDialog"/> to the user and saves the selected path as the new database path.
        /// </summary>
        private void EditPath()
        {
            // todo db implement material folder browser
            var dialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                SelectedPath = this.Database.Path,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.Database.Path = dialog.SelectedPath;
        }

        /// <summary>
        /// Initializes all commands.
        /// </summary>
        private void InitializeCommands()
        {
            this.ExpandCommand = new RelayCommand(() => this.ToggleExpansion(false));
            this.OpenPathCommand = new RelayCommand(async () =>
            {
                if (this.fileSystemAccess.DirectoryExists(this.Database.Path))
                {
                    this.processManager.Start(this.Database.Path);
                }
                else
                {
                    var messageBox = MainWindowViewModel.GetMessageBox(
                        string.Format("Error while trying to open path. Possibly the directory {0} was deleted or the folder was renamed.", this.Database.Path),
                        "Directory not found.",
                        MessageBoxButton.OK);
                    await MainWindowViewModel.ShowDialog(messageBox);
                }
            });
            this.SaveCommand = new RelayCommand(() => this.Save());
            this.EditCommand = new RelayCommand(() => this.EditMode = true);
            this.EditPathCommand = new RelayCommand(() => this.EditPath());
            this.ExportCommand = new RelayCommand(() => this.ExecuteInTask(() => this.Export()));
            this.OpenImportWindowCommand = new RelayCommand(() => MainWindowViewModel.OpenImportWindow(this));
            this.ReduceVersionCommand = new RelayCommand(() => this.ReduceVersion());
            this.TestCommand = new RelayCommand(this.TestDatabase);
            this.CreateDumpCommand = new RelayCommand(this.CreateDump);
            this.RemoveCommand = new RelayCommand(this.Remove);
            this.ResetCommand = new RelayCommand(this.Reset);
            this.UndoCommand = new RelayCommand(this.UndoChanges);
            this.ImportCommand = new RelayCommand(this.StartImport);
            this.CreateDatabaseCommand = new RelayCommand(this.CreateDatabase);
        }

        private void CreateDatabase()
        {
            new Task(() =>
            {
                try
                {
                    var name = DatabaseSetting.GetDatabaseSetting(this.Id).Name;
                    this.connectionManager.ExecuteCommandNonQuery(SQLTemplates.CreateDatabase(name));

                    // connect to database and update default data
                    this.ConnectDatabase();
                    this.UpdateData(); // do not remove this, else the edit mode will be finished when something else calls this

                    this.StartImport();
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while executing Action in Task", ex);
                }
            }).Start();
        }

        private void StartImport()
        {
            this.ExecuteInTask(async () =>
            {
                this.UpdateData();
                this.ResetTargetVersion();
                var messageBox = MainWindowViewModel.GetMessageBox(
                            $"Import changes for database '{this.Database.Name}' to Version {this.Database.LastApplicableVersion}. Do you want to check for import conflicts?",
                            "Import database changes",
                            MessageBoxButton.YesNoCancel);
                var result = await MainWindowViewModel.ShowDialog(messageBox);
                if (result == MaterialMessageBoxResult.Cancel)
                {
                    return;
                }

                this.Importing = true;
                var oldVersion = this.Database.CurrentVersion;

                // check compatibility
                if (result == MaterialMessageBoxResult.Yes)
                {
                    if (this.Database.ImportConflicts(
                       SettingsManager.Get().Setting.PgDumpLocation,
                       SettingsManager.Get().Setting.Host,
                       SettingsManager.Get().Setting.Id,
                       SettingsManager.Get().Setting.Password))
                    {
                        // todo add message
                        var compatibilityMessageBox = MainWindowViewModel.GetMessageBox($"TODO", "Compatibility problems found.", MessageBoxButton.OK);
                        await MainWindowViewModel.ShowDialog(compatibilityMessageBox);
                        return;
                    }
                }

                try
                {
                    this.UpdateToVersion(this.Database.LastApplicableVersion);
                    var finishedMessageBox = MainWindowViewModel.GetMessageBox("All SQL Files succesfully executed!", "Succesfully Executed", MessageBoxButton.OK);
                    await MainWindowViewModel.ShowDialog(finishedMessageBox);
                    Log.Info("All sql files succesfully executed");
                }
                catch (Exception ex)
                {
                    var errorFile = this.ApplicableSQLFiles.FirstOrDefault(f => f.Status == ErrorStatus.Error);
                    var path = "unknown";
                    if (errorFile != null)
                    {
                        path = errorFile.SQLFile.Path;
                    }

                    var message = $"Error in file {path}: {ex.Message}\n\nStart rolling back to version {oldVersion}";
                    MainWindowViewModel.ShowDialog(MainWindowViewModel.GetMessageBox(message, "Execution failed", MessageBoxButton.OK)).Wait();
                    Log.Info(string.Format("Error while executing files. Rolling back to version {0}", oldVersion), ex);

                    this.UpdateToVersion(oldVersion);
                }
                finally
                {
                    this.UpdateData();
                    this.Importing = false;
                }
            });
        }

        private void UndoChanges()
        {
            this.ExecuteInTask(async () =>
            {
                var messageBox = MainWindowViewModel.GetMessageBox(
                                $"Are you sure you want to undo all changes you did to '{this.Database.Name}'?",
                                "Undo changes to the database",
                                MessageBoxButton.YesNo);
                var result = await MainWindowViewModel.ShowDialog(messageBox);
                if (result == MaterialMessageBoxResult.No)
                {
                    return;
                }

                this.Undoing = true;
                try
                {
                    this.Database.UndoChanges(
                       SettingsManager.Get().Setting.PgDumpLocation,
                       SettingsManager.Get().Setting.Host,
                       SettingsManager.Get().Setting.Id,
                       SettingsManager.Get().Setting.Password);
                }
                catch (Exception ex)
                {
                    Log.Warn($"Error while undoing changes for database {this.Database.Name}", ex);
                    var errorMessageBox = MainWindowViewModel.GetMessageBox(
                            $"Error Message: {ex.Message}",
                            "Error while undoing database changes",
                            MessageBoxButton.OK);
                    MainWindowViewModel.ShowDialog(errorMessageBox).Wait();
                }
                finally
                {
                    // ensure that the loading spinner is disabled
                    this.Undoing = false;
                }
            });
        }

        private void Reset()
        {
            this.ExecuteInTask(async () =>
                {
                    var messageBox = MainWindowViewModel.GetMessageBox(
                                "Are you sure you want to reset the database? This will delete all structure of the database including the stored data.",
                                "Reset the database",
                                MessageBoxButton.YesNo);
                    var result = await MainWindowViewModel.ShowDialog(messageBox);
                    if (result == MaterialMessageBoxResult.No)
                    {
                        return;
                    }

                    this.Resetting = true;
                    try
                    {
                        this.Database.Reset();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        // ensure that the loading spinner is disabled
                        this.Resetting = false;
                    }

                    this.UpdateData();
                });
        }

        private void CreateDump()
        {
            var sfd = new SaveFileDialog()
            {
                Filter = DumpExtension,
            };
            var result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Database.CreateDump(
                    sfd.FileName,
                    SettingsManager.Get().Setting.PgDumpLocation,
                    SettingsManager.Get().Setting.Host,
                    SettingsManager.Get().Setting.Id,
                    SettingsManager.Get().Setting.Password);
            }
        }

        private async void ReduceVersion()
        {
            var messageBox = MainWindowViewModel.GetMessageBox(
                        "Are you sure you want to reduce the database version?",
                        "Reduce the version of the database",
                        MessageBoxButton.YesNo);
            var result = await MainWindowViewModel.ShowDialog(messageBox);

            if (result == MaterialMessageBoxResult.Yes)
            {
                this.Database.ReduceVersion();
            }
        }

        /// <summary>
        /// Exports the database and shows a message box to the user if a error occurred.
        /// </summary>
        private void Export()
        {
            this.Exporting = true;
            try
            {
                this.Database.Export(
                    SettingsManager.Get().Setting.PgDumpLocation,
                    SettingsManager.Get().Setting.Host,
                    SettingsManager.Get().Setting.Id,
                    SettingsManager.Get().Setting.Password);

                // open the diff files so user can verify them
                var diff = new SQLFileDisplayData(this.Database.DiffFiles.Last());
                var undoDiff = new SQLFileDisplayData(this.Database.UndoDiffFiles.First());

                if (SettingsManager.Get().Setting.OpenFilesInDefaultApplication)
                {
                    this.processManager.Start(diff.SQLFile.Path);
                    this.processManager.Start(undoDiff.SQLFile.Path);
                }
                else
                {
                    BaseViewModel.OpenExportWindow(diff, undoDiff);
                }
            }
            catch (Exception ex)
            {
                var title = "Error while exporting database";
                var message = ex.Message;

                if (ex is FileNotFoundException)
                {
                    message = $"{message}: {((FileNotFoundException)ex).FileName}";
                }
                else if (ex is TeamworkException && !((TeamworkException)ex).ShowAsError)
                {
                    title = "Info";
                }

                Log.Warn(string.Format("Error while exporting database {0}", this.Database.Name), ex);
                var messageBox = MainWindowViewModel.GetMessageBox(
                        $"Message: {message}",
                        title,
                        MessageBoxButton.OK);
                MainWindowViewModel.ShowDialog(messageBox).Wait();
            }

            this.Exporting = false;
        }

        /// <summary>
        /// Tests the database and shows a message box for the user which informs over the success or the error.
        /// </summary>
        private void TestDatabase()
        {
            this.ExecuteInTask(async () =>
            {
                this.Testing = true;
                try
                {
                    this.Database.TestSQLFiles();

                    var messageBox = MainWindowViewModel.GetMessageBox(
                        "All sql statements successfully executed!",
                        "Test succesfully finished",
                        MessageBoxButton.OK);
                    await MainWindowViewModel.ShowDialog(messageBox);
                    Log.Info("All sql statements successfully executed");
                }
                catch (TeamworkConnectionException ex)
                {
                    var messageBox = MainWindowViewModel.GetMessageBox(
                        string.Format("Error occured while executing file {0}: {1}", ex.File.Path, ex.Message),
                        "Test failed",
                        MessageBoxButton.OK);
                    MainWindowViewModel.ShowDialog(messageBox).Wait();
                    Log.Warn(string.Format("Error occured while executing file {0}: {1}", ex.File.Path, ex.Message), ex);
                }
                catch (Exception ex)
                {
                    var messageBox = MainWindowViewModel.GetMessageBox(
                        string.Format("Unkown error occured. Error: {0}", ex.Message),
                        "Test failed",
                        MessageBoxButton.OK);
                    MainWindowViewModel.ShowDialog(messageBox).Wait();
                }
                finally
                {
                    this.Testing = false;
                }
            });
        }

        /// <summary>
        /// Executes the given action in a new task
        /// and disables the control while executing.
        /// </summary>
        private void ExecuteInTask(Action action)
        {
            new Task(() =>
            {
                this.Enabled = false;

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while executing Action in Task", ex);
                }

                this.Enabled = true;
            }).Start();
        }

        /// <summary>
        /// Executes the given action in a new task
        /// and disables the control while executing.
        /// </summary>
        private void ExecuteInTask(Func<Task> action)
        {
            new Task(async () =>
            {
                this.Enabled = false;

                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while executing Action in Task", ex);
                }

                this.Enabled = true;
            }).Start();
        }
    }
}
