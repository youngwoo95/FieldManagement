using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PlantManagement.Comm.Converters;

public sealed class DoubleToGridLengthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width && !double.IsNaN(width) && width >= 0)
            return new GridLength(width, GridUnitType.Pixel);

        return new GridLength(0, GridUnitType.Pixel);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is GridLength gridLength)
            return Math.Max(0, gridLength.Value);

        return 0d;
    }
}
