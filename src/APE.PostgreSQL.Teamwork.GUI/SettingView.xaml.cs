// <copyright file="SettingView.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows;
using System.Windows.Controls;
using APE.PostgreSQL.Teamwork.ViewModel;
using MaterialDesignThemes.Wpf;
using Serilog;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// A <see cref="UserControl"/> which contains all settings.
    /// </summary>
    public partial class SettingView : UserControl
    {
        private readonly SettingViewModel viewModel;

        public SettingView(IConnectionManager connectionManager)
        {
            this.viewModel = new SettingViewModel(connectionManager);
            this.DataContext = this.viewModel;
            this.InitializeComponent();
            this.passwordBox.Password = this.viewModel.Password;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            if (DialogHost.CloseDialogCommand.CanExecute(true, this))
            {
                DialogHost.CloseDialogCommand.Execute(true, this);
            }
        }

        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e) => this.viewModel.Password = this.passwordBox.Password;
    }
}
