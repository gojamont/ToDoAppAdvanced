using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ToDoAdvanced.Converters;

public class NotificationConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
            return count > 0 ? $"My To-Do List" : "No tasks to display";
        return "My To-Do List";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}