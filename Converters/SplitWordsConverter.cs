using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ToDoAdvanced.Converters;

public class SplitWordsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var text = value.ToString();
        return System.Text.RegularExpressions.Regex.Replace(text!, "(\\B[A-Z])", " $1");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}