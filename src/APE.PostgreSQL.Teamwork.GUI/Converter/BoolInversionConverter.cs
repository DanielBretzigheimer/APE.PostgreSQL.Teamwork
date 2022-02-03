// <copyright file="BoolInversionConverter.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows.Data;

namespace APE.PostgreSQL.Teamwork.GUI.Converter
{
    /// <summary>
    /// Converter which inverts the given boolean.
    /// </summary>
    public class BoolInversionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not bool)
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotSupportedException();
    }
}
