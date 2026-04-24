using System;
using System.Globalization;
using System.Windows.Data;

namespace PlantManagement.Comm.Converters;

public sealed class SafeUriConverter : IValueConverter
{
    private static readonly Uri BlankUri = new("about:blank", UriKind.Absolute);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Uri uri)
            return uri;

        if (value is string text && Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out var parsed))
            return parsed;

        return BlankUri;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value as Uri ?? BlankUri;
    }
}
