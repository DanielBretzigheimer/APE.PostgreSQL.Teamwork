// <copyright file="MainWindowViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using APE.PostgreSQL.Teamwork.ViewModel.ViewModels;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// ViewModel for the MainWindowView which displays a list of databases.
    /// </summary>
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "ErrorMessage", "")]
    [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "SuccessMessage", "")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Editable", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "Loading", false)]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "ShowSearch", false)]
    [NotifyProperty(AccessModifier.Public, typeof(string), "FilterText", "")]
    [AllowNullNotifyProperty(typeof(List<DatabaseDisplayData>), "Databases")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "EditButtonEnabled", true)]
    [NotifyProperty(AccessModifier.Public, typeof(Visibility), "SaveButtonVisibility", Visibility.Hidden)]
    [Startable]
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(typeof(ISQLFileTester))]
    public partial class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
    {
        private readonly object updateLock = new object();
        private DispatcherTimer worker = null;
        private Dispatcher uiDispatcher = null;

        private List<DatabaseDisplayData> unfilteredDatabases = null;

        /// <summary>
        /// DESIGN TIME CONSTRUCTOR.
        /// </summary>
        public MainWindowViewModel()
        {
        }

        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        public string WindowTitle { get; private set; }

        /// <summary>
        /// Gets a command which will open the settings popup.
        /// </summary>
        public ICommand SettingCommand { get; private set; }

        /// <summary>
        /// Gets a command which will open the add database popup.
        /// </summary>
        public ICommand AddDatabaseCommand { get; private set; }

        /// <summary>
        /// Gets a command which will refresh the database list.
        /// </summary>
        public ICommand RefreshDatabasesCommand { get; private set; }

        /// <summary>
        /// Gets a command which will hide all database which do not match the search query.
        /// </summary>
        public ICommand SearchCommand { get; private set; }

        /// <summary>
        /// Gets a command which adjusts the databases when the window size is changed.
        /// </summary>
        public ICommand SizeChangedCommand { get; set; }

        /// <summary>
        /// Adds the databases to the list and updates there data.
        /// </summary>
        public void UpdateDatabases()
        {
            lock (this)
            {
                // only update values if databases changed
                if (this.unfilteredDatabases == null
                    || this.unfilteredDatabases.Count != DatabaseSetting.GetDatabaseSettings().Count)
                {
                    // unregister the old events
                    if (this.unfilteredDatabases != null)
                    {
                        foreach (var database in this.unfilteredDatabases)
                        {
                            database.Removed -= this.DatabaseRemoved;
                        }
                    }

                    this.unfilteredDatabases = new List<DatabaseDisplayData>();
                    foreach (var setting in DatabaseSetting.GetDatabaseSettings())
                    {
                        var databaseDisplayData = new DatabaseDisplayData(this.connectionManager, this.fileSystemAccess, this.processManager, this.differenceCreator, this.sQLFileTester, setting.Id);
                        databaseDisplayData.Removed += this.DatabaseRemoved;
                        this.uiDispatcher.Invoke(() => this.unfilteredDatabases.Add(databaseDisplayData));
                    }

                    var databases = this.unfilteredDatabases.Where(d => string.IsNullOrWhiteSpace(this.FilterText) || d.Database.Name.ToLower().Contains(this.FilterText.ToLower()));
                    this.Databases = new List<DatabaseDisplayData>(databases);
                }

                // update each database in its own task so the slow ones don't
                // delay the others
                foreach (var database in this.unfilteredDatabases)
                {
                    this.ExecuteInTask(() => database.UpdateData());
                }

                Log.Info("Databases successfully updated");
            }
        }

        /// <summary>
        /// Changes the order of the databases to the given one.
        /// </summary>
        /// <param name="newDatabaseOrder">The new order which is applied.</param>
        public void ReorderDatabases(IEnumerable<DatabaseDisplayData> newDatabaseOrder)
        {
            // do not reorder list if it was modified
            if (newDatabaseOrder.Count() != this.Databases.Count)
            {
                Log.Warn("Could not reorder databases because they were modified.");
                return;
            }

            var databaseSettings = new List<DatabaseSetting>();
            foreach (var database in newDatabaseOrder)
            {
                databaseSettings.Add(new DatabaseSetting(database.Id, database.Name, database.Path));
            }

            SettingsManager.Get().Setting.DatabaseSettings = databaseSettings;
        }

        public async void CheckVersion()
        {
            var settings = SettingsManager.Get().Setting;
            var previousVersionString = settings.ApplicationVersion == null ? "none" : settings.ApplicationVersion.Version.ToString();
            var assemblyVersion = Assembly.GetAssembly(typeof(MainWindowViewModel)).GetName().Version;
            if (settings.ApplicationVersion == null || assemblyVersion > settings.ApplicationVersion.Version)
            {
                Log.Info($"Version was upgraded from {previousVersionString} to {assemblyVersion.ToString()}");
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\CHANGELOG.md";

                var changelog = string.Empty;
                try
                {
                    using (var streamReader = new StreamReader(path))
                    {
                        changelog = streamReader.ReadToEnd();
                    }
                }
                catch (Exception)
                {
                    // changelog could not be loaded
                    changelog = "Could not load changelog!";
                }

                // upgrade => show change log
                await ShowDialog(GetMarkdownBox(changelog, "Code generation was upgraded", MessageBoxButton.OK));
            }
            else if (assemblyVersion < settings.ApplicationVersion.Version)
            {
                // downgrade
                Log.Info($"Version was downgraded from {previousVersionString} to {assemblyVersion.ToString()}");
                await ShowDialog(GetMessageBox("Your version of the CodeGeneration was downgrade. Check your settings.", "Version downgraded", MessageBoxButton.OK));
            }

            SettingsManager.Get().UpdateVersion(assemblyVersion);
        }

        /// <summary>
        /// Initializes the settings with default values if they are not set
        /// and starts an async worker which manages the databases.
        /// </summary>
        partial void MainWindowViewModelCtor()
        {
            this.uiDispatcher = Dispatcher.CurrentDispatcher;
            this.CreateCommands();

            this.connectionManager.Initialize(
                SettingsManager.Get().Setting.Id,
                SettingsManager.Get().Setting.Host,
                SettingsManager.Get().Setting.Password,
                SettingsManager.Get().Setting.Port);

            this.WindowTitle = "APE PostgreSQL Teamwork (" + Assembly.GetAssembly(typeof(MainWindowViewModel)).GetName().Version.ToString() + ")";

            // disable buttons until a database is selected
            this.EditButtonEnabled = false;
            this.worker = new DispatcherTimer
            {
                Interval = new TimeSpan(TimeSpan.TicksPerSecond * 2),
            };

            this.worker.Tick += this.UpdateWorkerTick;
        }

        private void UpdateWorkerTick(object sender, EventArgs e)
        {
            this.UpdateDatabases();
        }

        /// <summary>
        /// Starts the async worker which updates the databases.
        /// </summary>
        partial void StartGenerated()
        {
            this.CheckSettings();

            this.ExecuteInTask(() =>
                    {
                        this.UpdateDatabases();
                        if (this.worker != null && SettingsManager.Get().Setting.AutoRefresh)
                            this.worker.Start();
                    });

            this.CheckVersion();
        }

        private async void CheckSettings()
        {
            while (true)
            {
                var connectionError = !this.connectionManager.CheckConnection(SettingsManager.Get().Setting.Id);
                var dumpCreatorNotFound = !File.Exists(SettingsManager.Get().Setting.PgDumpLocation);
                var defaultPathNotFound = !Directory.Exists(SettingsManager.Get().Setting.DefaultDatabaseFolderPath)
                    && !string.IsNullOrWhiteSpace(SettingsManager.Get().Setting.DefaultDatabaseFolderPath);

                var message = string.Empty;
                if (connectionError)
                {
                    message = "Connection to the Database Server could not be established";
                }
                else if (dumpCreatorNotFound)
                {
                    var path = this.SearchFileRecursivly("pg_dump.exe", "C:\\PostgreSQL\\"); // todo db move it to settings
                    if (path == null)
                        path = this.SearchFileRecursivly("pg_dump.exe", "C:\\Program Files\\PostgreSQL\\"); // todo db move it to settings

                    // check if postgre is installed
                    if (File.Exists(path))
                    {
                        SettingsManager.Get().Setting.PgDumpLocation = path;
                        SettingsManager.Get().Save();
                        continue;
                    }
                    else
                    {
                        message = "PgDump.exe was not found. Please set the correct path in the Settings";
                    }
                }
                else if (defaultPathNotFound)
                {
                    // reset path and do not bother the user
                    SettingsManager.Get().Setting.DefaultDatabaseFolderPath = string.Empty;
                    SettingsManager.Get().Save();
                    continue;
                }
                else
                {
                    break;
                }

                var messageBox = GetMessageBox(
                    $"{message}. Do you want to change your settings? If not the application will shut down.",
                    "Verify Settings",
                    MessageBoxButton.YesNo);

                await MainWindowViewModel.ShowExtendedDialog(messageBox, this.ConnectionMessageBoxClosingEventHandler);
            }
        }

        /// <summary>
        /// Searches for the file with the given file name at the given path and returns the files path to it.
        /// </summary>
        /// <remarks>If one folder of the path contains a number the highest is chosen.</remarks>
        /// <returns>The path to the file or null if the file was not found.</returns>
        [return: NullGuard.AllowNull]
        private string SearchFileRecursivly(string filename, string path)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetFileName(file) == filename)
                {
                    return file;
                }
            }

            var directories = Directory.GetDirectories(path);
            for (var i = directories.Count() - 1; i >= 0; i--)
            {
                var file = this.SearchFileRecursivly(filename, directories.ElementAt(i));
                if (file != null)
                {
                    return file;
                }
            }

            return null;
        }

        [return: NullGuard.AllowNull]
        private object ConnectionMessageBoxClosingEventHandler(MaterialMessageBoxResult result)
        {
            if (result == MaterialMessageBoxResult.No)
            {
                Application.Current.Shutdown();
                return null;
            }

            // update content of the session
            return MainWindowViewModel.GetSettingView();
        }

        /// <summary>
        /// Stops the async worker which updates the databases.
        /// </summary>
        partial void StopGenerated()
        {
            if (this.worker != null)
                this.worker.Stop();
        }

        partial void FilterTextAfterSet()
        {
            var databases = this.unfilteredDatabases.Where(d => string.IsNullOrWhiteSpace(this.FilterText) || d.Database.Name.ToLower().Contains(this.FilterText.ToLower()));
            this.Databases = new List<DatabaseDisplayData>(databases);
        }

        private void DatabaseRemoved(object sender, EventArgs e)
        {
            this.UpdateDatabases();
        }

        private void RefreshDatabases()
        {
            this.unfilteredDatabases = null;
            this.ExecuteInTask(() => this.UpdateDatabases());
        }

        private void OpenSettings()
        {
            MainWindowViewModel.ShowDialog(MainWindowViewModel.GetSettingView());

            //// todo db this.CheckSettings();

            if (this.worker == null)
            {
                return;
            }

            if (SettingsManager.Get().Setting.AutoRefresh)
            {
                this.worker.Start();
            }
            else
            {
                this.worker.Stop();
            }
        }

        private void CreateCommands()
        {
            this.SettingCommand = new RelayCommand(this.OpenSettings);
            this.RefreshDatabasesCommand = new RelayCommand(this.RefreshDatabases);
            this.AddDatabaseCommand = new RelayCommand(async () =>
            {
                await ShowDialog(GetAddDatabseView());
                this.UpdateDatabases();
            });
            this.SearchCommand = new RelayCommand(() =>
            {
                this.ShowSearch = !this.ShowSearch;
                if (!this.ShowSearch)
                    this.FilterText = string.Empty;
            });

            this.SizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(this.SizeChanged);
        }

        private void SizeChanged(SizeChangedEventArgs args)
        {
            // expand databases if window is bigger
            if (args.NewSize.Height > 600 && args.NewSize.Width > 650)
            {
                foreach (var d in this.Databases.Where(d => !d.ShowDetails))
                {
                    d.ToggleExpansion(true);
                }
            }
            else
            {
                foreach (var d in this.Databases.Where(d => d.ShowDetails))
                {
                    d.ToggleExpansion(true);
                }
            }
        }
    }
}