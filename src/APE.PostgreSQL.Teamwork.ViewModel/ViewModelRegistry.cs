// <copyright file="ViewModelRegistry.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.PostgreSQL.Teamwork.ViewModel.Postgres;
using APE.PostgreSQL.Teamwork.ViewModel.TestHelper;
using APE.PostgreSQL.Teamwork.ViewModel.ViewModels;
using StructureMap;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Holds information for structure map.
    /// </summary>
    public class ViewModelRegistry : Registry
    {
        /// <summary>
        /// Creates the <see cref="ViewModelRegistry"/>.
        /// </summary>
        public ViewModelRegistry()
        {
            // Special view models
            this.For<IMainWindowViewModel>().Singleton().Use<MainWindowViewModel>();

            // Manager
            this.For<IConnectionManager>().Singleton().Use<ConnectionManager>();

            // TestHelper classes
            this.For<IFileSystemAccess>().Singleton().Use<FileSystemAccess>();
            this.For<IProcessManager>().Singleton().Use<ProcessManager>();
            this.For<IDifferenceCreator>().Singleton().Use<DifferenceCreator>();
            this.For<ISQLFileTester>().Singleton().Use<SQLFileTester>();
        }
    }
}
