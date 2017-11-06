// <copyright file="AddDatabaseViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// ViewModel for the AddDatabaseView which lets you add databases and checks the validity of the input.
    /// </summary>
    [NotifyProperty(AccessModifier.Public, typeof(string), "DatabaseName", "")]
    [NotifyProperty(AccessModifier.Public, typeof(string), "DatabasePath", "")]
    [NotifyProperty(typeof(List<string>), "Databases")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "DataChecked", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "DatabaseExists", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Loading", false)]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(bool), "CreatingDatabase", false)]
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(typeof(ISQLFileTester))]
    [CtorParameter(AccessModifier.Private, typeof(Action), "close")]
    public partial class AddDatabaseViewModel : BaseViewModel
    {
        private readonly object databaseDirectoriesLock = new object();
        private List<string> databaseDirectories = new List<string>();

        public ICommand OkCommand { get; set; }

        public ICommand ChooseDirectoryPathCommand { get; set; }

        partial void AddDatabaseViewModelCtor()
        {
            this.InitializeCommands();

            this.ExecuteInTask(this.SearchDatabaseDirectories, (exec) => this.Loading = exec);

            this.Databases = this.connectionManager.ExecuteCommand<string>(SQLTemplates.GetAllTables());
            this.Databases.Remove(SettingsManager.Get().Setting.Id);

            // remove databases which are already added
            foreach (var db in DatabaseSetting.GetDatabaseSettings())
                this.Databases.Remove(db.Name);

            if (this.Databases.Count == 1)
                this.DatabaseName = this.Databases.Single();
        }

        private void SearchDatabaseDirectories()
        {
            var defaultDatabaseFolderPath = SettingsManager.Get().Setting.DefaultDatabaseFolderPath;

            if (!this.fileSystemAccess.DirectoryExists(defaultDatabaseFolderPath))
                return;

            this.SearchDatabaseDirectories(SettingsManager.Get().Setting.DefaultDatabaseFolderPath);

            if (string.IsNullOrWhiteSpace(this.DatabasePath) && !string.IsNullOrWhiteSpace(this.DatabaseName))
            {
                this.UpdatePath();
            }
        }

        private void CreateDatabase()
        {
            var uiDisp = Dispatcher.CurrentDispatcher;
            Task.Run(() =>
            {
                this.CreatingDatabase = true;

                var db = new Database(
                        this.DatabaseName,
                        this.DatabasePath,
                        DatabaseSetting.DefaultIgnoredSchemas,
                        this.connectionManager,
                        this.fileSystemAccess,
                        this.processManager,
                        this.differenceCreator,
                        this.sQLFileTester,
                        true);

                // create first dump if none is existing
                if (db.DiffFiles.Where(x => x.FileType == FileType.Dump).Count() == 0)
                {
                    var file = db.CreateDump(
                        SettingsManager.Get().Setting.PgDumpLocation,
                        SettingsManager.Get().Setting.Host,
                        SettingsManager.Get().Setting.Id,
                        SettingsManager.Get().Setting.Password);

                    // workaround to remove the Teamwork Schema from the first dump. This is needed
                    // to avoid conflicts when the teamwork schema is already created and the first
                    // dump is executed
                    var firstDump = File.ReadAllText(file.Path);

                    firstDump = firstDump
                    .Replace("CREATE SCHEMA \"APE.PostgreSQL.Teamwork\";", string.Empty)
                    .Replace("ALTER SCHEMA \"APE.PostgreSQL.Teamwork\" OWNER TO postgres;", string.Empty);

                    var teamworkSearchPath = "SET search_path = \"APE.PostgreSQL.Teamwork\", pg_catalog;";

                    // remove all teamwork schema tables, constraints, etc.
                    while (firstDump.Contains(teamworkSearchPath))
                    {
                        var schemaStart = firstDump.IndexOf(teamworkSearchPath);
                        var findNextStart = schemaStart + teamworkSearchPath.Length;
                        var schemaEnd = firstDump.Substring(findNextStart).IndexOf("SET search_path = ");

                        if (schemaEnd == -1)
                        {
                            firstDump = firstDump.Substring(0, schemaStart);
                        }
                        else
                        {
                            firstDump = firstDump.Substring(0, schemaStart) + firstDump.Substring(findNextStart + schemaEnd);
                        }
                    }

                    File.WriteAllText(file.Path, firstDump);

                    file.MarkAsExecuted();
                }

                DatabaseSetting.AddDatabaseSetting(db.Name, db.Path);

                this.CreatingDatabase = false;
                uiDisp.Invoke(this.close);
            });
        }

        private void InitializeCommands()
        {
            this.OkCommand = new RelayCommand(this.CreateDatabase);

            this.ChooseDirectoryPathCommand = new RelayCommand(() =>
            {
                var dialog = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = true,
                };

                dialog.SelectedPath = string.IsNullOrEmpty(this.DatabasePath)
                    ? SettingsManager.Get().Setting.DefaultDatabaseFolderPath
                    : this.DatabasePath;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                this.DatabasePath = dialog.SelectedPath;
            });
        }

        private void CheckData()
        {
            // verify that database exists with a connection
            if (!string.IsNullOrWhiteSpace(this.DatabaseName))
            {
                this.DatabaseExists = this.connectionManager.CheckConnection(this.DatabaseName);
            }

            if (string.IsNullOrWhiteSpace(this.DatabaseName)
                || string.IsNullOrWhiteSpace(this.DatabasePath)
                || DatabaseSetting.GetDatabaseSettings().Any(d => d.Name == this.DatabaseName && d.Path == this.DatabasePath)
                || !this.DatabaseExists)
            {
                this.DataChecked = false;
            }
            else
            {
                this.DataChecked = true;
            }
        }

        partial void DatabaseNameAfterSet()
        {
            this.CheckData();
            this.UpdatePath();
        }

        partial void DatabasePathAfterSet()
        {
            this.CheckData();
        }

        private void UpdatePath()
        {
            // try to find database folder
            var splitName = new Regex("([A-Z].*?(?=[A-Z\r\n]|$))");

            var splittedDatabaseName = splitName
                .Split(this.DatabaseName)
                .Where(s =>
                {
                    s = s.Replace(".", string.Empty)
                    .Replace(",", string.Empty)
                    .Trim();
                    return !string.IsNullOrWhiteSpace(s);
                });

            double confidence = 0;
            var path = string.Empty;
            lock (this.databaseDirectoriesLock)
            {
                foreach (var databaseDirectory in this.databaseDirectories)
                {
                    var matches = splittedDatabaseName.Count(name => databaseDirectory.Contains(name));
                    var lowerCaseMatches = splittedDatabaseName.Count(name => databaseDirectory.ToLower().Contains(name.ToLower()));

                    var currentConfidence = (matches * 2) + lowerCaseMatches;
                    if (currentConfidence > confidence)
                    {
                        confidence = currentConfidence;
                        path = databaseDirectory;
                    }
                }
            }

            this.DatabasePath = path;
        }

        private void SearchDatabaseDirectories(string path, int depth = 0)
        {
            // stop to search for directories if its to deep
            if (depth > 10)
            {
                return;
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                foreach (var file in Directory.GetFiles(directory, $"*{SQLTemplates.DumpFile}"))
                {
                    lock (this.databaseDirectoriesLock)
                    {
                        this.databaseDirectories.Add(directory);
                    }

                    break;
                }

                this.SearchDatabaseDirectories(directory, depth + 1);
            }
        }
    }
}
