﻿namespace DesktopApplicationLauncher.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public sealed class ApplicationItemVisibleIfNullOrWhiteSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ApplicationListItemModel applicationItem)
            {
                return !string.IsNullOrWhiteSpace(applicationItem.Name)
                       && !string.IsNullOrWhiteSpace(applicationItem.Path);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}