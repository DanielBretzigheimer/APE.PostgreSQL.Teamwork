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
        public MaterialMessageBox(string text, string title, MessageBoxButton buttons, bool isMarkdown)
        {
            this.Markdown = text;
            this.InitializeComponent();

            this.title.Text = title;

            if (isMarkdown)
            {
                this.flowDocument.Visibility = Visibility.Visible;
                this.message.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.message.Text = text;
            }

            switch (buttons)
            {
                case MessageBoxButton.OKCancel:
                    this.btnCancel.Visibility = Visibility.Visible;
                    this.btnOk.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.OK:
                    this.btnOk.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    this.btnYes.Visibility = Visibility.Visible;
                    this.btnCancel.Visibility = Visibility.Visible;
                    this.btnNo.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    this.btnYes.Visibility = Visibility.Visible;
                    this.btnNo.Visibility = Visibility.Visible;
                    break;
            }
        }

        public string Markdown { get; private set; }

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

        private void CopyMessageClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(this.message.Text);
            }
            catch (Exception)
            {
            }
        }
    }
}
