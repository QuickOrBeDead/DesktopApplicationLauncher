namespace DesktopApplicationLauncher.Wpf.Converters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    using FontAwesome.Sharp;

    using Icon = System.Drawing.Icon;

    public sealed class ApplicationItemToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ApplicationListItemModel applicationItem)
            {
                if (applicationItem.ItemType == ApplicationItemType.Website)
                {
                    return IconChar.Chrome.ToImageSource(System.Windows.Media.Brushes.RoyalBlue, 32);
                }

                var filePath = applicationItem.Path;
                using var iconForFile = GetIconForFile(filePath);

                return GetBitmapSource(iconForFile);
            }

            return GetBitmapSource(SystemIcons.WinLogo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Reviewed")]
        private static Icon GetIconForFile(string filePath)
        {
            var defaultIcon = SystemIcons.WinLogo;
            var iconForFile = defaultIcon;
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    iconForFile = DefaultIcons.FolderLarge;
                }
                else if (File.Exists(filePath))
                {
                    iconForFile = Icon.ExtractAssociatedIcon(filePath) ?? defaultIcon;
                }
            }
            catch
            {
                // Empty
                iconForFile = defaultIcon;
            }

            return iconForFile;
        }

        private static BitmapSource GetBitmapSource(Icon iconForFile)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                iconForFile.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        private static class DefaultIcons
        {
            private static readonly Lazy<Icon> _lazyFolderIcon = new(FetchFolderIcon, true);

            public static Icon FolderLarge => _lazyFolderIcon.Value;

            private static Icon FetchFolderIcon()
            {
                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;
                var icon = ExtractFromPath(tempDir);
                Directory.Delete(tempDir);
                return icon;
            }

            [SuppressMessage("Minor Code Smell", "CA1704:Identifiers should be spelled correctly", Justification = "<Pending>")]
            private static Icon ExtractFromPath(string path)
            {
                var shinfo = new SHFILEINFO();
                SHGetFileInfo(
                    path,
                    0, ref shinfo, (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON);
                return Icon.FromHandle(shinfo.hIcon);
            }

            //Struct used by SHGetFileInfo function
            [StructLayout(LayoutKind.Sequential)]
            [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
            [SuppressMessage("Minor Code Smell", "CA1704:Identifiers should be spelled correctly", Justification = "<Pending>")]
            [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>")]
            private struct SHFILEINFO
            {
                public IntPtr hIcon;
                public int iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            };

            [DllImport(dllName: "shell32.dll", CharSet = CharSet.Unicode)]
            [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
            [SuppressMessage("Minor Code Smell", "CA1704:Identifiers should be spelled correctly", Justification = "<Pending>")]
            private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

            [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
            [SuppressMessage("Minor Code Smell", "CA1704:Identifiers should be spelled correctly", Justification = "<Pending>")]
            private const uint SHGFI_ICON = 0x100;
        }
    }
}