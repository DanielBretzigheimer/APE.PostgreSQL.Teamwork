// <copyright file="ErrorStatusToColorConverter.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Windows.Data;
using System.Windows.Media;
using APE.PostgreSQL.Teamwork.Model;

namespace APE.PostgreSQL.Teamwork.GUI.Converter
{
    public class ErrorStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ErrorStatus status)
            {
                switch (status)
                {
                    case ErrorStatus.Error:
                        return Brushes.Red;
                    case ErrorStatus.Successful:
                        return Brushes.Green;
                    case ErrorStatus.Unknown:
                        return Brushes.Black;
                }
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
    }
}
