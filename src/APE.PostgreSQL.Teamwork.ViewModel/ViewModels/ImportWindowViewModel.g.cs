#nullable enable
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    public partial class ImportWindowViewModel  : System.ComponentModel.INotifyPropertyChanged
    {
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private IProcessManager processManager;

        /// <summary>
        /// 
        /// </summary>
        public DatabaseDisplayData SelectedDatabase { get; set; }

        public ImportWindowViewModel(IProcessManager processManager, DatabaseDisplayData SelectedDatabase)
        {
            if (processManager == null)
                throw new System.ArgumentNullException("processManager", "processManager == null");
            this.processManager = processManager;

            if (SelectedDatabase == null)
                throw new System.ArgumentNullException("SelectedDatabase", "SelectedDatabase == null");
            this.SelectedDatabase = SelectedDatabase;

            this.ImportWindowViewModelCtor();
        }

        partial void ImportWindowViewModelCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DiffFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DiffFiles));
        private List<SQLFileDisplayData> diffFiles;

        public List<SQLFileDisplayData> DiffFiles
        {
            get
            {
                return this.diffFiles;
            }
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

        partial void DiffFilesBeforeSet(List<SQLFileDisplayData> newValue);
        partial void DiffFilesAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExecuteButtonEnabledEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ExecuteButtonEnabled));
        private bool executeButtonEnabled;

        public bool ExecuteButtonEnabled
        {
            get
            {
                return this.executeButtonEnabled;
            }
            set
            {
                if (!object.Equals(this.executeButtonEnabled, value))
                {
                    this.ExecuteButtonEnabledBeforeSet(value);
                    this.executeButtonEnabled = value;
                    this.OnPropertyChanged(ExecuteButtonEnabledEventArgs);
                    this.ExecuteButtonEnabledAfterSet();
                }
            }
        }

        partial void ExecuteButtonEnabledBeforeSet(bool newValue);
        partial void ExecuteButtonEnabledAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExecuteButtonVisibleEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ExecuteButtonVisible));
        private bool executeButtonVisible = true;

        public bool ExecuteButtonVisible
        {
            get
            {
                return this.executeButtonVisible;
            }
            set
            {
                if (!object.Equals(this.executeButtonVisible, value))
                {
                    this.ExecuteButtonVisibleBeforeSet(value);
                    this.executeButtonVisible = value;
                    this.OnPropertyChanged(ExecuteButtonVisibleEventArgs);
                    this.ExecuteButtonVisibleAfterSet();
                }
            }
        }

        partial void ExecuteButtonVisibleBeforeSet(bool newValue);
        partial void ExecuteButtonVisibleAfterSet();
        

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
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SelectedDiffFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedDiffFile));
        private SQLFileDisplayData? selectedDiffFile;

        public SQLFileDisplayData? SelectedDiffFile
        {
            get
            {
                return this.selectedDiffFile;
            }
            set
            {
                if (!object.Equals(this.selectedDiffFile, value))
                {
                    this.SelectedDiffFileBeforeSet(value);
                    this.selectedDiffFile = value;
                    this.OnPropertyChanged(SelectedDiffFileEventArgs);
                    this.SelectedDiffFileAfterSet();
                }
            }
        }

        partial void SelectedDiffFileBeforeSet(SQLFileDisplayData? newValue);
        partial void SelectedDiffFileAfterSet();
        

        //ncrunch: no coverage end
    }
}
