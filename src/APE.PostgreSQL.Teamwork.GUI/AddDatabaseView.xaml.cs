// <copyright file="AddDatabaseView.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows.Controls;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.ViewModel;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using MaterialDesignThemes.Wpf;

namespace APE.PostgreSQL.Teamwork
{
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(typeof(ISQLFileTester))]
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
                var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                n.ShowDialog();
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
