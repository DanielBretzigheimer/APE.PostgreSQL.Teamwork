using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.Model.Setting;
using APE.PostgreSQL.Teamwork.Model.Templates;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "DatabaseName")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "DatabasePath")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(List<string>), "Databases")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "DataChecked", false)]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.Public, typeof(bool), "DatabaseExists", false)]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IConnectionManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IFileSystemAccess))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IDifferenceCreator))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(ISQLFileTester))]
    public partial class AddDatabaseViewModel  : System.ComponentModel.INotifyPropertyChanged
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

        public AddDatabaseViewModel(IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sQLFileTester)
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

            this.AddDatabaseViewModelCtor();
        }

        partial void AddDatabaseViewModelCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseNameEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DatabaseName));
        private string _DatabaseName;
        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return this._DatabaseName;
            }
            set
            {
                if (!object.Equals(this._DatabaseName, value))
                {
                    //this.DatabaseNameChanging(value);
                    this.DatabaseNameBeforeSet(value);
                    this._DatabaseName = value;
                    this.OnPropertyChanged(DatabaseNameEventArgs);
                    //this.DatabaseNameChanged();
                    this.DatabaseNameAfterSet();
                }
            }
        }

        partial void DatabaseNameBeforeSet(string newValue);
        partial void DatabaseNameAfterSet();

        //protected virtual void DatabaseNameChanging(string newValue) { }
        //protected virtual void DatabaseNameChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabasePathEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DatabasePath));
        private string _DatabasePath;
        /// <summary>
        /// 
        /// </summary>
        public string DatabasePath
        {
            get
            {
                return this._DatabasePath;
            }
            set
            {
                if (!object.Equals(this._DatabasePath, value))
                {
                    //this.DatabasePathChanging(value);
                    this.DatabasePathBeforeSet(value);
                    this._DatabasePath = value;
                    this.OnPropertyChanged(DatabasePathEventArgs);
                    //this.DatabasePathChanged();
                    this.DatabasePathAfterSet();
                }
            }
        }

        partial void DatabasePathBeforeSet(string newValue);
        partial void DatabasePathAfterSet();

        //protected virtual void DatabasePathChanging(string newValue) { }
        //protected virtual void DatabasePathChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabasesEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Databases));
        private List<string> _Databases;
        /// <summary>
        /// 
        /// </summary>
        public List<string> Databases
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

        partial void DatabasesBeforeSet(List<string> newValue);
        partial void DatabasesAfterSet();

        //protected virtual void DatabasesChanging(List<string> newValue) { }
        //protected virtual void DatabasesChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DataCheckedEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DataChecked));
        private bool _DataChecked = false;
        /// <summary>
        /// 
        /// </summary>
        public bool DataChecked
        {
            get
            {
                return this._DataChecked;
            }
            set
            {
                if (!object.Equals(this._DataChecked, value))
                {
                    //this.DataCheckedChanging(value);
                    this.DataCheckedBeforeSet(value);
                    this._DataChecked = value;
                    this.OnPropertyChanged(DataCheckedEventArgs);
                    //this.DataCheckedChanged();
                    this.DataCheckedAfterSet();
                }
            }
        }

        partial void DataCheckedBeforeSet(bool newValue);
        partial void DataCheckedAfterSet();

        //protected virtual void DataCheckedChanging(bool newValue) { }
        //protected virtual void DataCheckedChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DatabaseExistsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DatabaseExists));
        private bool _DatabaseExists = false;
        /// <summary>
        /// 
        /// </summary>
        public bool DatabaseExists
        {
            get
            {
                return this._DatabaseExists;
            }
            set
            {
                if (!object.Equals(this._DatabaseExists, value))
                {
                    //this.DatabaseExistsChanging(value);
                    this.DatabaseExistsBeforeSet(value);
                    this._DatabaseExists = value;
                    this.OnPropertyChanged(DatabaseExistsEventArgs);
                    //this.DatabaseExistsChanged();
                    this.DatabaseExistsAfterSet();
                }
            }
        }

        partial void DatabaseExistsBeforeSet(bool newValue);
        partial void DatabaseExistsAfterSet();

        //protected virtual void DatabaseExistsChanging(bool newValue) { }
        //protected virtual void DatabaseExistsChanged() { }

        //ncrunch: no coverage end
    }
}
