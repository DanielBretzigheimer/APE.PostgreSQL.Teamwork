// <copyright file="SettingViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows.Forms;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model.Setting;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Lets the user manage all settings.
    /// </summary>
    [NotifyProperty(typeof(string), "ConnectionString")]
    [NotifyProperty(typeof(string), "ConnectionStringPreview")]
    [NotifyProperty(typeof(string), "PgDump")]
    [NotifyProperty(typeof(string), "Password")]
    [NotifyProperty(typeof(string), "Host")]
    [NotifyProperty(typeof(string), "Id")]
    [NotifyProperty(typeof(int), "Port")]
    [NotifyProperty(typeof(string), "DatabaseFolderPath")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "OpenFilesDefault")]
    [NotifyProperty(AccessModifier.Public, typeof(bool), "AutoRefresh")]
    [NotifyProperty(AccessModifier.Public, typeof(string), "Message", null)]
    public partial class SettingViewModel : BaseViewModel
    {
        private const string ConfigExtension = "Config (*.config)|*.config";
        private const string ExeExtension = "Exe (*.exe)|*.exe";

        private IConnectionManager connectionManager;

        /// <summary>
        /// Reads all user settings and shows them to the user.
        /// </summary>
        public SettingViewModel(IConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager ?? throw new ArgumentNullException("connectionManager", "connectionManager == null");

            if (!this.connectionManager.CheckConnection())
            {
                this.Message = "Could not establish a connection to the default database. Check your Settings";
            }

            this.Load();

            this.SaveCommand = new RelayCommand(() => this.Save());
            this.SelectDefaultDatabaseFolderPathCommand = new RelayCommand(this.SelectDefaultDatabaseFolderPath);
            this.SelectPgDumpPathCommand = new RelayCommand(this.SelectPgDumpPath);
        }

        /// <summary>
        /// Gets a command which will save the settings.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets a command which will open a dialog to select a default database folder.
        /// </summary>
        public ICommand SelectDefaultDatabaseFolderPathCommand { get; private set; }

        /// <summary>
        /// Gets a command which will open a dialog to select the current path to the postgres dump exe.
        /// </summary>
        public ICommand SelectPgDumpPathCommand { get; private set; }

        /// <summary>
        /// Loads the settings from the user setting file into the properties.
        /// </summary>
        private void Load()
        {
            this.ConnectionString = SettingsManager.Get().Setting.ConnectionStringTemplate;
            this.PgDump = SettingsManager.Get().Setting.PgDumpLocation;
            this.Password = SettingsManager.Get().Setting.Password;
            this.Host = SettingsManager.Get().Setting.Host;
            this.Id = SettingsManager.Get().Setting.Id;
            this.Port = SettingsManager.Get().Setting.Port;
            this.OpenFilesDefault = SettingsManager.Get().Setting.OpenFilesInDefaultApplication;
            this.AutoRefresh = SettingsManager.Get().Setting.AutoRefresh;
            this.DatabaseFolderPath = SettingsManager.Get().Setting.DefaultDatabaseFolderPath;
            this.UpdateConnectionStringPreview();
        }

        partial void ConnectionStringAfterSet()
        {
            this.UpdateConnectionStringPreview();
        }

        partial void HostAfterSet()
        {
            this.UpdateConnectionStringPreview();
        }

        partial void IdAfterSet()
        {
            this.UpdateConnectionStringPreview();
        }

        partial void PasswordAfterSet()
        {
            this.UpdateConnectionStringPreview();
        }

        partial void PortAfterSet()
        {
            this.UpdateConnectionStringPreview();
        }

        private void UpdateConnectionStringPreview()
        {
            // update connection string preview
            this.ConnectionStringPreview = this.ConnectionString
                .Replace("[Id]", this.Id)
                .Replace("[Host]", this.Host)
                .Replace("[Password]", "***")
                .Replace("[Port]", this.Port.ToString());
        }

        /// <summary>
        /// Saves the changed settings in the user settings file.
        /// </summary>
        private void Save()
        {
            SettingsManager.Get().Setting.ConnectionStringTemplate = this.ConnectionString;
            SettingsManager.Get().Setting.PgDumpLocation = this.PgDump;
            SettingsManager.Get().Setting.Password = this.Password;
            SettingsManager.Get().Setting.Host = this.Host;
            SettingsManager.Get().Setting.Id = this.Id;
            SettingsManager.Get().Setting.Port = this.Port;
            SettingsManager.Get().Setting.OpenFilesInDefaultApplication = this.OpenFilesDefault;
            SettingsManager.Get().Setting.AutoRefresh = this.AutoRefresh;
            SettingsManager.Get().Setting.DefaultDatabaseFolderPath = this.DatabaseFolderPath;
            SettingsManager.Get().Save();

            // reinitialize the connection manager to work with the new settings
            this.connectionManager.Initialize(SettingsManager.Get().Setting.Id, SettingsManager.Get().Setting.Host, SettingsManager.Get().Setting.Password, SettingsManager.Get().Setting.Port);
        }

        /// <summary>
        /// Shows a dialog to select the postgres dump exe which is used to generate the dump files.
        /// </summary>
        private void SelectPgDumpPath()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = ExeExtension,
            };
            var result = dialog.ShowDialog();
            if (result != null && result == true)
            {
                this.PgDump = dialog.FileName;
            }
        }

        /// <summary>
        /// Shows a dialog to select a default database path which is used
        /// when the user has to choose a the directory for a new database.
        /// </summary>
        private void SelectDefaultDatabaseFolderPath()
        {
            var dialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.DatabaseFolderPath = dialog.SelectedPath;
        }
    }
}
