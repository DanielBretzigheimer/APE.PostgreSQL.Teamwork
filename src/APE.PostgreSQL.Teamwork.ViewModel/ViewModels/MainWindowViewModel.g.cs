#nullable enable
using System.Windows;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public partial class MainWindowViewModel  : System.ComponentModel.INotifyPropertyChanged
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

        public MainWindowViewModel(IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sQLFileTester)
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

            this.MainWindowViewModelCtor();
        }

        partial void MainWindowViewModelCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ErrorMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ErrorMessage));
        private string errorMessage = "";

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            private set
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
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SuccessMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SuccessMessage));
        private string successMessage = "";

        public string SuccessMessage
        {
            get
            {
                return this.successMessage;
            }
            private set
            {
                if (!object.Equals(this.successMessage, value))
                {
                    this.SuccessMessageBeforeSet(value);
                    this.successMessage = value;
                    this.OnPropertyChanged(SuccessMessageEventArgs);
                    this.SuccessMessageAfterSet();
                }
            }
        }

        partial void SuccessMessageBeforeSet(string newValue);
        partial void SuccessMessageAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditableEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Editable));
        private bool editable = false;

        public bool Editable
        {
            get
            {
                return this.editable;
            }
            set
            {
                if (!object.Equals(this.editable, value))
                {
                    this.EditableBeforeSet(value);
                    this.editable = value;
                    this.OnPropertyChanged(EditableEventArgs);
                    this.EditableAfterSet();
                }
            }
        }

        partial void EditableBeforeSet(bool newValue);
        partial void EditableAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs LoadingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Loading));
        private bool loading = false;

        public bool Loading
        {
            get
            {
                return this.loading;
            }
            set
            {
                if (!object.Equals(this.loading, value))
                {
                    this.LoadingBeforeSet(value);
                    this.loading = value;
                    this.OnPropertyChanged(LoadingEventArgs);
                    this.LoadingAfterSet();
                }
            }
        }

        partial void LoadingBeforeSet(bool newValue);
        partial void LoadingAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowSearchEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowSearch));
        private bool showSearch = false;

        public bool ShowSearch
        {
            get
            {
                return this.showSearch;
            }
            set
            {
                if (!object.Equals(this.showSearch, value))
                {
                    this.ShowSearchBeforeSet(value);
                    this.showSearch = value;
                    this.OnPropertyChanged(ShowSearchEventArgs);
                    this.ShowSearchAfterSet();
                }
            }
        }

        partial void ShowSearchBeforeSet(bool newValue);
        partial void ShowSearchAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs FilterTextEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(FilterText));
        private string filterText = "";

        public string FilterText
        {
            get
            {
                return this.filterText;
            }
            set
            {
                if (!object.Equals(this.filterText, value))
                {
                    this.FilterTextBeforeSet(value);
                    this.filterText = value;
                    this.OnPropertyChanged(FilterTextEventArgs);
                    this.FilterTextAfterSet();
                }
            }
        }

        partial void FilterTextBeforeSet(string newValue);
        partial void FilterTextAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditButtonEnabledEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(EditButtonEnabled));
        private bool editButtonEnabled = true;

        public bool EditButtonEnabled
        {
            get
            {
                return this.editButtonEnabled;
            }
            set
            {
                if (!object.Equals(this.editButtonEnabled, value))
                {
                    this.EditButtonEnabledBeforeSet(value);
                    this.editButtonEnabled = value;
                    this.OnPropertyChanged(EditButtonEnabledEventArgs);
                    this.EditButtonEnabledAfterSet();
                }
            }
        }

        partial void EditButtonEnabledBeforeSet(bool newValue);
        partial void EditButtonEnabledAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SaveButtonVisibilityEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SaveButtonVisibility));
        private Visibility saveButtonVisibility = Visibility.Hidden;

        public Visibility SaveButtonVisibility
        {
            get
            {
                return this.saveButtonVisibility;
            }
            set
            {
                if (!object.Equals(this.saveButtonVisibility, value))
                {
                    this.SaveButtonVisibilityBeforeSet(value);
                    this.saveButtonVisibility = value;
                    this.OnPropertyChanged(SaveButtonVisibilityEventArgs);
                    this.SaveButtonVisibilityAfterSet();
                }
            }
        }

        partial void SaveButtonVisibilityBeforeSet(Visibility newValue);
        partial void SaveButtonVisibilityAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabasesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Databases));
        private List<DatabaseDisplayData>? databases;

        public List<DatabaseDisplayData>? Databases
        {
            get
            {
                return this.databases;
            }
            set
            {
                if (!object.Equals(this.databases, value))
                {
                    this.DatabasesBeforeSet(value);
                    this.databases = value;
                    this.OnPropertyChanged(DatabasesEventArgs);
                    this.DatabasesAfterSet();
                }
            }
        }

        partial void DatabasesBeforeSet(List<DatabaseDisplayData>? newValue);
        partial void DatabasesAfterSet();
        

        //--------------------------------------------------------------------------------
        // generated code for Startable
        //--------------------------------------------------------------------------------

        public event System.EventHandler Started;

        // disable c# compile warning 'field is assigned, but not used'
#pragma warning disable 414
        private bool isStarting = false;

        private bool isStopping = false;
#pragma warning restore 414

        public bool IsStarted { get; private set; }

        partial void StartGenerated();
        partial void StopGenerated();

        [System.Diagnostics.DebuggerStepThrough()]
        public void Start()
        {
            if (this.IsStarted)
                throw new System.InvalidOperationException(this.GetType().Name + " is already started -> cannot start it twice.");
            this.isStarting = true;

            this.StartGenerated();

            this.OnStarted();
            this.isStarting = false;
        }

        private void OnStarted()
        {
            this.IsStarted = true;

            if (this.Started != null)
                this.Started(this, System.EventArgs.Empty);
        }

        [System.Diagnostics.DebuggerStepThrough()]
        public void Stop()
        {
            if (this.IsStarted == false)
                throw new System.InvalidOperationException(this.GetType().Name + " is not started -> cannot stop it.");

            this.isStopping = true;

            this.StopGenerated();

            this.IsStarted = false;
            this.isStopping = false;
        }

        //ncrunch: no coverage end
    }
}
