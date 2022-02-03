// <copyright file="ViewModelRegistry.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using APE.PostgreSQL.Teamwork.ViewModel.ViewModels;
using Lamar;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Holds information for structure map.
    /// </summary>
    public class ViewModelRegistry : ServiceRegistry
    {
        /// <summary>
        /// Creates the <see cref="ViewModelRegistry"/>.
        /// </summary>
        public ViewModelRegistry()
        {
            // Special view models
            this.For<IMainWindowViewModel>().Use<MainWindowViewModel>().Singleton();

            // Manager
            this.For<IConnectionManager>().Use<ConnectionManager>().Singleton();

            // TestHelper classes
            this.For<IFileSystemAccess>().Use<FileSystemAccess>().Singleton();
            this.For<IProcessManager>().Use<ProcessManager>().Singleton();
            this.For<IDifferenceCreator>().Use<DifferenceCreator>().Singleton();
            this.For<ISQLFileTester>().Use<SQLFileTester>().Singleton();
        }
    }
}
