namespace DesktopApplicationLauncher.Wpf.Infrastructure.Entities
{
    using System.ComponentModel;

    public enum ApplicationItemType
    {
        /// <summary>The file</summary>
        [Description("File")]
        File = 0,

        /// <summary>The folder</summary>
        [Description("Folder")]
        Folder = 1,

        /// <summary>The website</summary>
        [Description("Website")]
        Website = 2
    }
}