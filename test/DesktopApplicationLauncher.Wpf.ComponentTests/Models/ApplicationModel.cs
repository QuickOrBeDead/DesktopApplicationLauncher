namespace DesktopApplicationLauncher.Wpf.ComponentTests.Models
{
    public sealed class ApplicationModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int? ParentId { get; set; }

        public string HierarchyPath { get; set; }
    }
}
