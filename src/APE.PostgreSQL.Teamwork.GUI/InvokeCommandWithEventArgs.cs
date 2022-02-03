// <copyright file="InvokeCommandWithEventArgs.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace APE.PostgreSQL.Teamwork.GUI
{
    public partial class InvokeCommandWithEventArgs : TriggerAction<DependencyObject>
    {
        private string commandName;

        /// <summary>
        /// Gets or sets the name of the command which should be executed.
        /// </summary>
        /// <remarks>
        /// This property is replaced by the "Command" property if both are set.
        /// </remarks>
        public string CommandName
        {
            get => this.commandName;

            set
            {
                if (this.CommandName != value)
                    this.commandName = value;
            }
        }

        /// <summary>
        /// Calls the action.
        /// </summary>
        /// <param name="parameter">The parameter which is forwarded to the command.</param>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            var command = this.ResolveCommand();
            if (command == null || !command.CanExecute(parameter))
            {
                return;
            }

            command.Execute(parameter);
        }

        private ICommand? ResolveCommand()
        {
            ICommand? command = null;
            if (this.Command != null)
            {
                command = this.Command;
            }
            else if (this.DataContext != null)
            {
                // search command in given datacontext
                foreach (var propertyInfo in this.DataContext.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, this.CommandName, StringComparison.Ordinal))
                        command = (ICommand?)propertyInfo.GetValue(this.DataContext, null);
                }
            }
            else if (this.AssociatedObject != null)
            {
                // serach command in associated object
                foreach (var propertyInfo in this.AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, this.CommandName, StringComparison.Ordinal))
                        command = (ICommand?)propertyInfo.GetValue(this.AssociatedObject, null);
                }
            }

            return command;
        }
    }
}