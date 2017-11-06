// <copyright file="CreateMinorVersionView.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

using System.Windows.Controls;
using APE.PostgreSQL.Teamwork.ViewModel;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// Interaction logic for CreateMinorVersionView.xaml
    /// </summary>
    public partial class CreateMinorVersionView : UserControl
    {
        public CreateMinorVersionView(DatabaseDisplayData database)
        {
            this.DataContext = new CreateMinorVersionViewModel(database);
            this.InitializeComponent();
        }
    }
}
