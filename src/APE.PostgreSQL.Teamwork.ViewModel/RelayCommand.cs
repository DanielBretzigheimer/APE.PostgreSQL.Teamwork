// <copyright file="RelayCommand.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Serilog;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// Implementation of an ICommand that represents a command with a typed CommandParameter.
    /// </summary>
    /// <typeparam name="T">CommandParameter type.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// Initializes a new command that is always executable.
        /// </summary>
        /// <param name="executeAction">Action that is invoked when the command is executed.</param>
        public RelayCommand(Action<T?> executeAction)
            : this() => this.ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));

        /// <summary>
        /// Initializes a new command.
        /// </summary>
        /// <param name="executeAction">Action that is invoked when the command is executed.</param>
        /// <param name="canExecuteFunc">Function that is evaluated when the CanExecute method is executed.</param>
        public RelayCommand(Action<T?> executeAction, Func<T?, bool> canExecuteFunc)
        {
            this.ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            this.CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        /// <summary>
        /// Default constructor only visible to subclasses.
        /// </summary>
        protected RelayCommand()
        {
            this.ExecuteAction = p => { };
            this.CanExecuteFunc = p => true;
        }

        /// <summary>
        /// Event that is raised when the Command's CanExecute-status needs to be reevaluated
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Action that is executed when the command is executed.
        /// </summary>
        protected Action<T?> ExecuteAction { get; set; }

        /// <summary>
        /// Function that is evaluated when the CanExecute method is executed.
        /// </summary>
        protected Func<T?, bool> CanExecuteFunc { get; set; }

        /// <summary>
        /// Implementation of the ICommand interface: Gets if this command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>true if this command can be executed.</returns>
        public virtual bool CanExecute(object? parameter)
        {
            try
            {
                return parameter is T value ? this.CanExecuteFunc(value) : this.CanExecuteFunc(default);
            }
            catch (Exception ex)
            {
                Log.Error("Exception while RelayCommand.CanExecute", ex);
                return false;
            }
        }

        /// <summary>
        /// Implementation of the ICommand interface: Execute this command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public virtual void Execute(object? parameter)
        {
            if (this.CanExecute(parameter))
            {
                var param = parameter is T value ? value : default;

                Exception? exception = null;

                this.OnBeforeExecute(param);

                try
                {
                    this.ExecuteAction(param);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    exception = ex;
                }

                this.OnAfterExecute(param, exception);
            }
        }

        /// <summary>
        /// Raises the CanExecuteChanged-Event. Useful for reevaluation of the commands CanExecute() method.
        /// </summary>
        public void ReEvaluate()
        {
            if (this.CanExecuteChanged != null)
            {
                var currentApplication = System.Windows.Application.Current;
                if (currentApplication != null && currentApplication.Dispatcher != Dispatcher.CurrentDispatcher)
                    currentApplication.Dispatcher.Invoke(() => this.CanExecuteChanged(this, new EventArgs()));
                else
                    this.CanExecuteChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Is called before the action is executed.
        /// </summary>
        /// <param name="parameter">Parameter the action is invoked with.</param>
        protected virtual void OnBeforeExecute(T? parameter)
        {
        }

        /// <summary>
        /// Is called after the action is executed.
        /// </summary>
        /// <param name="parameter">Parameter the action is invoked with.</param>
        /// <param name="ex">Exception if thrown by the action. Null if no exception is thrown.</param>
        protected virtual void OnAfterExecute(T? parameter, Exception? ex)
        {
            if (ex != null)
                Log.Error("Exception while RelayCommand.Execute", ex);
        }
    }

    /// <summary>
    /// Implementation of an ICommand that represents a command with no CommandParameter.
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Initializes a new command that is always executable.
        /// </summary>
        /// <param name="executeAction">Action that is invoked when the command is executed.</param>
        public RelayCommand(Action executeAction)
            : base(x => executeAction())
        {
        }

        /// <summary>
        /// Initializes a new command.
        /// </summary>
        /// <param name="executeAction">Action that is invoked when the command is executed.</param>
        /// <param name="canExecuteFunc">Function that is evaluated when the CanExecute method is executed.</param>
        public RelayCommand(Action executeAction, Func<bool> canExecuteFunc)
            : base(x => executeAction(), x => canExecuteFunc())
        {
        }
    }
}