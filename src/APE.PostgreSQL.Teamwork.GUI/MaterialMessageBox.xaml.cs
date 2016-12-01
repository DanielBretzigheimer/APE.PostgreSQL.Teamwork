// <copyright file="MaterialMessageBox.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// A <see cref="UserControl"/> which contains a <see cref="MessageBox"/> which is designed with the Material Design Principles.
    /// </summary>
    public partial class MaterialMessageBox : UserControl
    {
        public MaterialMessageBox(string text, string title, MessageBoxButton buttons)
        {
            this.InitializeComponent();

            this.title.Text = title;
            this.message.Text = text;

            switch (buttons)
            {
                case MessageBoxButton.OKCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnOk.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.OK:
                    btnOk.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    btnYes.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Gets the <see cref="TextBlock"/> which shows the message content to the user.
        /// </summary>
        internal TextBlock MessageTextBlock
        {
            get
            {
                return this.message;
            }
        }
    }
}
