#nullable enable

using System.Collections.ObjectModel;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public partial class Database  : System.ComponentModel.INotifyPropertyChanged
    {
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private string databaseName;

        /// <summary>
        /// 
        /// </summary>
        private string databasePath;

        /// <summary>
        /// 
        /// </summary>
        private List<string> databaseIgnoredSchemas;

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
        private ISQLFileTester sqlFileTester;

        /// <summary>
        /// Indicates if the data should be initialized
        /// </summary>
        private bool initializeData;

        public Database(string databaseName, string databasePath, List<string> databaseIgnoredSchemas, IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sqlFileTester, bool initializeData)
        {
            if (databaseName == null)
                throw new System.ArgumentNullException("databaseName", "databaseName == null");
            this.databaseName = databaseName;

            if (databasePath == null)
                throw new System.ArgumentNullException("databasePath", "databasePath == null");
            this.databasePath = databasePath;

            if (databaseIgnoredSchemas == null)
                throw new System.ArgumentNullException("databaseIgnoredSchemas", "databaseIgnoredSchemas == null");
            this.databaseIgnoredSchemas = databaseIgnoredSchemas;

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

            if (sqlFileTester == null)
                throw new System.ArgumentNullException("sqlFileTester", "sqlFileTester == null");
            this.sqlFileTester = sqlFileTester;

            this.initializeData = initializeData;

            this.DatabaseCtor();
        }

        partial void DatabaseCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs NameEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Name));
        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (!object.Equals(this.name, value))
                {
                    this.NameBeforeSet(value);
                    this.name = value;
                    this.OnPropertyChanged(NameEventArgs);
                    this.NameAfterSet();
                }
            }
        }

        partial void NameBeforeSet(string newValue);
        partial void NameAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PathEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Path));
        private string path;

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if (!object.Equals(this.path, value))
                {
                    this.PathBeforeSet(value);
                    this.path = value;
                    this.OnPropertyChanged(PathEventArgs);
                    this.PathAfterSet();
                }
            }
        }

        partial void PathBeforeSet(string newValue);
        partial void PathAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs IgnoredSchemasEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(IgnoredSchemas));
        private ObservableCollection<string> ignoredSchemas;

        public ObservableCollection<string> IgnoredSchemas
        {
            get
            {
                return this.ignoredSchemas;
            }
            set
            {
                if (!object.Equals(this.ignoredSchemas, value))
                {
                    this.IgnoredSchemasBeforeSet(value);
                    this.ignoredSchemas = value;
                    this.OnPropertyChanged(IgnoredSchemasEventArgs);
                    this.IgnoredSchemasAfterSet();
                }
            }
        }

        partial void IgnoredSchemasBeforeSet(ObservableCollection<string> newValue);
        partial void IgnoredSchemasAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs LastApplicableVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(LastApplicableVersion));
        private DatabaseVersion lastApplicableVersion;

        public DatabaseVersion LastApplicableVersion
        {
            get
            {
                return this.lastApplicableVersion;
            }
            set
            {
                if (!object.Equals(this.lastApplicableVersion, value))
                {
                    this.LastApplicableVersionBeforeSet(value);
                    this.lastApplicableVersion = value;
                    this.OnPropertyChanged(LastApplicableVersionEventArgs);
                    this.LastApplicableVersionAfterSet();
                }
            }
        }

        partial void LastApplicableVersionBeforeSet(DatabaseVersion newValue);
        partial void LastApplicableVersionAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ProgressEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Progress));
        private double progress = 100;

        /// <summary>
        /// The progress of the current action. Not all actions are modifing it.
        /// </summary>
        public double Progress
        {
            get
            {
                return this.progress;
            }
            set
            {
                if (!object.Equals(this.progress, value))
                {
                    this.ProgressBeforeSet(value);
                    this.progress = value;
                    this.OnPropertyChanged(ProgressEventArgs);
                    this.ProgressAfterSet();
                }
            }
        }

        partial void ProgressBeforeSet(double newValue);
        partial void ProgressAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs CurrentVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentVersion));
        private DatabaseVersion currentVersion;

        public DatabaseVersion CurrentVersion
        {
            get => this.currentVersion;
            
            set
            {
                if (!object.Equals(this.currentVersion, value))
                {
                    this.CurrentVersionBeforeSet(value);
                    this.currentVersion = value;
                    this.OnPropertyChanged(CurrentVersionEventArgs);
                    this.CurrentVersionAfterSet();
                }
            }
        }

        partial void CurrentVersionBeforeSet(DatabaseVersion newValue);
        partial void CurrentVersionAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs UndoDiffFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(UndoDiffFiles));
        private ObservableCollection<SQLFile> undoDiffFiles = new ObservableCollection<SQLFile>();

        public ObservableCollection<SQLFile> UndoDiffFiles
        {
            get => this.undoDiffFiles;
            set
            {
                if (!object.Equals(this.undoDiffFiles, value))
                {
                    this.UndoDiffFilesBeforeSet(value);
                    this.undoDiffFiles = value;
                    this.OnPropertyChanged(UndoDiffFilesEventArgs);
                    this.UndoDiffFilesAfterSet();
                }
            }
        }

        partial void UndoDiffFilesBeforeSet(ObservableCollection<SQLFile> newValue);
        partial void UndoDiffFilesAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DiffFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DiffFiles));
        private ObservableCollection<SQLFile> diffFiles = new ObservableCollection<SQLFile>();

        public ObservableCollection<SQLFile> DiffFiles
        {
            get => this.diffFiles;
            set
            {
                if (!object.Equals(this.diffFiles, value))
                {
                    this.DiffFilesBeforeSet(value);
                    this.diffFiles = value;
                    this.OnPropertyChanged(DiffFilesEventArgs);
                    this.DiffFilesAfterSet();
                }
            }
        }

        partial void DiffFilesBeforeSet(ObservableCollection<SQLFile> newValue);
        partial void DiffFilesAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ProgressInfoEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ProgressInfo));
        private string progressInfo = string.Empty;

        /// <summary>
        /// This will be shown to the user as additional info to the current progress.
        /// </summary>
        public string ProgressInfo
        {
            get
            {
                return this.progressInfo;
            }
            set
            {
                if (!object.Equals(this.progressInfo, value))
                {
                    this.ProgressInfoBeforeSet(value);
                    this.progressInfo = value;
                    this.OnPropertyChanged(ProgressInfoEventArgs);
                    this.ProgressInfoAfterSet();
                }
            }
        }

        partial void ProgressInfoBeforeSet(string newValue);
        partial void ProgressInfoAfterSet();
        

        //--------------------------------------------------------------------------------
        // generated code for NotifyPropertySupport
        //--------------------------------------------------------------------------------

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        //ncrunch: no coverage end
    }
}
