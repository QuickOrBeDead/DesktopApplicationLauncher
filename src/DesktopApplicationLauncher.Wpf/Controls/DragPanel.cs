namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using MahApps.Metro.Controls;

    public sealed class DragPanel : StackPanel
    {
        public static readonly DependencyProperty SwapItemsProperty = DependencyProperty.Register("SwapItems", typeof(ICommand), typeof(DragPanel), new PropertyMetadata(null));
        public static readonly DependencyProperty ItemsMoveProperty = DependencyProperty.Register("ItemsMove", typeof(ICommand), typeof(DragPanel), new PropertyMetadata(null));

        public ICommand SwapItems
        {
            get => (ICommand)GetValue(SwapItemsProperty);
            set => SetValue(SwapItemsProperty, value);
        }

        public ICommand ItemsMove
        {
            get => (ICommand)GetValue(ItemsMoveProperty);
            set => SetValue(ItemsMoveProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var itemsControl = GetItemsControl();
            if (itemsControl != null)
            {
                itemsControl.ItemContainerGenerator.StatusChanged += (o, _) =>
                    {
                        var itemContainerGenerator = (ItemContainerGenerator)o;
                        if (itemContainerGenerator == null || itemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                        {
                            return;
                        }

                        var items = itemsControl.Items;
                        for (var i = 0; i < items.Count; i++)
                        {
                            var contentPresenter = (ContentPresenter)itemContainerGenerator.ContainerFromIndex(i);
                            var index = i;
                            contentPresenter.Loaded += (cp, _) =>
                                {
                                    var draggableBorder = ((ContentPresenter)cp).FindChild<DraggableBorder>();
                                    draggableBorder.Index = index;
                                    draggableBorder.StopMove += DraggableBorder_StopMove;
                                    draggableBorder.Move += DraggableBorder_Move;
                                };
                        }
                    };
            }
        }

        private void DraggableBorder_Move(object sender, EventArgs e)
        {
            var source = (DraggableBorder)sender;
            ForEachItems(
                target =>
                    {
                        if (ReferenceEquals(source, target))
                        {
                            return true;
                        }

                        var itemIsOver = CheckItemIsOver(source, target);
                        ItemsMove?.Execute((source.Index, target.Index, itemIsOver, false));

                        target.SetSwapItemsAdorner(GetItemsSwapAdornerLocation(source, target));

                        return true;
                    });
        }

        private void DraggableBorder_StopMove(object sender, EventArgs e)
        {
            var source = (DraggableBorder)sender;
            if (source == null || !IsDraggableItemExists(source))
            {
                return;
            }

            ForEachItems(
                target =>
                    {
                        if (target == null)
                        {
                            return true;
                        }

                        target.SetSwapItemsAdorner(DraggableBorderSwapItemsAdornerLocation.None);

                        if (ReferenceEquals(source, target))
                        {
                            return true;
                        }

                        if (CheckItemsSwap(source, target))
                        {
                            SwapItems?.Execute((source.Index, target.Index));

                            SetDraggableItemsIndexes();
                        }

                        var itemIsOver = CheckItemIsOver(source, target);
                        ItemsMove?.Execute((source.Index, target.Index, itemIsOver, true));

                        return true;
                    });
        }

        private static bool CheckItemIsOver(UIElement source, UIElement target)
        {
            var relativeLocation = target.TranslatePoint(new Point(0, 0), source);
            return Math.Abs(relativeLocation.X) < 25 && Math.Abs(relativeLocation.Y) < 25;
        }

        private static DraggableBorderSwapItemsAdornerLocation GetItemsSwapAdornerLocation(DraggableBorder source, DraggableBorder target)
        {
            var itemsSwap = CheckItemsSwap(source, target);
            var adornerLocation = DraggableBorderSwapItemsAdornerLocation.None;
            if (itemsSwap)
            {
                adornerLocation = source.Index > target.Index ? DraggableBorderSwapItemsAdornerLocation.Left : DraggableBorderSwapItemsAdornerLocation.Right;
            }

            return adornerLocation;
        }

        private static bool CheckItemsSwap(DraggableBorder source, DraggableBorder target)
        {
            var sourceIndexIsBigger = source.Index > target.Index;
            var relativeLocation = target.TranslatePoint(new Point(0, 0), source);
            return ((sourceIndexIsBigger && relativeLocation.X > 25 && relativeLocation.X < 50) ||
                    (!sourceIndexIsBigger && relativeLocation.X < -25 && relativeLocation.X > -50))
                        && Math.Abs(relativeLocation.Y) < 25;
        }

        private void SetDraggableItemsIndexes()
        {
            var index = 0;
            ForEachItems(draggableBorder =>
                {
                    draggableBorder.Index = index++;
                    return true;
                });
        }

        private bool IsDraggableItemExists(DraggableBorder item)
        {
            var result = false;
            ForEachItems(
                x =>
                    {
                        if (ReferenceEquals(x, item))
                        {
                            result = true;
                            return false;
                        }

                        return true;
                    });

            return result;
        }

        private void ForEachItems(Func<DraggableBorder, bool> func)
        {
            var itemsControl = GetItemsControl();
            if (itemsControl == null)
            {
                return;
            }

            var items = itemsControl.Items;
            for (var i = 0; i < items.Count; i++)
            {
                var contentPresenter = (ContentPresenter)itemsControl.ItemContainerGenerator.ContainerFromIndex(i);

                var draggableBorder = contentPresenter.FindChild<DraggableBorder>();
                var @continue = func(draggableBorder);
                if (!@continue)
                {
                    break;
                }
            }
        }

        private ItemsControl GetItemsControl()
        {
            return Children.Count == 0 ? null : Children[0] as ItemsControl;
        }
    }
}