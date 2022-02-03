// <copyright file="ConverterChain.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace APE.PostgreSQL.Teamwork.GUI.Converter
{
    /// <summary>
    /// Represents a chain of <see cref="IValueConverter"/>s to be executed in succession.
    /// </summary>
    [ContentProperty("Converters")]
    [ContentWrapper(typeof(Collection<IValueConverter>))]
    public class ConverterChain : IValueConverter
    {
        private Collection<IValueConverter> converters;

        /// <summary>
        /// Gets the converters to execute.
        /// </summary>
        public Collection<IValueConverter> Converters => this.converters ?? (this.converters = new Collection<IValueConverter>());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var valueConverter in this.Converters)
            {
                value = valueConverter.Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}