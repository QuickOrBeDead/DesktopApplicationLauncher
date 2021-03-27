namespace DesktopApplicationLauncher.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    public class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Path.GetFileName((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}