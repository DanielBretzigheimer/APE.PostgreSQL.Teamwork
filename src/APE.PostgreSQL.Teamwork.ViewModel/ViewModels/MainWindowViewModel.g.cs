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
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "ErrorMessage")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "SuccessMessage")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Editable", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "Loading", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "ShowSearch", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(string), "FilterText", "")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(List<DatabaseDisplayData>), "Databases")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "EditButtonEnabled", true)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(Visibility), "SaveButtonVisibility", Visibility.Hidden)]
    // APE.CodeGeneration.Attribute [Startable]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IConnectionManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IFileSystemAccess))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IDifferenceCreator))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(ISQLFileTester))]
    public partial class MainWindowViewModel  : System.ComponentModel.INotifyPropertyChanged, IStartable
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

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SuccessMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SuccessMessage));
        private string _SuccessMessage;
        /// <summary>
        /// 
        /// </summary>
        public string SuccessMessage
        {
            get
            {
                return this._SuccessMessage;
            }
            set
            {
                if (!object.Equals(this._SuccessMessage, value))
                {
                    //this.SuccessMessageChanging(value);
                    this.SuccessMessageBeforeSet(value);
                    this._SuccessMessage = value;
                    this.OnPropertyChanged(SuccessMessageEventArgs);
                    //this.SuccessMessageChanged();
                    this.SuccessMessageAfterSet();
                }
            }
        }

        partial void SuccessMessageBeforeSet(string newValue);
        partial void SuccessMessageAfterSet();

        //protected virtual void SuccessMessageChanging(string newValue) { }
        //protected virtual void SuccessMessageChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditableEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Editable));
        private bool _Editable = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Editable
        {
            get
            {
                return this._Editable;
            }
            set
            {
                if (!object.Equals(this._Editable, value))
                {
                    //this.EditableChanging(value);
                    this.EditableBeforeSet(value);
                    this._Editable = value;
                    this.OnPropertyChanged(EditableEventArgs);
                    //this.EditableChanged();
                    this.EditableAfterSet();
                }
            }
        }

        partial void EditableBeforeSet(bool newValue);
        partial void EditableAfterSet();

        //protected virtual void EditableChanging(bool newValue) { }
        //protected virtual void EditableChanged() { }

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

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowSearchEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowSearch));
        private bool _ShowSearch = false;
        /// <summary>
        /// 
        /// </summary>
        public bool ShowSearch
        {
            get
            {
                return this._ShowSearch;
            }
            set
            {
                if (!object.Equals(this._ShowSearch, value))
                {
                    //this.ShowSearchChanging(value);
                    this.ShowSearchBeforeSet(value);
                    this._ShowSearch = value;
                    this.OnPropertyChanged(ShowSearchEventArgs);
                    //this.ShowSearchChanged();
                    this.ShowSearchAfterSet();
                }
            }
        }

        partial void ShowSearchBeforeSet(bool newValue);
        partial void ShowSearchAfterSet();

        //protected virtual void ShowSearchChanging(bool newValue) { }
        //protected virtual void ShowSearchChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs FilterTextEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(FilterText));
        private string _FilterText = "";
        /// <summary>
        /// 
        /// </summary>
        public string FilterText
        {
            get
            {
                return this._FilterText;
            }
            set
            {
                if (!object.Equals(this._FilterText, value))
                {
                    //this.FilterTextChanging(value);
                    this.FilterTextBeforeSet(value);
                    this._FilterText = value;
                    this.OnPropertyChanged(FilterTextEventArgs);
                    //this.FilterTextChanged();
                    this.FilterTextAfterSet();
                }
            }
        }

        partial void FilterTextBeforeSet(string newValue);
        partial void FilterTextAfterSet();

        //protected virtual void FilterTextChanging(string newValue) { }
        //protected virtual void FilterTextChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabasesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Databases));
        private List<DatabaseDisplayData> _Databases;
        /// <summary>
        /// 
        /// </summary>
        public List<DatabaseDisplayData> Databases
        {
            get
            {
                return this._Databases;
            }
            set
            {
                if (!object.Equals(this._Databases, value))
                {
                    //this.DatabasesChanging(value);
                    this.DatabasesBeforeSet(value);
                    this._Databases = value;
                    this.OnPropertyChanged(DatabasesEventArgs);
                    //this.DatabasesChanged();
                    this.DatabasesAfterSet();
                }
            }
        }

        partial void DatabasesBeforeSet(List<DatabaseDisplayData> newValue);
        partial void DatabasesAfterSet();

        //protected virtual void DatabasesChanging(List<DatabaseDisplayData> newValue) { }
        //protected virtual void DatabasesChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditButtonEnabledEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(EditButtonEnabled));
        private bool _EditButtonEnabled = true;
        /// <summary>
        /// 
        /// </summary>
        public bool EditButtonEnabled
        {
            get
            {
                return this._EditButtonEnabled;
            }
            set
            {
                if (!object.Equals(this._EditButtonEnabled, value))
                {
                    //this.EditButtonEnabledChanging(value);
                    this.EditButtonEnabledBeforeSet(value);
                    this._EditButtonEnabled = value;
                    this.OnPropertyChanged(EditButtonEnabledEventArgs);
                    //this.EditButtonEnabledChanged();
                    this.EditButtonEnabledAfterSet();
                }
            }
        }

        partial void EditButtonEnabledBeforeSet(bool newValue);
        partial void EditButtonEnabledAfterSet();

        //protected virtual void EditButtonEnabledChanging(bool newValue) { }
        //protected virtual void EditButtonEnabledChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SaveButtonVisibilityEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SaveButtonVisibility));
        private Visibility _SaveButtonVisibility = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility SaveButtonVisibility
        {
            get
            {
                return this._SaveButtonVisibility;
            }
            set
            {
                if (!object.Equals(this._SaveButtonVisibility, value))
                {
                    //this.SaveButtonVisibilityChanging(value);
                    this.SaveButtonVisibilityBeforeSet(value);
                    this._SaveButtonVisibility = value;
                    this.OnPropertyChanged(SaveButtonVisibilityEventArgs);
                    //this.SaveButtonVisibilityChanged();
                    this.SaveButtonVisibilityAfterSet();
                }
            }
        }

        partial void SaveButtonVisibilityBeforeSet(Visibility newValue);
        partial void SaveButtonVisibilityAfterSet();

        //protected virtual void SaveButtonVisibilityChanging(Visibility newValue) { }
        //protected virtual void SaveButtonVisibilityChanged() { }

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

        partial void StartCore();
        partial void StopCore();

        //this needs to be virtual for child classes (because StartCore cannot be overriden.)
        [System.Diagnostics.DebuggerStepThrough()]
        public virtual void Start()
        {
            this.isStarting = true;
            this.StartCore();
            this.OnStarted();
            this.isStarting = false;
        }

        private void OnStarted()
        {
            if (this.IsStarted)
                throw new InvalidOperationException(this.GetType().Name + " is already started.");

            this.IsStarted = true;

            if (this.Started != null)
                this.Started(this, System.EventArgs.Empty);
        }

        //this needs to be virtual for child classes (because StopCore cannot be overriden.)
        [System.Diagnostics.DebuggerStepThrough()]
        public virtual void Stop()
        {
            if (this.IsStarted == false)
                throw new InvalidOperationException(this.GetType().Name + " is already stopped.");

            this.isStopping = true;

            this.StopCore();

            this.IsStarted = false;
            this.isStopping = false;
        }

        //ncrunch: no coverage end
    }
}
