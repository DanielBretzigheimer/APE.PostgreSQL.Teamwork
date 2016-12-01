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
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IMainWindowViewModel))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IConnectionManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IFileSystemAccess))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(IDifferenceCreator))]
    // APE.CodeGeneration.Attribute [CtorParameter(typeof(ISQLFileTester))]
    public partial class MainWindow {        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private IMainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// 
        /// </summary>
        private IConnectionManager connectionManager;

        /// <summary>
        /// 
        /// </summary>
        private IFileSystemAccess fileSystemAccess;

        /// <summary>
        /// 
        /// </summary>
        private IProcessManager processManager;

        /// <summary>
        /// 
        /// </summary>
        private IDifferenceCreator differenceCreator;

        /// <summary>
        /// 
        /// </summary>
        private ISQLFileTester sQLFileTester;

        public MainWindow(IMainWindowViewModel mainWindowViewModel, IConnectionManager connectionManager, IFileSystemAccess fileSystemAccess, IProcessManager processManager, IDifferenceCreator differenceCreator, ISQLFileTester sQLFileTester)
        {
            if (mainWindowViewModel == null)
                throw new System.ArgumentNullException("mainWindowViewModel", "mainWindowViewModel == null");
            this.mainWindowViewModel = mainWindowViewModel;

            if (connectionManager == null)
                throw new System.ArgumentNullException("connectionManager", "connectionManager == null");
            this.connectionManager = connectionManager;

            if (fileSystemAccess == null)
                throw new System.ArgumentNullException("fileSystemAccess", "fileSystemAccess == null");
            this.fileSystemAccess = fileSystemAccess;

            if (processManager == null)
                throw new System.ArgumentNullException("processManager", "processManager == null");
            this.processManager = processManager;

            if (differenceCreator == null)
                throw new System.ArgumentNullException("differenceCreator", "differenceCreator == null");
            this.differenceCreator = differenceCreator;

            if (sQLFileTester == null)
                throw new System.ArgumentNullException("sQLFileTester", "sQLFileTester == null");
            this.sQLFileTester = sQLFileTester;

            this.MainWindowCtor();
        }

        partial void MainWindowCtor();
        //ncrunch: no coverage end
    }
}
