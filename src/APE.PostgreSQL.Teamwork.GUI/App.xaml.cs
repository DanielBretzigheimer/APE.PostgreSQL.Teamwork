// <copyright file="App.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Lamar;
using Serilog;

namespace APE.PostgreSQL.Teamwork.GUI
{
    public partial class App : Application
    {
        /// <summary>
        /// Register Unhandled Exceptions.
        /// </summary>
        protected override async void OnStartup(StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/APE.PostgreSQL.Teamwork.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application starting");

            // Register handlers for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomainUnhandledException;
            Current.DispatcherUnhandledException += this.CurrentDispatcherUnhandledException;

            Container? container = null;
            await Task.Run(() => container = new Container(new GUIRegistry())).ConfigureAwait(true);

            if (container == null)
                throw new InvalidOperationException("Container could not be created.");

            // Show main window
            var mainWindow = container.GetInstance<MainWindow>();
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void CurrentDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) => Log.Error(e.Exception, "CurrentDispatcherUnhandledException");

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                Log.Error(ex, "CurrentDomainUnhandledException");
            else
                Log.Error("CurrentDomainUnhandledException");
        }
    }
}
