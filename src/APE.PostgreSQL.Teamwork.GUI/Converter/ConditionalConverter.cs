// <copyright file="ConditionalConverter.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Globalization;
using System.Windows.Data;

namespace APE.PostgreSQL.Teamwork.GUI.Converter
{
    /// <summary>
    /// Simple WPF 'if'.
    /// </summary>
    public class ConditionalConverter : IValueConverter
    {
        public object Comparant { get; set; }

        public object IfTrue { get; set; }

        public object IfFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return this.Comparant == null
                    ? this.IfTrue
                    : this.IfFalse;
            }

            return value.Equals(this.Comparant)
                ? this.IfTrue
                : this.IfFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}