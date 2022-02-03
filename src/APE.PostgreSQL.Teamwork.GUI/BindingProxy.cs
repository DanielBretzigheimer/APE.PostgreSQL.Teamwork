// <copyright file="BindingProxy.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System.Windows;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// This class provides access from framework elements that are not within the visual tree (i.e. in ItemTemplates of ListViews).
    /// </summary>
    /// <remarks>Sample taken from <c>http://www.thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/</c>. </remarks>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// Data-DependencyProperty as a bind able backing store for the proxy object.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        /// The proxy object.
        /// </summary>
        public object Data
        {
            get => (object)this.GetValue(DataProperty);
            set => this.SetValue(DataProperty, value);
        }

        /// <summary>
        /// See <see cref="Freezable"/>.
        /// </summary>
        protected override Freezable CreateInstanceCore() => new BindingProxy();
    }
}