namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    public sealed class ListItemModel<TValue>
    {
        public string Text { get; set; }

        public TValue Value { get; set; }
    }
}
