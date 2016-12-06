using System.Text;
using System.Windows;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using Npgsql;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(IStatement), "Statement")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "EditMode")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class StatementDisplayData
  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs StatementEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Statement));
        private IStatement _Statement;
        /// <summary>
        /// 
        /// </summary>
        public IStatement Statement
        {
            get
            {
                return this._Statement;
            }
            set
            {
                if (!object.Equals(this._Statement, value))
                {
                    //this.StatementChanging(value);
                    this.StatementBeforeSet(value);
                    this._Statement = value;
                    this.OnPropertyChanged(StatementEventArgs);
                    //this.StatementChanged();
                    this.StatementAfterSet();
                }
            }
        }

        partial void StatementBeforeSet(IStatement newValue);
        partial void StatementAfterSet();

        //protected virtual void StatementChanging(IStatement newValue) { }
        //protected virtual void StatementChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs EditModeEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(EditMode));
        private bool _EditMode;
        /// <summary>
        /// 
        /// </summary>
        public bool EditMode
        {
            get
            {
                return this._EditMode;
            }
            set
            {
                if (!object.Equals(this._EditMode, value))
                {
                    //this.EditModeChanging(value);
                    this.EditModeBeforeSet(value);
                    this._EditMode = value;
                    this.OnPropertyChanged(EditModeEventArgs);
                    //this.EditModeChanged();
                    this.EditModeAfterSet();
                }
            }
        }

        partial void EditModeBeforeSet(bool newValue);
        partial void EditModeAfterSet();

        //protected virtual void EditModeChanging(bool newValue) { }
        //protected virtual void EditModeChanged() { }

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
