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
        private Point _dragStartMousePosition;
        private UIElement _parent;

        public event EventHandler Move = delegate { };

        public event EventHandler StopMove = delegate { };

        public int Index { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _parent = GetParent();

            MouseDown += DraggableBorder_MouseDown;
            MouseUp += DraggableBorder_MouseUp;
            MouseMove += DraggableBorder_MouseMove;
        }

        private void DraggableBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            StartMoving();
        }

        public void StartMoving()
        {
            _dragStartMousePosition = GetCurrentMousePosition();
            _isMoving = true;

            SetZIndex(1000);
            CaptureMouse();
        }

        private void DraggableBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            StopMoving();
        }

        public void StopMoving()
        {
            _isMoving = false;

            RenderTransform = new TranslateTransform(0, 0);

            SetZIndex(0);
            ReleaseMouseCapture();

            StopMove(this, EventArgs.Empty);
        }

        private void DraggableBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMoving)
            {
                return;
            }

            var currentMousePosition = GetCurrentMousePosition();

            var offsetX = currentMousePosition.X - _dragStartMousePosition.X;
            var offsetY = currentMousePosition.Y - _dragStartMousePosition.Y;

            RenderTransform = new TranslateTransform(offsetX, offsetY);

            Move(this, EventArgs.Empty);
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

        private Point GetCurrentMousePosition()
        {
            return Mouse.GetPosition(_parent);
        }
    }
}
