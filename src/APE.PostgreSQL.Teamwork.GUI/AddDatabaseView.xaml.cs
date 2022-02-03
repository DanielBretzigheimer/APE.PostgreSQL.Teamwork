// <copyright file="AddDatabaseView.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows.Controls;
using APE.PostgreSQL.Teamwork.ViewModel;
using MaterialDesignThemes.Wpf;
using Serilog;

namespace APE.PostgreSQL.Teamwork
{
    public partial class AddDatabaseView : UserControl
    {
        partial void AddDatabaseViewCtor()
        {
            try
            {
                this.DataContext = new AddDatabaseViewModel(this.connectionManager, this.fileSystemAccess, this.processManager, this.differenceCreator, this.sQLFileTester, this.Close);
                this.InitializeComponent();
            }
            catch (Exception e)
            {
                Log.Error(e, "Unhandled Exception");
            }
        }

        private void Close()
        {
            if (DialogHost.CloseDialogCommand.CanExecute(true, this))
            {
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
        }
    }
}
