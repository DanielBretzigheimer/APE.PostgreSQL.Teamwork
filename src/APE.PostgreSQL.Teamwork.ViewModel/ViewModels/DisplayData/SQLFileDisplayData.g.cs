using System.Collections.ObjectModel;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(ObservableCollection<StatementDisplayData>), "SQLStatements")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(SQLFile), "SQLFile")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(ErrorStatus), "Status")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "ShowWarning")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(string), "WarningText")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class SQLFileDisplayData
  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SQLStatementsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SQLStatements));
        private ObservableCollection<StatementDisplayData> _SQLStatements;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<StatementDisplayData> SQLStatements
        {
            get
            {
                return this._SQLStatements;
            }
            set
            {
                if (!object.Equals(this._SQLStatements, value))
                {
                    //this.SQLStatementsChanging(value);
                    this.SQLStatementsBeforeSet(value);
                    this._SQLStatements = value;
                    this.OnPropertyChanged(SQLStatementsEventArgs);
                    //this.SQLStatementsChanged();
                    this.SQLStatementsAfterSet();
                }
            }
        }

        partial void SQLStatementsBeforeSet(ObservableCollection<StatementDisplayData> newValue);
        partial void SQLStatementsAfterSet();

        //protected virtual void SQLStatementsChanging(ObservableCollection<StatementDisplayData> newValue) { }
        //protected virtual void SQLStatementsChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SQLFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SQLFile));
        private SQLFile _SQLFile;
        /// <summary>
        /// 
        /// </summary>
        public SQLFile SQLFile
        {
            get
            {
                return this._SQLFile;
            }
            set
            {
                if (!object.Equals(this._SQLFile, value))
                {
                    //this.SQLFileChanging(value);
                    this.SQLFileBeforeSet(value);
                    this._SQLFile = value;
                    this.OnPropertyChanged(SQLFileEventArgs);
                    //this.SQLFileChanged();
                    this.SQLFileAfterSet();
                }
            }
        }

        partial void SQLFileBeforeSet(SQLFile newValue);
        partial void SQLFileAfterSet();

        //protected virtual void SQLFileChanging(SQLFile newValue) { }
        //protected virtual void SQLFileChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs StatusEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Status));
        private ErrorStatus _Status;
        /// <summary>
        /// 
        /// </summary>
        public ErrorStatus Status
        {
            get
            {
                return this._Status;
            }
            private set
            {
                if (!object.Equals(this._Status, value))
                {
                    //this.StatusChanging(value);
                    this.StatusBeforeSet(value);
                    this._Status = value;
                    this.OnPropertyChanged(StatusEventArgs);
                    //this.StatusChanged();
                    this.StatusAfterSet();
                }
            }
        }

        partial void StatusBeforeSet(ErrorStatus newValue);
        partial void StatusAfterSet();

        //protected virtual void StatusChanging(ErrorStatus newValue) { }
        //protected virtual void StatusChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowWarningEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowWarning));
        private bool _ShowWarning;
        /// <summary>
        /// 
        /// </summary>
        public bool ShowWarning
        {
            get
            {
                return this._ShowWarning;
            }
            set
            {
                if (!object.Equals(this._ShowWarning, value))
                {
                    //this.ShowWarningChanging(value);
                    this.ShowWarningBeforeSet(value);
                    this._ShowWarning = value;
                    this.OnPropertyChanged(ShowWarningEventArgs);
                    //this.ShowWarningChanged();
                    this.ShowWarningAfterSet();
                }
            }
        }

        partial void ShowWarningBeforeSet(bool newValue);
        partial void ShowWarningAfterSet();

        //protected virtual void ShowWarningChanging(bool newValue) { }
        //protected virtual void ShowWarningChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs WarningTextEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(WarningText));
        private string _WarningText;
        /// <summary>
        /// 
        /// </summary>
        public string WarningText
        {
            get
            {
                return this._WarningText;
            }
            set
            {
                if (!object.Equals(this._WarningText, value))
                {
                    //this.WarningTextChanging(value);
                    this.WarningTextBeforeSet(value);
                    this._WarningText = value;
                    this.OnPropertyChanged(WarningTextEventArgs);
                    //this.WarningTextChanged();
                    this.WarningTextAfterSet();
                }
            }
        }

        partial void WarningTextBeforeSet(string newValue);
        partial void WarningTextAfterSet();

        //protected virtual void WarningTextChanging(string newValue) { }
        //protected virtual void WarningTextChanged() { }

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
