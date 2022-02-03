#nullable enable

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "ConnectionString")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "ConnectionStringPreview")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "PgDump")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "Password")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "Host")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "Id")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(int), "Port")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "DatabaseFolderPath")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "OpenFilesDefault")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "AutoRefresh")]
    // APE.CodeGeneration.Attribute [AllowNullNotifyProperty(AccessModifier.Public, typeof(string), "Message", null)]
    public partial class SettingViewModel  : System.ComponentModel.INotifyPropertyChanged
    {
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ConnectionStringEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ConnectionString));
        private string connectionString;

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                if (!object.Equals(this.connectionString, value))
                {
                    this.ConnectionStringBeforeSet(value);
                    this.connectionString = value;
                    this.OnPropertyChanged(ConnectionStringEventArgs);
                    this.ConnectionStringAfterSet();
                }
            }
        }

        partial void ConnectionStringBeforeSet(string newValue);
        partial void ConnectionStringAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ConnectionStringPreviewEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ConnectionStringPreview));
        private string connectionStringPreview;

        public string ConnectionStringPreview
        {
            get
            {
                return this.connectionStringPreview;
            }
            set
            {
                if (!object.Equals(this.connectionStringPreview, value))
                {
                    this.ConnectionStringPreviewBeforeSet(value);
                    this.connectionStringPreview = value;
                    this.OnPropertyChanged(ConnectionStringPreviewEventArgs);
                    this.ConnectionStringPreviewAfterSet();
                }
            }
        }

        partial void ConnectionStringPreviewBeforeSet(string newValue);
        partial void ConnectionStringPreviewAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PgDumpEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(PgDump));
        private string pgDump;

        public string PgDump
        {
            get
            {
                return this.pgDump;
            }
            set
            {
                if (!object.Equals(this.pgDump, value))
                {
                    this.PgDumpBeforeSet(value);
                    this.pgDump = value;
                    this.OnPropertyChanged(PgDumpEventArgs);
                    this.PgDumpAfterSet();
                }
            }
        }

        partial void PgDumpBeforeSet(string newValue);
        partial void PgDumpAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PasswordEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Password));
        private string password;

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (!object.Equals(this.password, value))
                {
                    this.PasswordBeforeSet(value);
                    this.password = value;
                    this.OnPropertyChanged(PasswordEventArgs);
                    this.PasswordAfterSet();
                }
            }
        }

        partial void PasswordBeforeSet(string newValue);
        partial void PasswordAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs HostEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Host));
        private string host;

        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                if (!object.Equals(this.host, value))
                {
                    this.HostBeforeSet(value);
                    this.host = value;
                    this.OnPropertyChanged(HostEventArgs);
                    this.HostAfterSet();
                }
            }
        }

        partial void HostBeforeSet(string newValue);
        partial void HostAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs IdEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Id));
        private string id;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (!object.Equals(this.id, value))
                {
                    this.IdBeforeSet(value);
                    this.id = value;
                    this.OnPropertyChanged(IdEventArgs);
                    this.IdAfterSet();
                }
            }
        }

        partial void IdBeforeSet(string newValue);
        partial void IdAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PortEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Port));
        private int port;

        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (!object.Equals(this.port, value))
                {
                    this.PortBeforeSet(value);
                    this.port = value;
                    this.OnPropertyChanged(PortEventArgs);
                    this.PortAfterSet();
                }
            }
        }

        partial void PortBeforeSet(int newValue);
        partial void PortAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseFolderPathEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DatabaseFolderPath));
        private string databaseFolderPath;

        public string DatabaseFolderPath
        {
            get
            {
                return this.databaseFolderPath;
            }
            set
            {
                if (!object.Equals(this.databaseFolderPath, value))
                {
                    this.DatabaseFolderPathBeforeSet(value);
                    this.databaseFolderPath = value;
                    this.OnPropertyChanged(DatabaseFolderPathEventArgs);
                    this.DatabaseFolderPathAfterSet();
                }
            }
        }

        partial void DatabaseFolderPathBeforeSet(string newValue);
        partial void DatabaseFolderPathAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs OpenFilesDefaultEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(OpenFilesDefault));
        private bool openFilesDefault;

        public bool OpenFilesDefault
        {
            get
            {
                return this.openFilesDefault;
            }
            set
            {
                if (!object.Equals(this.openFilesDefault, value))
                {
                    this.OpenFilesDefaultBeforeSet(value);
                    this.openFilesDefault = value;
                    this.OnPropertyChanged(OpenFilesDefaultEventArgs);
                    this.OpenFilesDefaultAfterSet();
                }
            }
        }

        partial void OpenFilesDefaultBeforeSet(bool newValue);
        partial void OpenFilesDefaultAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs AutoRefreshEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(AutoRefresh));
        private bool autoRefresh;

        public bool AutoRefresh
        {
            get
            {
                return this.autoRefresh;
            }
            set
            {
                if (!object.Equals(this.autoRefresh, value))
                {
                    this.AutoRefreshBeforeSet(value);
                    this.autoRefresh = value;
                    this.OnPropertyChanged(AutoRefreshEventArgs);
                    this.AutoRefreshAfterSet();
                }
            }
        }

        partial void AutoRefreshBeforeSet(bool newValue);
        partial void AutoRefreshAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs MessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Message));
        private string? message = null;

        public string? Message
        {
            get
            {
                return this.message;
            }
            set
            {
                if (!object.Equals(this.message, value))
                {
                    this.MessageBeforeSet(value);
                    this.message = value;
                    this.OnPropertyChanged(MessageEventArgs);
                    this.MessageAfterSet();
                }
            }
        }

        partial void MessageBeforeSet(string? newValue);
        partial void MessageAfterSet();
        

        //ncrunch: no coverage end
    }
}
