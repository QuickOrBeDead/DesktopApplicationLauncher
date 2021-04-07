namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    public sealed class ParentFolderModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
