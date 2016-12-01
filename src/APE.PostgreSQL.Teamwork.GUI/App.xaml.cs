// <copyright file="App.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using APE.PostgreSQL.Teamwork.ViewModel;
using log4net;
using StructureMap;

namespace APE.PostgreSQL.Teamwork.GUI
{
	public partial class App : Application
	{
		private Container structureMapContainer;

		/// <summary>
		/// Register Unhandled Exceptions.
		/// </summary>
		protected override async void OnStartup(StartupEventArgs e)
		{
			var log4NetConfigFile = "log4net.config";
			if (!System.IO.File.Exists(log4NetConfigFile))
				throw new Exception(log4NetConfigFile + " does not exist.");
			log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFile));

			var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			logger.Info("Log4net is configured by using file: " + log4NetConfigFile);

			// Register handlers for unhandled exceptions
			AppDomain.CurrentDomain.UnhandledException += this.CurrentDomainUnhandledException;
			System.Windows.Application.Current.DispatcherUnhandledException += this.CurrentDispatcherUnhandledException;

			await Task.Run(() => this.structureMapContainer = new Container(new GUIRegistry())).ConfigureAwait(true);

			// Show main window
			var mainWindow = this.structureMapContainer.GetInstance<MainWindow>();
			System.Windows.Application.Current.MainWindow = mainWindow;
			mainWindow.Show();
		}

		private void CurrentDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			var n = new Greg.WPF.Utility.ExceptionMessageBox(e.Exception, "CurrentDispatcherUnhandledException");
			n.ShowDialog();
		}

		private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var n = new Greg.WPF.Utility.ExceptionMessageBox((Exception)e.ExceptionObject, "CurrentDomainUnhandledException");
			n.ShowDialog();
		}
	}
}
