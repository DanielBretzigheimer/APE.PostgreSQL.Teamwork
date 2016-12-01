using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using APE.CodeGeneration.Attributes;

namespace APE.PostgreSQL.Teamwork.GUI
{
    // APE.CodeGeneration.Attribute [DependencyProperty(typeof(object), "DataContext", null)]
    // APE.CodeGeneration.Attribute [DependencyProperty(typeof(ICommand), "Command", null)]
    public partial class InvokeCommandWithEventArgs {        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for DependencyProperty
        //--------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public object DataContext
        {
            get { return (object)this.GetValue(DataContextProperty); }
            set { this.SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(InvokeCommandWithEventArgs), new PropertyMetadata(null, DataContextChangedCallback));

        static void DataContextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataContextChanged(d, e);
        }

        static partial void DataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandWithEventArgs), new PropertyMetadata(null, CommandChangedCallback));

        static void CommandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandChanged(d, e);
        }

        static partial void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

        //ncrunch: no coverage end
    }
}
