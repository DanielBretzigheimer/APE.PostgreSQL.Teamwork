using APE.CodeGeneration.Attributes;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(SQLFileDisplayData), "DiffFile")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(SQLFileDisplayData), "UndoDiffFile")]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class ExportWindowViewModel
  : System.ComponentModel.INotifyPropertyChanged
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs DiffFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(DiffFile));
        private SQLFileDisplayData _DiffFile;
        /// <summary>
        /// 
        /// </summary>
        public SQLFileDisplayData DiffFile
        {
            get
            {
                return this._DiffFile;
            }
            set
            {
                if (!object.Equals(this._DiffFile, value))
                {
                    //this.DiffFileChanging(value);
                    this.DiffFileBeforeSet(value);
                    this._DiffFile = value;
                    this.OnPropertyChanged(DiffFileEventArgs);
                    //this.DiffFileChanged();
                    this.DiffFileAfterSet();
                }
            }
        }

        partial void DiffFileBeforeSet(SQLFileDisplayData newValue);
        partial void DiffFileAfterSet();

        //protected virtual void DiffFileChanging(SQLFileDisplayData newValue) { }
        //protected virtual void DiffFileChanged() { }

        protected static readonly System.ComponentModel.PropertyChangedEventArgs UndoDiffFileEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(UndoDiffFile));
        private SQLFileDisplayData _UndoDiffFile;
        /// <summary>
        /// 
        /// </summary>
        public SQLFileDisplayData UndoDiffFile
        {
            get
            {
                return this._UndoDiffFile;
            }
            set
            {
                if (!object.Equals(this._UndoDiffFile, value))
                {
                    //this.UndoDiffFileChanging(value);
                    this.UndoDiffFileBeforeSet(value);
                    this._UndoDiffFile = value;
                    this.OnPropertyChanged(UndoDiffFileEventArgs);
                    //this.UndoDiffFileChanged();
                    this.UndoDiffFileAfterSet();
                }
            }
        }

        partial void UndoDiffFileBeforeSet(SQLFileDisplayData newValue);
        partial void UndoDiffFileAfterSet();

        //protected virtual void UndoDiffFileChanging(SQLFileDisplayData newValue) { }
        //protected virtual void UndoDiffFileChanged() { }

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
