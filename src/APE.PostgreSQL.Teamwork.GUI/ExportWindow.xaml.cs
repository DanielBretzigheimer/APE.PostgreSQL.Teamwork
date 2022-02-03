// <copyright file="ExportWindow.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Windows;
using APE.PostgreSQL.Teamwork.ViewModel;

namespace APE.PostgreSQL.Teamwork.GUI
{
    public partial class ExportWindow : Window
    {
        public ExportWindow(SQLFileDisplayData diff, SQLFileDisplayData undoDiff)
        {
            this.DataContext = new ExportWindowViewModel(diff, undoDiff);
            this.InitializeComponent();
        }
    }
}
