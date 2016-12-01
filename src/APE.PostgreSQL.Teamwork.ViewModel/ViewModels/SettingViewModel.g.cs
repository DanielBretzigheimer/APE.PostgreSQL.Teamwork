using System;
using System.Windows.Forms;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model.Setting;

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
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(string), "Message", null)]
    public partial class SettingViewModel  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ConnectionStringEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ConnectionString));
        private string _ConnectionString;
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                if (!object.Equals(this._ConnectionString, value))
                {
                    //this.ConnectionStringChanging(value);
                    this.ConnectionStringBeforeSet(value);
                    this._ConnectionString = value;
                    this.OnPropertyChanged(ConnectionStringEventArgs);
                    //this.ConnectionStringChanged();
                    this.ConnectionStringAfterSet();
                }
            }
        }

        partial void ConnectionStringBeforeSet(string newValue);
        partial void ConnectionStringAfterSet();

        //protected virtual void ConnectionStringChanging(string newValue) { }
        //protected virtual void ConnectionStringChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ConnectionStringPreviewEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ConnectionStringPreview));
        private string _ConnectionStringPreview;
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionStringPreview
        {
            get
            {
                return this._ConnectionStringPreview;
            }
            set
            {
                if (!object.Equals(this._ConnectionStringPreview, value))
                {
                    //this.ConnectionStringPreviewChanging(value);
                    this.ConnectionStringPreviewBeforeSet(value);
                    this._ConnectionStringPreview = value;
                    this.OnPropertyChanged(ConnectionStringPreviewEventArgs);
                    //this.ConnectionStringPreviewChanged();
                    this.ConnectionStringPreviewAfterSet();
                }
            }
        }

        partial void ConnectionStringPreviewBeforeSet(string newValue);
        partial void ConnectionStringPreviewAfterSet();

        //protected virtual void ConnectionStringPreviewChanging(string newValue) { }
        //protected virtual void ConnectionStringPreviewChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PgDumpEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(PgDump));
        private string _PgDump;
        /// <summary>
        /// 
        /// </summary>
        public string PgDump
        {
            get
            {
                return this._PgDump;
            }
            set
            {
                if (!object.Equals(this._PgDump, value))
                {
                    //this.PgDumpChanging(value);
                    this.PgDumpBeforeSet(value);
                    this._PgDump = value;
                    this.OnPropertyChanged(PgDumpEventArgs);
                    //this.PgDumpChanged();
                    this.PgDumpAfterSet();
                }
            }
        }

        partial void PgDumpBeforeSet(string newValue);
        partial void PgDumpAfterSet();

        //protected virtual void PgDumpChanging(string newValue) { }
        //protected virtual void PgDumpChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PasswordEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Password));
        private string _Password;
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if (!object.Equals(this._Password, value))
                {
                    //this.PasswordChanging(value);
                    this.PasswordBeforeSet(value);
                    this._Password = value;
                    this.OnPropertyChanged(PasswordEventArgs);
                    //this.PasswordChanged();
                    this.PasswordAfterSet();
                }
            }
        }

        partial void PasswordBeforeSet(string newValue);
        partial void PasswordAfterSet();

        //protected virtual void PasswordChanging(string newValue) { }
        //protected virtual void PasswordChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs HostEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Host));
        private string _Host;
        /// <summary>
        /// 
        /// </summary>
        public string Host
        {
            get
            {
                return this._Host;
            }
            set
            {
                if (!object.Equals(this._Host, value))
                {
                    //this.HostChanging(value);
                    this.HostBeforeSet(value);
                    this._Host = value;
                    this.OnPropertyChanged(HostEventArgs);
                    //this.HostChanged();
                    this.HostAfterSet();
                }
            }
        }

        partial void HostBeforeSet(string newValue);
        partial void HostAfterSet();

        //protected virtual void HostChanging(string newValue) { }
        //protected virtual void HostChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs IdEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Id));
        private string _Id;
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if (!object.Equals(this._Id, value))
                {
                    //this.IdChanging(value);
                    this.IdBeforeSet(value);
                    this._Id = value;
                    this.OnPropertyChanged(IdEventArgs);
                    //this.IdChanged();
                    this.IdAfterSet();
                }
            }
        }

        partial void IdBeforeSet(string newValue);
        partial void IdAfterSet();

        //protected virtual void IdChanging(string newValue) { }
        //protected virtual void IdChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PortEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Port));
        private int _Port;
        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get
            {
                return this._Port;
            }
            set
            {
                if (!object.Equals(this._Port, value))
                {
                    //this.PortChanging(value);
                    this.PortBeforeSet(value);
                    this._Port = value;
                    this.OnPropertyChanged(PortEventArgs);
                    //this.PortChanged();
                    this.PortAfterSet();
                }
            }
        }

        partial void PortBeforeSet(int newValue);
        partial void PortAfterSet();

        //protected virtual void PortChanging(int newValue) { }
        //protected virtual void PortChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseFolderPathEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DatabaseFolderPath));
        private string _DatabaseFolderPath;
        /// <summary>
        /// 
        /// </summary>
        public string DatabaseFolderPath
        {
            get
            {
                return this._DatabaseFolderPath;
            }
            set
            {
                if (!object.Equals(this._DatabaseFolderPath, value))
                {
                    //this.DatabaseFolderPathChanging(value);
                    this.DatabaseFolderPathBeforeSet(value);
                    this._DatabaseFolderPath = value;
                    this.OnPropertyChanged(DatabaseFolderPathEventArgs);
                    //this.DatabaseFolderPathChanged();
                    this.DatabaseFolderPathAfterSet();
                }
            }
        }

        partial void DatabaseFolderPathBeforeSet(string newValue);
        partial void DatabaseFolderPathAfterSet();

        //protected virtual void DatabaseFolderPathChanging(string newValue) { }
        //protected virtual void DatabaseFolderPathChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs OpenFilesDefaultEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(OpenFilesDefault));
        private bool _OpenFilesDefault;
        /// <summary>
        /// 
        /// </summary>
        public bool OpenFilesDefault
        {
            get
            {
                return this._OpenFilesDefault;
            }
            set
            {
                if (!object.Equals(this._OpenFilesDefault, value))
                {
                    //this.OpenFilesDefaultChanging(value);
                    this.OpenFilesDefaultBeforeSet(value);
                    this._OpenFilesDefault = value;
                    this.OnPropertyChanged(OpenFilesDefaultEventArgs);
                    //this.OpenFilesDefaultChanged();
                    this.OpenFilesDefaultAfterSet();
                }
            }
        }

        partial void OpenFilesDefaultBeforeSet(bool newValue);
        partial void OpenFilesDefaultAfterSet();

        //protected virtual void OpenFilesDefaultChanging(bool newValue) { }
        //protected virtual void OpenFilesDefaultChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs AutoRefreshEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(AutoRefresh));
        private bool _AutoRefresh;
        /// <summary>
        /// 
        /// </summary>
        public bool AutoRefresh
        {
            get
            {
                return this._AutoRefresh;
            }
            set
            {
                if (!object.Equals(this._AutoRefresh, value))
                {
                    //this.AutoRefreshChanging(value);
                    this.AutoRefreshBeforeSet(value);
                    this._AutoRefresh = value;
                    this.OnPropertyChanged(AutoRefreshEventArgs);
                    //this.AutoRefreshChanged();
                    this.AutoRefreshAfterSet();
                }
            }
        }

        partial void AutoRefreshBeforeSet(bool newValue);
        partial void AutoRefreshAfterSet();

        //protected virtual void AutoRefreshChanging(bool newValue) { }
        //protected virtual void AutoRefreshChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs MessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Message));
        private string _Message = null;
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                if (!object.Equals(this._Message, value))
                {
                    //this.MessageChanging(value);
                    this.MessageBeforeSet(value);
                    this._Message = value;
                    this.OnPropertyChanged(MessageEventArgs);
                    //this.MessageChanged();
                    this.MessageAfterSet();
                }
            }
        }

        partial void MessageBeforeSet(string newValue);
        partial void MessageAfterSet();

        //protected virtual void MessageChanging(string newValue) { }
        //protected virtual void MessageChanged() { }

        //ncrunch: no coverage end
    }
}
