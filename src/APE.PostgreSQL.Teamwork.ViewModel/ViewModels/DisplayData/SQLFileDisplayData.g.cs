// <auto-generated>
//     This code was generated by the APE CodeGeneration.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;

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
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SQLStatementsEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SQLStatements));
        private ObservableCollection<StatementDisplayData> sQLStatements;

        public ObservableCollection<StatementDisplayData> SQLStatements
        {
            get
            {
                return this.sQLStatements;
            }
            set
            {
                if (!object.Equals(this.sQLStatements, value))
                {
                    this.SQLStatementsBeforeSet(value);
                    this.sQLStatements = value;
                    this.OnPropertyChanged(SQLStatementsEventArgs);
                    this.SQLStatementsAfterSet();
                }
            }
        }

        partial void SQLStatementsBeforeSet(ObservableCollection<StatementDisplayData> newValue);
        partial void SQLStatementsAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SQLFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SQLFile));
        private SQLFile sQLFile;

        public SQLFile SQLFile
        {
            get
            {
                return this.sQLFile;
            }
            set
            {
                if (!object.Equals(this.sQLFile, value))
                {
                    this.SQLFileBeforeSet(value);
                    this.sQLFile = value;
                    this.OnPropertyChanged(SQLFileEventArgs);
                    this.SQLFileAfterSet();
                }
            }
        }

        partial void SQLFileBeforeSet(SQLFile newValue);
        partial void SQLFileAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs StatusEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Status));
        private ErrorStatus status;

        public ErrorStatus Status
        {
            get
            {
                return this.status;
            }
            private set
            {
                if (!object.Equals(this.status, value))
                {
                    this.StatusBeforeSet(value);
                    this.status = value;
                    this.OnPropertyChanged(StatusEventArgs);
                    this.StatusAfterSet();
                }
            }
        }

        partial void StatusBeforeSet(ErrorStatus newValue);
        partial void StatusAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowWarningEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowWarning));
        private bool showWarning;

        public bool ShowWarning
        {
            get
            {
                return this.showWarning;
            }
            set
            {
                if (!object.Equals(this.showWarning, value))
                {
                    this.ShowWarningBeforeSet(value);
                    this.showWarning = value;
                    this.OnPropertyChanged(ShowWarningEventArgs);
                    this.ShowWarningAfterSet();
                }
            }
        }

        partial void ShowWarningBeforeSet(bool newValue);
        partial void ShowWarningAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs WarningTextEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(WarningText));
        private string warningText;

        public string WarningText
        {
            get
            {
                return this.warningText;
            }
            set
            {
                if (!object.Equals(this.warningText, value))
                {
                    this.WarningTextBeforeSet(value);
                    this.warningText = value;
                    this.OnPropertyChanged(WarningTextEventArgs);
                    this.WarningTextAfterSet();
                }
            }
        }

        partial void WarningTextBeforeSet(string newValue);
        partial void WarningTextAfterSet();
        

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
