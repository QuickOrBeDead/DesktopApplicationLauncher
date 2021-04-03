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

    public class FilePathToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filePath = (string)value;
            using var iconForFile = GetIconForFile(filePath);

            return Imaging.CreateBitmapSourceFromHIcon(iconForFile.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
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

        private static class DefaultIcons
        {
            private static readonly Lazy<Icon> _lazyFolderIcon = new(FetchIcon, true);

            public static Icon FolderLarge => _lazyFolderIcon.Value;

            private static Icon FetchIcon()
            {
                var tmpDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;
                var icon = ExtractFromPath(tmpDir);
                Directory.Delete(tmpDir);
                return icon;
            }

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
            private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

            private const uint SHGFI_ICON = 0x100;
        }
    }
}