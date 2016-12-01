using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Enabled", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "EditMode", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "ShowDetails", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Exporting", false, "Indicates that the database is exporting at the moment")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Importing", false, "Indicates that the database is importing at the moment")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Testing", false, "Indicates that the database is testing at the moment")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Resetting", false, "Indicates that the database is resetting at the moment")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Undoing", false, "Indicates that the database is undoing changes at the moment")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Error", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "ImportableFilesFound", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "ErrorMessage")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(Database), "Database")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(DatabaseVersion), "TargetVersion")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(List<DatabaseVersion>), "Versions")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(ObservableCollection<SQLFileDisplayData>), "ApplicableSQLFiles")]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IConnectionManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IFileSystemAccess))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IDifferenceCreator))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(ISQLFileTester))]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Public, typeof(int), "Id", false, "id of the database for which the name and path are loaded")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class DatabaseDisplayData
  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private IConnectionManager connectionManager;

        /// <summary>
        /// 
        /// </summary>
        private IFileSystemAccess fileSystemAccess;

        /// <summary>
        /// 
        /// </summary>
        private IProcessManager processManager;

        /// <summary>
        /// 
        /// </summary>
        private IDifferenceCreator differenceCreator;

        /// <summary>
        /// 
        /// </summary>
        private ISQLFileTester sQLFileTester;

        /// <summary>
        /// id of the database for which the name and path are loaded
        /// </summary>
        public int Id { get; set; }

        public DatabaseDisplayData(IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sQLFileTester, int Id)
        {
            if (connectionManager == null)
                throw new System.ArgumentNullException("connectionManager", "connectionManager == null");
            this.connectionManager = connectionManager;

            if (fileSystemAccess == null)
                throw new System.ArgumentNullException("fileSystemAccess", "fileSystemAccess == null");
            this.fileSystemAccess = fileSystemAccess;

            if (processManager == null)
                throw new System.ArgumentNullException("processManager", "processManager == null");
            this.processManager = processManager;

            if (differenceCreator == null)
                throw new System.ArgumentNullException("differenceCreator", "differenceCreator == null");
            this.differenceCreator = differenceCreator;

            if (sQLFileTester == null)
                throw new System.ArgumentNullException("sQLFileTester", "sQLFileTester == null");
            this.sQLFileTester = sQLFileTester;

            this.Id = Id;

            this.DatabaseDisplayDataCtor();
        }

        partial void DatabaseDisplayDataCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EnabledEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Enabled));
        private bool _Enabled = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                if (!object.Equals(this._Enabled, value))
                {
                    //this.EnabledChanging(value);
                    this.EnabledBeforeSet(value);
                    this._Enabled = value;
                    this.OnPropertyChanged(EnabledEventArgs);
                    //this.EnabledChanged();
                    this.EnabledAfterSet();
                }
            }
        }

        partial void EnabledBeforeSet(bool newValue);
        partial void EnabledAfterSet();

        //protected virtual void EnabledChanging(bool newValue) { }
        //protected virtual void EnabledChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditModeEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(EditMode));
        private bool _EditMode = false;
        /// <summary>
        /// 
        /// </summary>
        public bool EditMode
        {
            get
            {
                return this._EditMode;
            }
            set
            {
                if (!object.Equals(this._EditMode, value))
                {
                    //this.EditModeChanging(value);
                    this.EditModeBeforeSet(value);
                    this._EditMode = value;
                    this.OnPropertyChanged(EditModeEventArgs);
                    //this.EditModeChanged();
                    this.EditModeAfterSet();
                }
            }
        }

        partial void EditModeBeforeSet(bool newValue);
        partial void EditModeAfterSet();

        //protected virtual void EditModeChanging(bool newValue) { }
        //protected virtual void EditModeChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowDetailsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowDetails));
        private bool _ShowDetails = false;
        /// <summary>
        /// 
        /// </summary>
        public bool ShowDetails
        {
            get
            {
                return this._ShowDetails;
            }
            set
            {
                if (!object.Equals(this._ShowDetails, value))
                {
                    //this.ShowDetailsChanging(value);
                    this.ShowDetailsBeforeSet(value);
                    this._ShowDetails = value;
                    this.OnPropertyChanged(ShowDetailsEventArgs);
                    //this.ShowDetailsChanged();
                    this.ShowDetailsAfterSet();
                }
            }
        }

        partial void ShowDetailsBeforeSet(bool newValue);
        partial void ShowDetailsAfterSet();

        //protected virtual void ShowDetailsChanging(bool newValue) { }
        //protected virtual void ShowDetailsChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExportingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Exporting));
        private bool _Exporting = false;
        /// <summary>
        /// Indicates that the database is exporting at the moment
        /// </summary>
        public bool Exporting
        {
            get
            {
                return this._Exporting;
            }
            set
            {
                if (!object.Equals(this._Exporting, value))
                {
                    //this.ExportingChanging(value);
                    this.ExportingBeforeSet(value);
                    this._Exporting = value;
                    this.OnPropertyChanged(ExportingEventArgs);
                    //this.ExportingChanged();
                    this.ExportingAfterSet();
                }
            }
        }

        partial void ExportingBeforeSet(bool newValue);
        partial void ExportingAfterSet();

        //protected virtual void ExportingChanging(bool newValue) { }
        //protected virtual void ExportingChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ImportingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Importing));
        private bool _Importing = false;
        /// <summary>
        /// Indicates that the database is importing at the moment
        /// </summary>
        public bool Importing
        {
            get
            {
                return this._Importing;
            }
            set
            {
                if (!object.Equals(this._Importing, value))
                {
                    //this.ImportingChanging(value);
                    this.ImportingBeforeSet(value);
                    this._Importing = value;
                    this.OnPropertyChanged(ImportingEventArgs);
                    //this.ImportingChanged();
                    this.ImportingAfterSet();
                }
            }
        }

        partial void ImportingBeforeSet(bool newValue);
        partial void ImportingAfterSet();

        //protected virtual void ImportingChanging(bool newValue) { }
        //protected virtual void ImportingChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs TestingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Testing));
        private bool _Testing = false;
        /// <summary>
        /// Indicates that the database is testing at the moment
        /// </summary>
        public bool Testing
        {
            get
            {
                return this._Testing;
            }
            set
            {
                if (!object.Equals(this._Testing, value))
                {
                    //this.TestingChanging(value);
                    this.TestingBeforeSet(value);
                    this._Testing = value;
                    this.OnPropertyChanged(TestingEventArgs);
                    //this.TestingChanged();
                    this.TestingAfterSet();
                }
            }
        }

        partial void TestingBeforeSet(bool newValue);
        partial void TestingAfterSet();

        //protected virtual void TestingChanging(bool newValue) { }
        //protected virtual void TestingChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ResettingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Resetting));
        private bool _Resetting = false;
        /// <summary>
        /// Indicates that the database is resetting at the moment
        /// </summary>
        public bool Resetting
        {
            get
            {
                return this._Resetting;
            }
            set
            {
                if (!object.Equals(this._Resetting, value))
                {
                    //this.ResettingChanging(value);
                    this.ResettingBeforeSet(value);
                    this._Resetting = value;
                    this.OnPropertyChanged(ResettingEventArgs);
                    //this.ResettingChanged();
                    this.ResettingAfterSet();
                }
            }
        }

        partial void ResettingBeforeSet(bool newValue);
        partial void ResettingAfterSet();

        //protected virtual void ResettingChanging(bool newValue) { }
        //protected virtual void ResettingChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs UndoingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Undoing));
        private bool _Undoing = false;
        /// <summary>
        /// Indicates that the database is undoing changes at the moment
        /// </summary>
        public bool Undoing
        {
            get
            {
                return this._Undoing;
            }
            set
            {
                if (!object.Equals(this._Undoing, value))
                {
                    //this.UndoingChanging(value);
                    this.UndoingBeforeSet(value);
                    this._Undoing = value;
                    this.OnPropertyChanged(UndoingEventArgs);
                    //this.UndoingChanged();
                    this.UndoingAfterSet();
                }
            }
        }

        partial void UndoingBeforeSet(bool newValue);
        partial void UndoingAfterSet();

        //protected virtual void UndoingChanging(bool newValue) { }
        //protected virtual void UndoingChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ErrorEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Error));
        private bool _Error = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Error
        {
            get
            {
                return this._Error;
            }
            set
            {
                if (!object.Equals(this._Error, value))
                {
                    //this.ErrorChanging(value);
                    this.ErrorBeforeSet(value);
                    this._Error = value;
                    this.OnPropertyChanged(ErrorEventArgs);
                    //this.ErrorChanged();
                    this.ErrorAfterSet();
                }
            }
        }

        partial void ErrorBeforeSet(bool newValue);
        partial void ErrorAfterSet();

        //protected virtual void ErrorChanging(bool newValue) { }
        //protected virtual void ErrorChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ImportableFilesFoundEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ImportableFilesFound));
        private bool _ImportableFilesFound = false;
        /// <summary>
        /// 
        /// </summary>
        public bool ImportableFilesFound
        {
            get
            {
                return this._ImportableFilesFound;
            }
            set
            {
                if (!object.Equals(this._ImportableFilesFound, value))
                {
                    //this.ImportableFilesFoundChanging(value);
                    this.ImportableFilesFoundBeforeSet(value);
                    this._ImportableFilesFound = value;
                    this.OnPropertyChanged(ImportableFilesFoundEventArgs);
                    //this.ImportableFilesFoundChanged();
                    this.ImportableFilesFoundAfterSet();
                }
            }
        }

        partial void ImportableFilesFoundBeforeSet(bool newValue);
        partial void ImportableFilesFoundAfterSet();

        //protected virtual void ImportableFilesFoundChanging(bool newValue) { }
        //protected virtual void ImportableFilesFoundChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ErrorMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ErrorMessage));
        private string _ErrorMessage;
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this._ErrorMessage;
            }
            set
            {
                if (!object.Equals(this._ErrorMessage, value))
                {
                    //this.ErrorMessageChanging(value);
                    this.ErrorMessageBeforeSet(value);
                    this._ErrorMessage = value;
                    this.OnPropertyChanged(ErrorMessageEventArgs);
                    //this.ErrorMessageChanged();
                    this.ErrorMessageAfterSet();
                }
            }
        }

        partial void ErrorMessageBeforeSet(string newValue);
        partial void ErrorMessageAfterSet();

        //protected virtual void ErrorMessageChanging(string newValue) { }
        //protected virtual void ErrorMessageChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Database));
        private Database _Database;
        /// <summary>
        /// 
        /// </summary>
        public Database Database
        {
            get
            {
                return this._Database;
            }
            set
            {
                if (!object.Equals(this._Database, value))
                {
                    //this.DatabaseChanging(value);
                    this.DatabaseBeforeSet(value);
                    this._Database = value;
                    this.OnPropertyChanged(DatabaseEventArgs);
                    //this.DatabaseChanged();
                    this.DatabaseAfterSet();
                }
            }
        }

        partial void DatabaseBeforeSet(Database newValue);
        partial void DatabaseAfterSet();

        //protected virtual void DatabaseChanging(Database newValue) { }
        //protected virtual void DatabaseChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs TargetVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(TargetVersion));
        private DatabaseVersion _TargetVersion;
        /// <summary>
        /// 
        /// </summary>
        public DatabaseVersion TargetVersion
        {
            get
            {
                return this._TargetVersion;
            }
            set
            {
                if (!object.Equals(this._TargetVersion, value))
                {
                    //this.TargetVersionChanging(value);
                    this.TargetVersionBeforeSet(value);
                    this._TargetVersion = value;
                    this.OnPropertyChanged(TargetVersionEventArgs);
                    //this.TargetVersionChanged();
                    this.TargetVersionAfterSet();
                }
            }
        }

        partial void TargetVersionBeforeSet(DatabaseVersion newValue);
        partial void TargetVersionAfterSet();

        //protected virtual void TargetVersionChanging(DatabaseVersion newValue) { }
        //protected virtual void TargetVersionChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs VersionsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Versions));
        private List<DatabaseVersion> _Versions;
        /// <summary>
        /// 
        /// </summary>
        public List<DatabaseVersion> Versions
        {
            get
            {
                return this._Versions;
            }
            set
            {
                if (!object.Equals(this._Versions, value))
                {
                    //this.VersionsChanging(value);
                    this.VersionsBeforeSet(value);
                    this._Versions = value;
                    this.OnPropertyChanged(VersionsEventArgs);
                    //this.VersionsChanged();
                    this.VersionsAfterSet();
                }
            }
        }

        partial void VersionsBeforeSet(List<DatabaseVersion> newValue);
        partial void VersionsAfterSet();

        //protected virtual void VersionsChanging(List<DatabaseVersion> newValue) { }
        //protected virtual void VersionsChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ApplicableSQLFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ApplicableSQLFiles));
        private ObservableCollection<SQLFileDisplayData> _ApplicableSQLFiles;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SQLFileDisplayData> ApplicableSQLFiles
        {
            get
            {
                return this._ApplicableSQLFiles;
            }
            set
            {
                if (!object.Equals(this._ApplicableSQLFiles, value))
                {
                    //this.ApplicableSQLFilesChanging(value);
                    this.ApplicableSQLFilesBeforeSet(value);
                    this._ApplicableSQLFiles = value;
                    this.OnPropertyChanged(ApplicableSQLFilesEventArgs);
                    //this.ApplicableSQLFilesChanged();
                    this.ApplicableSQLFilesAfterSet();
                }
            }
        }

        partial void ApplicableSQLFilesBeforeSet(ObservableCollection<SQLFileDisplayData> newValue);
        partial void ApplicableSQLFilesAfterSet();

        //protected virtual void ApplicableSQLFilesChanging(ObservableCollection<SQLFileDisplayData> newValue) { }
        //protected virtual void ApplicableSQLFilesChanged() { }

        //--------------------------------------------------------------------------------
        // generated code for NotifyPropertySupport
        //--------------------------------------------------------------------------------

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        //ncrunch: no coverage end
    }
}
