using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace notes5;

public class StringTrimmerConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            if (str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Length > 22)
            {
                return $"{str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Substring(0, 22)}..." ;
            }
            else
            {
                return str.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }
        return value;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}