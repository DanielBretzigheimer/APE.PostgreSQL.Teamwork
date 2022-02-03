// <copyright file="ImportWindow.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using APE.PostgreSQL.Teamwork.ViewModel;
using Serilog;

namespace APE.PostgreSQL.Teamwork.GUI
{
    public partial class ImportWindow : DialogWindow
    {
        public override object DialogIdentifier
        {
            get
            {
                if (this.dialogHost == null)
                    throw new InvalidOperationException("Can not show a dialog before the dialog host was initialized");

                return this.dialogHost.Identifier ?? new object();
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
                Log.Error(e, "Unhandled Exception");
            }
        }
    }
}
