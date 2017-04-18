// <copyright file="Statement.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
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

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// Displays one <see cref="ViewModel.StatementDisplayData"/>.
    /// </summary>
    public partial class Statement : UserControl
    {
        public Statement()
        {
            this.InitializeComponent();
        }
    }
}
