// <copyright file="BaseViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using APE.CodeGeneration.Attributes;
using APE.PostgreSQL.Teamwork.Model;
using log4net;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Base partial class for all ViewModels which contains a logger and
    /// the possibility to execute a action in an task.
    /// </summary>
    [NotifyPropertySupport]
    public partial class BaseViewModel
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets a function which shows a dialog of the given object (should be a user control).
        /// </summary>
        public static Func<object, Task<MaterialMessageBoxResult>> ShowDialog { get; set; }

        /// <summary>
        /// Gets a function which shows a dialog of the given object (should be a user control)
        /// and uses a action to notify when the dialog is closed.
        /// </summary>
        public static Func<object, Action<MaterialMessageBoxResult>, Task<MaterialMessageBoxResult>> ShowDialogWithCloseHandler { get; set; }

        /// <summary>
        /// Gets a function which shows a dialog of the given object (should be a user control)
        /// and uses a function to notify when the dialog is closed. The function can return a new user control which
        /// is displayed afterwards or null if the dialog should be closed.
        /// </summary>
        public static Func<object, Func<MaterialMessageBoxResult, object>, Task<MaterialMessageBoxResult>> ShowExtendedDialog { get; set; }

        // Get basic views
        public static Func<object> GetAddDatabseView { get; set; }

        public static Func<object> GetSettingView { get; set; }

        /// <summary>
        /// Gets or sets a function which can create a MaterialMessageBox with the given parameters (text, title, buttons).
        /// </summary>
        public static Func<string, string, MessageBoxButton, object> GetMessageBox { get; set; }

        public static Action<DatabaseDisplayData> OpenImportWindow { get; set; }

        public static Action<SQLFileDisplayData, SQLFileDisplayData> OpenExportWindow { get; set; }

        /// <summary>
        /// Gets or sets a command which can close the view which belongs to
        /// this <see cref="BaseViewModel"/>
        /// </summary>
        public static Action CloseView { get; set; }

        /// <summary>
        /// Executes the given action in a new task
        /// and disables the control while executing.
        /// </summary>
        protected void ExecuteInTask(Action action, Action<bool> setExecuting = null)
        {
            new Task(() =>
            {
                try
                {
                    setExecuting?.Invoke(true);
                    action();
                    setExecuting?.Invoke(false);
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    Log.Error("Exception while executing Action in Task", ex);
                }
            }).Start();
        }
    }
}
