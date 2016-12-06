using System;
using System.IO;
using APE.CodeGeneration.Attributes;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "SQL")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "Title")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class Statement  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs SQLEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(SQL));
        private string _SQL;
        /// <summary>
        /// 
        /// </summary>
        public string SQL
        {
            get
            {
                return this._SQL;
            }
            private set
            {
                if (!object.Equals(this._SQL, value))
                {
                    //this.SQLChanging(value);
                    this.SQLBeforeSet(value);
                    this._SQL = value;
                    this.OnPropertyChanged(SQLEventArgs);
                    //this.SQLChanged();
                    this.SQLAfterSet();
                }
            }
        }

        partial void SQLBeforeSet(string newValue);
        partial void SQLAfterSet();

        //protected virtual void SQLChanging(string newValue) { }
        //protected virtual void SQLChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs TitleEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Title));
        private string _Title;
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get
            {
                return this._Title;
            }
            private set
            {
                if (!object.Equals(this._Title, value))
                {
                    //this.TitleChanging(value);
                    this.TitleBeforeSet(value);
                    this._Title = value;
                    this.OnPropertyChanged(TitleEventArgs);
                    //this.TitleChanged();
                    this.TitleAfterSet();
                }
            }
        }

        partial void TitleBeforeSet(string newValue);
        partial void TitleAfterSet();

        //protected virtual void TitleChanging(string newValue) { }
        //protected virtual void TitleChanged() { }

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
