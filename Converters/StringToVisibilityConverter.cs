using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ToDoAdvanced.Converters
{
    public class StringToVisibilityConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // values[0]: NotificationMessage (string)
            // values[1]: ToDoItemsCount (int)
            // values[2]: DeadlineReached (bool, optional)
            if (values.Count >= 3 && values[2] is bool deadlineReached && deadlineReached)
                return true;

            if (values.Count >= 2 &&
                values[0] is string message &&
                !string.IsNullOrEmpty(message) &&
                values[1] is int count &&
                count > 0)
            {
                return true;
            }
            return false;
        }

        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}