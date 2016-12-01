using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using log4net;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "Name")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "Path")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(DatabaseVersion), "CurrentVersion")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(DatabaseVersion), "LastApplicableVersion")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(ObservableCollection<SQLFile>), "UndoDiffFiles")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(ObservableCollection<SQLFile>), "DiffFiles")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(double), "Progress", 100, "The progress of the current action. Not all actions are modifing it.")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(string), "ProgressInfo", "", "This will be shown to the user as additional info to the current progress.")]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Private, typeof(string), "name")]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Private, typeof(string), "path")]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IConnectionManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IFileSystemAccess))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IDifferenceCreator))]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Private, typeof(ISQLFileTester), "sqlFileTester")]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Private, typeof(bool), "initializeData", false, "Indicates if the data should be initialized")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class Database  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private string name;

        /// <summary>
        /// 
        /// </summary>
        private string path;

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

        public Database(string name, string path, IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sqlFileTester, bool initializeData)
        {
            if (name == null)
                throw new System.ArgumentNullException("name", "name == null");
            this.name = name;

            if (path == null)
                throw new System.ArgumentNullException("path", "path == null");
            this.path = path;

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
        private string _Name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if (!object.Equals(this._Name, value))
                {
                    //this.NameChanging(value);
                    this.NameBeforeSet(value);
                    this._Name = value;
                    this.OnPropertyChanged(NameEventArgs);
                    //this.NameChanged();
                    this.NameAfterSet();
                }
            }
        }

        partial void NameBeforeSet(string newValue);
        partial void NameAfterSet();

        //protected virtual void NameChanging(string newValue) { }
        //protected virtual void NameChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs PathEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Path));
        private string _Path;
        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get
            {
                return this._Path;
            }
            set
            {
                if (!object.Equals(this._Path, value))
                {
                    //this.PathChanging(value);
                    this.PathBeforeSet(value);
                    this._Path = value;
                    this.OnPropertyChanged(PathEventArgs);
                    //this.PathChanged();
                    this.PathAfterSet();
                }
            }
        }

        partial void PathBeforeSet(string newValue);
        partial void PathAfterSet();

        //protected virtual void PathChanging(string newValue) { }
        //protected virtual void PathChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs CurrentVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentVersion));
        private DatabaseVersion _CurrentVersion;
        /// <summary>
        /// 
        /// </summary>
        public DatabaseVersion CurrentVersion
        {
            get
            {
                return this._CurrentVersion;
            }
            set
            {
                if (!object.Equals(this._CurrentVersion, value))
                {
                    //this.CurrentVersionChanging(value);
                    this.CurrentVersionBeforeSet(value);
                    this._CurrentVersion = value;
                    this.OnPropertyChanged(CurrentVersionEventArgs);
                    //this.CurrentVersionChanged();
                    this.CurrentVersionAfterSet();
                }
            }
        }

        partial void CurrentVersionBeforeSet(DatabaseVersion newValue);
        partial void CurrentVersionAfterSet();

        //protected virtual void CurrentVersionChanging(DatabaseVersion newValue) { }
        //protected virtual void CurrentVersionChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs LastApplicableVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(LastApplicableVersion));
        private DatabaseVersion _LastApplicableVersion;
        /// <summary>
        /// 
        /// </summary>
        public DatabaseVersion LastApplicableVersion
        {
            get
            {
                return this._LastApplicableVersion;
            }
            set
            {
                if (!object.Equals(this._LastApplicableVersion, value))
                {
                    //this.LastApplicableVersionChanging(value);
                    this.LastApplicableVersionBeforeSet(value);
                    this._LastApplicableVersion = value;
                    this.OnPropertyChanged(LastApplicableVersionEventArgs);
                    //this.LastApplicableVersionChanged();
                    this.LastApplicableVersionAfterSet();
                }
            }
        }

        partial void LastApplicableVersionBeforeSet(DatabaseVersion newValue);
        partial void LastApplicableVersionAfterSet();

        //protected virtual void LastApplicableVersionChanging(DatabaseVersion newValue) { }
        //protected virtual void LastApplicableVersionChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs UndoDiffFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(UndoDiffFiles));
        private ObservableCollection<SQLFile> _UndoDiffFiles;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SQLFile> UndoDiffFiles
        {
            get
            {
                return this._UndoDiffFiles;
            }
            set
            {
                if (!object.Equals(this._UndoDiffFiles, value))
                {
                    //this.UndoDiffFilesChanging(value);
                    this.UndoDiffFilesBeforeSet(value);
                    this._UndoDiffFiles = value;
                    this.OnPropertyChanged(UndoDiffFilesEventArgs);
                    //this.UndoDiffFilesChanged();
                    this.UndoDiffFilesAfterSet();
                }
            }
        }

        partial void UndoDiffFilesBeforeSet(ObservableCollection<SQLFile> newValue);
        partial void UndoDiffFilesAfterSet();

        //protected virtual void UndoDiffFilesChanging(ObservableCollection<SQLFile> newValue) { }
        //protected virtual void UndoDiffFilesChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DiffFilesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DiffFiles));
        private ObservableCollection<SQLFile> _DiffFiles;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SQLFile> DiffFiles
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

        partial void DiffFilesBeforeSet(ObservableCollection<SQLFile> newValue);
        partial void DiffFilesAfterSet();

        //protected virtual void DiffFilesChanging(ObservableCollection<SQLFile> newValue) { }
        //protected virtual void DiffFilesChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ProgressEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Progress));
        private double _Progress = 100;
        /// <summary>
        /// The progress of the current action. Not all actions are modifing it.
        /// </summary>
        public double Progress
        {
            get
            {
                return this._Progress;
            }
            set
            {
                if (!object.Equals(this._Progress, value))
                {
                    //this.ProgressChanging(value);
                    this.ProgressBeforeSet(value);
                    this._Progress = value;
                    this.OnPropertyChanged(ProgressEventArgs);
                    //this.ProgressChanged();
                    this.ProgressAfterSet();
                }
            }
        }

        partial void ProgressBeforeSet(double newValue);
        partial void ProgressAfterSet();

        //protected virtual void ProgressChanging(double newValue) { }
        //protected virtual void ProgressChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ProgressInfoEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ProgressInfo));
        private string _ProgressInfo = "";
        /// <summary>
        /// This will be shown to the user as additional info to the current progress.
        /// </summary>
        public string ProgressInfo
        {
            get
            {
                return this._ProgressInfo;
            }
            set
            {
                if (!object.Equals(this._ProgressInfo, value))
                {
                    //this.ProgressInfoChanging(value);
                    this.ProgressInfoBeforeSet(value);
                    this._ProgressInfo = value;
                    this.OnPropertyChanged(ProgressInfoEventArgs);
                    //this.ProgressInfoChanged();
                    this.ProgressInfoAfterSet();
                }
            }
        }

        partial void ProgressInfoBeforeSet(string newValue);
        partial void ProgressInfoAfterSet();

        //protected virtual void ProgressInfoChanging(string newValue) { }
        //protected virtual void ProgressInfoChanged() { }

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
