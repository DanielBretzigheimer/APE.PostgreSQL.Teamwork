// <copyright file="SettingView.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using APE.PostgreSQL.Teamwork.ViewModel;
using MaterialDesignThemes.Wpf;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// A <see cref="UserControl"/> which contains all settings.
    /// </summary>
    public partial class SettingView : UserControl
    {
        private SettingViewModel viewModel = null;

        public SettingView(IConnectionManager connectionManager)
        {
            try
            {
                this.viewModel = new SettingViewModel(connectionManager);
                this.DataContext = this.viewModel;
                this.InitializeComponent();
                this.passwordBox.Password = this.viewModel.Password;
            }
            catch (Exception e)
            {
                var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                n.ShowDialog();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            if (DialogHost.CloseDialogCommand.CanExecute(true, this))
            {
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
        }

        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.Password = this.passwordBox.Password;
        }
    }
}
