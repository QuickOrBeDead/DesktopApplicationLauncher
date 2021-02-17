namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public sealed class DraggableBorder : Border
    {
        private bool _isMoving;
        private Point? _itemPosition;
        private double _deltaX;
        private double _deltaY;
        private TranslateTransform _currentTranslateTransform;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            MouseDown += DraggableBorder_MouseDown;
            MouseUp += DraggableBorder_MouseUp;
            MouseMove += DraggableBorder_MouseMove;
        }

        private void DraggableBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var container = GetParent();
            if (container == null)
            {
                return;
            }

            _itemPosition ??= TransformToAncestor(container).Transform(new Point(0, 0));

            var mousePosition = Mouse.GetPosition(container);
            _deltaX = mousePosition.X - _itemPosition.Value.X;
            _deltaY = mousePosition.Y - _itemPosition.Value.Y;
            _isMoving = true;

            SetZIndex(1000);
           
            CaptureMouse();
        }

        private void DraggableBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentTranslateTransform = RenderTransform as TranslateTransform;
            _isMoving = false;

            SetZIndex(0);
            ReleaseMouseCapture();
        }

        private void DraggableBorder_MouseMove(object sender, MouseEventArgs e)
        {
            var container = GetParent();
            if (container == null)
            {
                return;
            }

            if (!_isMoving)
            {
                return;
            }

            var mousePoint = Mouse.GetPosition(container);

            var offsetX = (_currentTranslateTransform == null ? _itemPosition.Value.X : _itemPosition.Value.X - _currentTranslateTransform.X) + _deltaX - mousePoint.X;
            var offsetY = (_currentTranslateTransform == null ? _itemPosition.Value.Y : _itemPosition.Value.Y - _currentTranslateTransform.Y) + _deltaY - mousePoint.Y;

            RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        }

        private void SetZIndex(int value)
        {
            if (TemplatedParent is ContentPresenter presenter)
            {
                Panel.SetZIndex(presenter, value);
            }
            else
            {
                Panel.SetZIndex(this, value);
            }
        }

        private UIElement GetParent()
        {
            return VisualTreeHelper.GetParent(this) as UIElement;
        }
    }
}
