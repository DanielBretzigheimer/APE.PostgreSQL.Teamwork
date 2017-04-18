// <copyright file="ImportWindow.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
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
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.ViewModel;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using MaterialDesignThemes.Wpf;

namespace APE.PostgreSQL.Teamwork.GUI
{
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(AccessModifier.Private, typeof(DatabaseDisplayData), "selectedDatabase")]
    public partial class ImportWindow : DialogWindow
    {
        public override object DialogIdentifier
        {
            get
            {
                if (this.dialogHost == null)
                {
                    throw new InvalidOperationException("Can not show a dialog before the dialog host was initialized");
                }

                return this.dialogHost.Identifier;
            }
        }

        partial void ImportWindowCtor()
        {
            try
            {
                this.DataContext = new ImportWindowViewModel(this.processManager, this.selectedDatabase);
                this.InitializeComponent();
            }
            catch (Exception e)
            {
                var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                n.ShowDialog();
            }
        }
    }
}
