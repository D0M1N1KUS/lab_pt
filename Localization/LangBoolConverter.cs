using System;
using System.Globalization;
using System.Windows.Data;

namespace Lab3.Localization
{
    public class LangBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string) value == (string) parameter;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool) value)
                return (string) parameter;
            return null;
        }
    }
}