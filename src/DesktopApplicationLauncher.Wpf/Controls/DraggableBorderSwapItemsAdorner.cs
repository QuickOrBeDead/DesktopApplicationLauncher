namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    public sealed class DraggableBorderSwapItemsAdorner : Adorner
    {
        public DraggableBorderSwapItemsAdornerLocation Location { get; set; }

        public DraggableBorderSwapItemsAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Location == DraggableBorderSwapItemsAdornerLocation.None)
            {
                return;
            }

            var adornedElementRect = new Rect(AdornedElement.RenderSize);
            var brush = new SolidColorBrush(Colors.Black) { Opacity = 0.4 };
            var pen = new Pen(brush, 2);

            if (Location == DraggableBorderSwapItemsAdornerLocation.Left)
            {
                drawingContext.DrawLine(pen, adornedElementRect.TopLeft, adornedElementRect.BottomLeft);
            }
            else
            {
                drawingContext.DrawLine(pen, adornedElementRect.TopRight, adornedElementRect.BottomRight);
            }
        }
    }
}