// <auto-generated>
//     This code was generated by the APE CodeGeneration.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel.Exceptions;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(DatabaseVersion), "NewVersion")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "Loading")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "ShowErrorMessage")]
    // APE.CodeGeneration.Attribute [NotifyProperty(typeof(bool), "ShowSuccessMessage")]
    // APE.CodeGeneration.Attribute [NotifyProperty(AccessModifier.PublicGetPrivateSet, typeof(string), "Message", "")]
    // APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Public, typeof(DatabaseDisplayData))]
    // APE.CodeGeneration.Attribute [NotifyPropertySupport]
    public partial class CreateMinorVersionViewModel
  : System.ComponentModel.INotifyPropertyChanged
    {
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        public DatabaseDisplayData DatabaseDisplayData { get; set; }

        public CreateMinorVersionViewModel(DatabaseDisplayData DatabaseDisplayData)
        {
            if (DatabaseDisplayData == null)
                throw new System.ArgumentNullException("DatabaseDisplayData", "DatabaseDisplayData == null");
            this.DatabaseDisplayData = DatabaseDisplayData;

            this.CreateMinorVersionViewModelCtor();
        }

        partial void CreateMinorVersionViewModelCtor();
        //--------------------------------------------------------------------------------
        // generated code for NotifyProperty
        //--------------------------------------------------------------------------------

        protected static readonly System.ComponentModel.PropertyChangedEventArgs NewVersionEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(NewVersion));
        private DatabaseVersion newVersion;

        public DatabaseVersion NewVersion
        {
            get
            {
                return this.newVersion;
            }
            set
            {
                if (!object.Equals(this.newVersion, value))
                {
                    this.NewVersionBeforeSet(value);
                    this.newVersion = value;
                    this.OnPropertyChanged(NewVersionEventArgs);
                    this.NewVersionAfterSet();
                }
            }
        }

        partial void NewVersionBeforeSet(DatabaseVersion newValue);
        partial void NewVersionAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs LoadingEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Loading));
        private bool loading;

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
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowErrorMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowErrorMessage));
        private bool showErrorMessage;

        public bool ShowErrorMessage
        {
            get
            {
                return this.showErrorMessage;
            }
            set
            {
                if (!object.Equals(this.showErrorMessage, value))
                {
                    this.ShowErrorMessageBeforeSet(value);
                    this.showErrorMessage = value;
                    this.OnPropertyChanged(ShowErrorMessageEventArgs);
                    this.ShowErrorMessageAfterSet();
                }
            }
        }

        partial void ShowErrorMessageBeforeSet(bool newValue);
        partial void ShowErrorMessageAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs ShowSuccessMessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowSuccessMessage));
        private bool showSuccessMessage;

        public bool ShowSuccessMessage
        {
            get
            {
                return this.showSuccessMessage;
            }
            set
            {
                if (!object.Equals(this.showSuccessMessage, value))
                {
                    this.ShowSuccessMessageBeforeSet(value);
                    this.showSuccessMessage = value;
                    this.OnPropertyChanged(ShowSuccessMessageEventArgs);
                    this.ShowSuccessMessageAfterSet();
                }
            }
        }

        partial void ShowSuccessMessageBeforeSet(bool newValue);
        partial void ShowSuccessMessageAfterSet();
        

        protected static readonly System.ComponentModel.PropertyChangedEventArgs MessageEventArgs = new System.ComponentModel.PropertyChangedEventArgs(nameof(Message));
        private string message = "";

        public string Message
        {
            get
            {
                return this.message;
            }
            private set
            {
                if (!object.Equals(this.message, value))
                {
                    this.MessageBeforeSet(value);
                    this.message = value;
                    this.OnPropertyChanged(MessageEventArgs);
                    this.MessageAfterSet();
                }
            }
        }

        partial void MessageBeforeSet(string newValue);
        partial void MessageAfterSet();
        

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
