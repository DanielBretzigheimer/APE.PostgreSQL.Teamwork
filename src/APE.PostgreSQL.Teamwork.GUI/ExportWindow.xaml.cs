// <copyright file="ExportWindow.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
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
