#nullable enable
using System.Collections.ObjectModel;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

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
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(bool), "CanCreateMinor", false)]
    // APE.CodeGeneration.Attribute [AllowNullNotifyProperty(AccessModifier.Public, typeof(string), "SelectedSchema")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "ErrorMessage")]
    // APE.CodeGeneration.Attribute [AllowNullNotifyProperty(typeof(Database), "Database")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(DatabaseVersion), "TargetVersion")]
    // APE.CodeGeneration.Attribute [AllowNullNotifyProperty(AccessModifier.Public, typeof(List<DatabaseVersion>), "Versions")]
    // APE.CodeGeneration.Attribute [AllowNullNotifyProperty(typeof(ObservableCollection<SQLFileDisplayData>), "ApplicableSQLFiles")]
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
        private bool enabled = false;

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (!object.Equals(this.enabled, value))
                {
                    this.EnabledBeforeSet(value);
                    this.enabled = value;
                    this.OnPropertyChanged(EnabledEventArgs);
                    this.EnabledAfterSet();
                }
            }
        }

        partial void EnabledBeforeSet(bool newValue);
        partial void EnabledAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditModeEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(EditMode));
        private bool editMode = false;

        public bool EditMode
        {
            get
            {
                return this.editMode;
            }
            set
            {
                if (!object.Equals(this.editMode, value))
                {
                    this.EditModeBeforeSet(value);
                    this.editMode = value;
                    this.OnPropertyChanged(EditModeEventArgs);
                    this.EditModeAfterSet();
                }
            }
        }

        partial void EditModeBeforeSet(bool newValue);
        partial void EditModeAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowDetailsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowDetails));
        private bool showDetails = false;

        public bool ShowDetails
        {
            get
            {
                return this.showDetails;
            }
            set
            {
                if (!object.Equals(this.showDetails, value))
                {
                    this.ShowDetailsBeforeSet(value);
                    this.showDetails = value;
                    this.OnPropertyChanged(ShowDetailsEventArgs);
                    this.ShowDetailsAfterSet();
                }
            }
        }

        partial void ShowDetailsBeforeSet(bool newValue);
        partial void ShowDetailsAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExportingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Exporting));
        private bool exporting = false;

        /// <summary>
        /// Indicates that the database is exporting at the moment
        /// </summary>
        public bool Exporting
        {
            get
            {
                return this.exporting;
            }
            set
            {
                if (!object.Equals(this.exporting, value))
                {
                    this.ExportingBeforeSet(value);
                    this.exporting = value;
                    this.OnPropertyChanged(ExportingEventArgs);
                    this.ExportingAfterSet();
                }
            }
        }

        partial void ExportingBeforeSet(bool newValue);
        partial void ExportingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ImportingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Importing));
        private bool importing = false;

        /// <summary>
        /// Indicates that the database is importing at the moment
        /// </summary>
        public bool Importing
        {
            get
            {
                return this.importing;
            }
            set
            {
                if (!object.Equals(this.importing, value))
                {
                    this.ImportingBeforeSet(value);
                    this.importing = value;
                    this.OnPropertyChanged(ImportingEventArgs);
                    this.ImportingAfterSet();
                }
            }
        }

        partial void ImportingBeforeSet(bool newValue);
        partial void ImportingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs TestingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Testing));
        private bool testing = false;

        /// <summary>
        /// Indicates that the database is testing at the moment
        /// </summary>
        public bool Testing
        {
            get
            {
                return this.testing;
            }
            set
            {
                if (!object.Equals(this.testing, value))
                {
                    this.TestingBeforeSet(value);
                    this.testing = value;
                    this.OnPropertyChanged(TestingEventArgs);
                    this.TestingAfterSet();
                }
            }
        }

        partial void TestingBeforeSet(bool newValue);
        partial void TestingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ResettingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Resetting));
        private bool resetting = false;

        /// <summary>
        /// Indicates that the database is resetting at the moment
        /// </summary>
        public bool Resetting
        {
            get
            {
                return this.resetting;
            }
            set
            {
                if (!object.Equals(this.resetting, value))
                {
                    this.ResettingBeforeSet(value);
                    this.resetting = value;
                    this.OnPropertyChanged(ResettingEventArgs);
                    this.ResettingAfterSet();
                }
            }
        }

        partial void ResettingBeforeSet(bool newValue);
        partial void ResettingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs UndoingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Undoing));
        private bool undoing = false;

        /// <summary>
        /// Indicates that the database is undoing changes at the moment
        /// </summary>
        public bool Undoing
        {
            get
            {
                return this.undoing;
            }
            set
            {
                if (!object.Equals(this.undoing, value))
                {
                    this.UndoingBeforeSet(value);
                    this.undoing = value;
                    this.OnPropertyChanged(UndoingEventArgs);
                    this.UndoingAfterSet();
                }
            }
        }

        partial void UndoingBeforeSet(bool newValue);
        partial void UndoingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ErrorEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Error));
        private bool error = false;

        public bool Error
        {
            get
            {
                return this.error;
            }
            set
            {
                if (!object.Equals(this.error, value))
                {
                    this.ErrorBeforeSet(value);
                    this.error = value;
                    this.OnPropertyChanged(ErrorEventArgs);
                    this.ErrorAfterSet();
                }
            }
        }

        partial void ErrorBeforeSet(bool newValue);
        partial void ErrorAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ImportableFilesFoundEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ImportableFilesFound));
        private bool importableFilesFound = false;

        public bool ImportableFilesFound
        {
            get
            {
                return this.importableFilesFound;
            }
            set
            {
                if (!object.Equals(this.importableFilesFound, value))
                {
                    this.ImportableFilesFoundBeforeSet(value);
                    this.importableFilesFound = value;
                    this.OnPropertyChanged(ImportableFilesFoundEventArgs);
                    this.ImportableFilesFoundAfterSet();
                }
            }
        }

        partial void ImportableFilesFoundBeforeSet(bool newValue);
        partial void ImportableFilesFoundAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs CanCreateMinorEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(CanCreateMinor));
        private bool canCreateMinor = false;

        public bool CanCreateMinor
        {
            get
            {
                return this.canCreateMinor;
            }
            private set
            {
                if (!object.Equals(this.canCreateMinor, value))
                {
                    this.CanCreateMinorBeforeSet(value);
                    this.canCreateMinor = value;
                    this.OnPropertyChanged(CanCreateMinorEventArgs);
                    this.CanCreateMinorAfterSet();
                }
            }
        }

        partial void CanCreateMinorBeforeSet(bool newValue);
        partial void CanCreateMinorAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ErrorMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ErrorMessage));
        private string errorMessage;

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                if (!object.Equals(this.errorMessage, value))
                {
                    this.ErrorMessageBeforeSet(value);
                    this.errorMessage = value;
                    this.OnPropertyChanged(ErrorMessageEventArgs);
                    this.ErrorMessageAfterSet();
                }
            }
        }

        partial void ErrorMessageBeforeSet(string newValue);
        partial void ErrorMessageAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs TargetVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(TargetVersion));
        private DatabaseVersion targetVersion;

        public DatabaseVersion TargetVersion
        {
            get
            {
                return this.targetVersion;
            }
            set
            {
                if (!object.Equals(this.targetVersion, value))
                {
                    this.TargetVersionBeforeSet(value);
                    this.targetVersion = value;
                    this.OnPropertyChanged(TargetVersionEventArgs);
                    this.TargetVersionAfterSet();
                }
            }
        }

        partial void TargetVersionBeforeSet(DatabaseVersion newValue);
        partial void TargetVersionAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SelectedSchemaEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedSchema));
        private string selectedSchema;

        public string? SelectedSchema
        {
            get
            {
                return this.selectedSchema;
            }
            set
            {
                if (!object.Equals(this.selectedSchema, value))
                {
                    this.SelectedSchemaBeforeSet(value);
                    this.selectedSchema = value;
                    this.OnPropertyChanged(SelectedSchemaEventArgs);
                    this.SelectedSchemaAfterSet();
                }
            }
        }

        partial void SelectedSchemaBeforeSet(string newValue);
        partial void SelectedSchemaAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Database));
        private Database? database;

        public Database? Database
        {
            get
            {
                return this.database;
            }
            set
            {
                if (!object.Equals(this.database, value))
                {
                    this.DatabaseBeforeSet(value);
                    this.database = value;
                    this.OnPropertyChanged(DatabaseEventArgs);
                    this.DatabaseAfterSet();
                }
            }
        }

        partial void DatabaseBeforeSet(Database? newValue);
        partial void DatabaseAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs VersionsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Versions));
        private List<DatabaseVersion> versions;

        public List<DatabaseVersion>? Versions
        {
            get
            {
                return this.versions;
            }
            set
            {
                if (!object.Equals(this.versions, value))
                {
                    this.VersionsBeforeSet(value);
                    this.versions = value;
                    this.OnPropertyChanged(VersionsEventArgs);
                    this.VersionsAfterSet();
                }
            }
        }

        partial void VersionsBeforeSet(List<DatabaseVersion> newValue);
        partial void VersionsAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ApplicableSQLFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ApplicableSQLFiles));
        private ObservableCollection<SQLFileDisplayData> applicableSQLFiles;

        public ObservableCollection<SQLFileDisplayData>? ApplicableSQLFiles
        {
            get
            {
                return this.applicableSQLFiles;
            }
            set
            {
                if (!object.Equals(this.applicableSQLFiles, value))
                {
                    this.ApplicableSQLFilesBeforeSet(value);
                    this.applicableSQLFiles = value;
                    this.OnPropertyChanged(ApplicableSQLFilesEventArgs);
                    this.ApplicableSQLFilesAfterSet();
                }
            }
        }

        partial void ApplicableSQLFilesBeforeSet(ObservableCollection<SQLFileDisplayData> newValue);
        partial void ApplicableSQLFilesAfterSet();
        

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
