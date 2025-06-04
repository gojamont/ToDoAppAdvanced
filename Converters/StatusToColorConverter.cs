using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ToDoAdvanced.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "Completed" => new SolidColorBrush(Color.Parse("#4CAF50")), // Green
            "InProgress" => new SolidColorBrush(Color.Parse("#FF9800")), // Orange
            "NotStarted" => new SolidColorBrush(Color.Parse("#E0115F")),
            _ => new SolidColorBrush(Color.Parse("#9E9E9E")) // Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}