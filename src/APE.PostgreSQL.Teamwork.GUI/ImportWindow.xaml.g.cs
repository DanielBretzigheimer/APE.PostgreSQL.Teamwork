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
	// APE.CodeGeneration.Attribute [CtorParameter(typeof(IProcessManager))]
	// APE.CodeGeneration.Attribute [CtorParameter(AccessModifier.Private, typeof(DatabaseDisplayData), "selectedDatabase")]
	public partial class ImportWindow {        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for CtorParameter
        //--------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        private IProcessManager processManager;

        /// <summary>
        /// 
        /// </summary>
        private DatabaseDisplayData selectedDatabase;

        public ImportWindow(IProcessManager processManager, DatabaseDisplayData selectedDatabase)
        {
            if (processManager == null)
                throw new System.ArgumentNullException("processManager", "processManager == null");
            this.processManager = processManager;

            if (selectedDatabase == null)
                throw new System.ArgumentNullException("selectedDatabase", "selectedDatabase == null");
            this.selectedDatabase = selectedDatabase;

            this.ImportWindowCtor();
        }

        partial void ImportWindowCtor();
        //ncrunch: no coverage end
    }
}
