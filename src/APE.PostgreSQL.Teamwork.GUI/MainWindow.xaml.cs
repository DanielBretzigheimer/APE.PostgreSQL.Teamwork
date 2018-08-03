// <copyright file="MainWindow.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.ViewModel;
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using APE.PostgreSQL.Teamwork.ViewModel.ViewModels;
using Dragablz;

namespace APE.PostgreSQL.Teamwork.GUI
{
    [CtorParameter(typeof(IMainWindowViewModel))]
    [CtorParameter(typeof(IConnectionManager))]
    [CtorParameter(typeof(IFileSystemAccess))]
    [CtorParameter(typeof(IProcessManager))]
    [CtorParameter(typeof(IDifferenceCreator))]
    [CtorParameter(typeof(ISQLFileTester))]
    public partial class MainWindow : DialogWindow
    {
        private List<DatabaseDisplayData> lastOrder = new List<DatabaseDisplayData>();

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

        partial void MainWindowCtor()
        {
            try
            {
                this.InitializeComponent();

                BaseViewModel.GetAddDatabseView = this.GetAddDatabaseView;
                BaseViewModel.GetSettingView = this.GetSettingView;
                BaseViewModel.GetCreateMinorVersionView = this.GetCreateMinorVersionView;
                BaseViewModel.GetMessageBox = (text, title, buttons) => this.GetMessageBox(text, title, buttons, false);
                BaseViewModel.GetMarkdownBox = (text, title, buttons) => this.GetMessageBox(text, title, buttons, true);
                BaseViewModel.OpenImportWindow = this.OpenImportWindow;
                BaseViewModel.OpenExportWindow = this.OpenExportWindow;
                this.DataContext = this.mainWindowViewModel;

                var monitor = new VerticalPositionMonitor();
                this.DatabaseList.PositionMonitor = monitor;
                this.DatabaseList.PreviewMouseUp += this.DatabaseListPreviewMouseUp;
                monitor.OrderChanged += this.DatabaseOrderChanged;
                this.dialogHost.Loaded += this.DialogHostLoaded;
            }
            catch (Exception e)
            {
                var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                n.ShowDialog();
            }
        }

        private void DatabaseListPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.lastOrder != null)
            {
                this.mainWindowViewModel.ReorderDatabases(this.lastOrder);
                this.lastOrder = null;
            }
        }

        private void DatabaseOrderChanged(object sender, OrderChangedEventArgs e)
        {
            // first start
            if (e.NewOrder == null
                || e.PreviousOrder == null
                || e.NewOrder.Count() != e.PreviousOrder.Count())
            {
                return;
            }

            var oldDatabases = e.PreviousOrder.Cast<DatabaseDisplayData>().ToList();
            var newDatabases = e.NewOrder.Cast<DatabaseDisplayData>().ToList();

            var oldDatabaseNames = oldDatabases.Select(d => d.Name).ToList();
            var newDatabaseNames = newDatabases.Select(d => d.Name).ToList();
            var orderEquals = true;
            for (var i = 0; i < oldDatabaseNames.Count(); i++)
            {
                if (oldDatabaseNames[i] != newDatabaseNames[i])
                {
                    orderEquals = false;
                }
            }

            // no change in order
            if (orderEquals)
            {
                return;
            }

            this.lastOrder = newDatabases;
        }

        private void DialogHostLoaded(object sender, RoutedEventArgs e)
        {
            // start the viewmodel when the dialog host is initialized
            // to ensure that dialogs can be shown
            if (!this.mainWindowViewModel.IsStarted)
                this.mainWindowViewModel.Start();
        }

        private CreateMinorVersionView GetCreateMinorVersionView(DatabaseDisplayData database)
        {
            CreateMinorVersionView retval = null;

            this.UIDispatcher.Invoke(() => retval = new CreateMinorVersionView(database));

            return retval;
        }

        private SettingView GetSettingView()
        {
            SettingView retVal = null;

            // create in ui dispatcher so this method can also be called from an task
            this.UIDispatcher.Invoke(() => retVal = new SettingView(this.connectionManager));

            return retVal;
        }

        private AddDatabaseView GetAddDatabaseView()
        {
            AddDatabaseView retVal = null;

            // create in ui dispatcher so this method can also be called from an task
            this.UIDispatcher.Invoke(() => retVal = new AddDatabaseView(this.connectionManager, this.fileSystemAccess, this.processManager, this.differenceCreator, this.sQLFileTester));

            return retVal;
        }

        private MaterialMessageBox GetMessageBox(string text, string title, MessageBoxButton buttons, bool isMarkdown)
        {
            MaterialMessageBox retVal = null;

            // create in ui dispatcher so this method can also be called from an task
            this.UIDispatcher.Invoke(() => retVal = new MaterialMessageBox(text, title, buttons, isMarkdown));

            return retVal;
        }

        private void OpenImportWindow(DatabaseDisplayData database)
        {
            try
            {
                this.UIDispatcher.Invoke(() => new ImportWindow(this.processManager, database).ShowDialog());
                this.mainWindowViewModel.UpdateDatabases();
            }
            catch (Exception e)
            {
                this.UIDispatcher.Invoke(() =>
                {
                    var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                    n.ShowDialog();
                });
            }
        }

        private void OpenExportWindow(SQLFileDisplayData diff, SQLFileDisplayData undoDiff)
        {
            try
            {
                this.UIDispatcher.Invoke(() => new ExportWindow(diff, undoDiff).ShowDialog());
            }
            catch (Exception e)
            {
                this.UIDispatcher.Invoke(() =>
                {
                    var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                    n.ShowDialog();
                });
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            this.searchBar.Focus();
        }

        private async void ShowLicensesClick(object sender, RoutedEventArgs e)
        {
            var licenseText = new StringBuilder();

            var apgDiffLink = new Hyperlink() { NavigateUri = new Uri("http://www.apgdiff.com/index.php") };
            apgDiffLink.Inlines.Add("http://www.apgdiff.com/index.php");
            apgDiffLink.RequestNavigate += this.RequestNavigate;
            var wpfMaterialDesignLink = new Hyperlink() { NavigateUri = new Uri("http://materialdesigninxaml.net/") };
            wpfMaterialDesignLink.Inlines.Add("http://materialdesigninxaml.net/");
            wpfMaterialDesignLink.RequestNavigate += this.RequestNavigate;

            // message is filled with inlines to add links
            var messageBox = new MaterialMessageBox(string.Empty, "Licenses", MessageBoxButton.OK, false);
            messageBox.MessageTextBlock.Inlines.Clear();
            messageBox.MessageTextBlock.Inlines.Add("Postgres Diff Tool" + Environment.NewLine);
            messageBox.MessageTextBlock.Inlines.Add("Was used as base for this application." + Environment.NewLine);
            messageBox.MessageTextBlock.Inlines.Add(apgDiffLink);
            messageBox.MessageTextBlock.Inlines.Add(Environment.NewLine);
            messageBox.MessageTextBlock.Inlines.Add("Material Design for WPF." + Environment.NewLine);
            messageBox.MessageTextBlock.Inlines.Add(wpfMaterialDesignLink);
            await this.ShowDialog(messageBox);
        }

        private void RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void DialogWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.F)
            {
                this.mainWindowViewModel.ShowSearch = true;
                this.searchBar.Focus();
            }
        }
    }
}
