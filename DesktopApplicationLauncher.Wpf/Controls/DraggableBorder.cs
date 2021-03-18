namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System;
    using System.ComponentModel;
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

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DraggableBorder), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DraggableBorder), new PropertyMetadata(null));

        [Bindable(true)]
        [Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        [Bindable(true)]
        [Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

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
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (ContextMenu != null)
                {
                    ContextMenu.Tag = Tag;
                    ContextMenu.DataContext = DataContext;
                    ContextMenu.IsOpen = true;
                }

                e.Handled = true;
                return;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            e.Handled = true;

            _dragStartMousePosition = GetCurrentMousePosition();
            _isMoving = true;

            SetZIndex(1000);
            CaptureMouse();
        }

        private void DraggableBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            StopMoving();

            if (_dragStartMousePosition == GetCurrentMousePosition())
            {
                var command = Command;
                if (command != null && command.CanExecute(CommandParameter))
                {
                    command.Execute(CommandParameter);
                }
            }
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
            if (!_isMoving || e.LeftButton != MouseButtonState.Pressed)
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
