using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(List<SQLFileDisplayData>), "DiffFiles")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(SQLFileDisplayData), "SelectedDiffFile")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "ExecuteButtonEnabled")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "ExecuteButtonVisible", true)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Loading", false)]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Public, typeof(DatabaseDisplayData), "SelectedDatabase")]
    public partial class ImportWindowViewModel  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
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
        private List<SQLFileDisplayData> _DiffFiles;
        /// <summary>
        /// 
        /// </summary>
        public List<SQLFileDisplayData> DiffFiles
        {
            get
            {
                return this._DiffFiles;
            }
            set
            {
                if (!object.Equals(this._DiffFiles, value))
                {
                    //this.DiffFilesChanging(value);
                    this.DiffFilesBeforeSet(value);
                    this._DiffFiles = value;
                    this.OnPropertyChanged(DiffFilesEventArgs);
                    //this.DiffFilesChanged();
                    this.DiffFilesAfterSet();
                }
            }
        }

        partial void DiffFilesBeforeSet(List<SQLFileDisplayData> newValue);
        partial void DiffFilesAfterSet();

        //protected virtual void DiffFilesChanging(List<SQLFileDisplayData> newValue) { }
        //protected virtual void DiffFilesChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SelectedDiffFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedDiffFile));
        private SQLFileDisplayData _SelectedDiffFile;
        /// <summary>
        /// 
        /// </summary>
        public SQLFileDisplayData SelectedDiffFile
        {
            get
            {
                return this._SelectedDiffFile;
            }
            set
            {
                if (!object.Equals(this._SelectedDiffFile, value))
                {
                    //this.SelectedDiffFileChanging(value);
                    this.SelectedDiffFileBeforeSet(value);
                    this._SelectedDiffFile = value;
                    this.OnPropertyChanged(SelectedDiffFileEventArgs);
                    //this.SelectedDiffFileChanged();
                    this.SelectedDiffFileAfterSet();
                }
            }
        }

        partial void SelectedDiffFileBeforeSet(SQLFileDisplayData newValue);
        partial void SelectedDiffFileAfterSet();

        //protected virtual void SelectedDiffFileChanging(SQLFileDisplayData newValue) { }
        //protected virtual void SelectedDiffFileChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExecuteButtonEnabledEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ExecuteButtonEnabled));
        private bool _ExecuteButtonEnabled;
        /// <summary>
        /// 
        /// </summary>
        public bool ExecuteButtonEnabled
        {
            get
            {
                return this._ExecuteButtonEnabled;
            }
            set
            {
                if (!object.Equals(this._ExecuteButtonEnabled, value))
                {
                    //this.ExecuteButtonEnabledChanging(value);
                    this.ExecuteButtonEnabledBeforeSet(value);
                    this._ExecuteButtonEnabled = value;
                    this.OnPropertyChanged(ExecuteButtonEnabledEventArgs);
                    //this.ExecuteButtonEnabledChanged();
                    this.ExecuteButtonEnabledAfterSet();
                }
            }
        }

        partial void ExecuteButtonEnabledBeforeSet(bool newValue);
        partial void ExecuteButtonEnabledAfterSet();

        //protected virtual void ExecuteButtonEnabledChanging(bool newValue) { }
        //protected virtual void ExecuteButtonEnabledChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ExecuteButtonVisibleEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ExecuteButtonVisible));
        private bool _ExecuteButtonVisible = true;
        /// <summary>
        /// 
        /// </summary>
        public bool ExecuteButtonVisible
        {
            get
            {
                return this._ExecuteButtonVisible;
            }
            set
            {
                if (!object.Equals(this._ExecuteButtonVisible, value))
                {
                    //this.ExecuteButtonVisibleChanging(value);
                    this.ExecuteButtonVisibleBeforeSet(value);
                    this._ExecuteButtonVisible = value;
                    this.OnPropertyChanged(ExecuteButtonVisibleEventArgs);
                    //this.ExecuteButtonVisibleChanged();
                    this.ExecuteButtonVisibleAfterSet();
                }
            }
        }

        partial void ExecuteButtonVisibleBeforeSet(bool newValue);
        partial void ExecuteButtonVisibleAfterSet();

        //protected virtual void ExecuteButtonVisibleChanging(bool newValue) { }
        //protected virtual void ExecuteButtonVisibleChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs LoadingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Loading));
        private bool _Loading = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Loading
        {
            get
            {
                return this._Loading;
            }
            set
            {
                if (!object.Equals(this._Loading, value))
                {
                    //this.LoadingChanging(value);
                    this.LoadingBeforeSet(value);
                    this._Loading = value;
                    this.OnPropertyChanged(LoadingEventArgs);
                    //this.LoadingChanged();
                    this.LoadingAfterSet();
                }
            }
        }

        partial void LoadingBeforeSet(bool newValue);
        partial void LoadingAfterSet();

        //protected virtual void LoadingChanging(bool newValue) { }
        //protected virtual void LoadingChanged() { }

        //ncrunch: no coverage end
    }
}
