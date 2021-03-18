namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using MahApps.Metro.Controls;

    public sealed class DragPanel : StackPanel
    {
        public static readonly DependencyProperty ItemsSwappedProperty = DependencyProperty.Register("ItemsSwapped", typeof(ICommand), typeof(DragPanel), new PropertyMetadata(null));

        public ICommand ItemsSwapped
        {
            get => (ICommand)GetValue(ItemsSwappedProperty);
            set => SetValue(ItemsSwappedProperty, value);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Loaded += DragPanel_Loaded;
        }

        private void DragPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ForEachItems(draggableBorder => draggableBorder.StopMove += DraggableBorder_StopMove);

            SetDraggableItemsIndexes();
        }

        private void DraggableBorder_StopMove(object sender, EventArgs e)
        {
            SwapItemsOnCollision(sender);
        }

        private void SetDraggableItemsIndexes()
        {
            var index = 0;
            ForEachItems(
                draggableBorder =>
                    {
                        draggableBorder.Index = index++;
                    });
        }

        private void SwapItemsOnCollision(object sender)
        {
            var draggableBorder = (DraggableBorder)sender;
            ForEachItems(
                item =>
                    {
                        if (ReferenceEquals(draggableBorder, item))
                        {
                            return;
                        }

                        var relativeLocation = item.TranslatePoint(new Point(0, 0), draggableBorder);
                        if (Math.Abs(relativeLocation.X) < 25 && Math.Abs(relativeLocation.Y) < 25)
                        {
                            var itemsControl = GetItemsControl();
                            if (itemsControl != null)
                            {
                                var list = (IList)itemsControl.ItemsSource;

                                ReplaceItems(list, draggableBorder.Index, item.Index);

                                SetDraggableItemsIndexes();
                            }
                        }
                    });
        }

        private void ReplaceItems(IList list, int sourceIndex, int targetIndex)
        {
            var target = list[targetIndex];
            list[targetIndex] = list[sourceIndex];

            if (sourceIndex < targetIndex)
            {
                for (var i = targetIndex - 1; i >= sourceIndex; i--)
                {
                    var next = list[i];
                    list[i] = target;
                    target = next;
                }
            }
            else
            {
                for (var i = targetIndex + 1; i <= sourceIndex; i++)
                {
                    var next = list[i];
                    list[i] = target;
                    target = next;
                }
            }

            var itemsSwapped = ItemsSwapped;
            if (itemsSwapped != null)
            {
                var sourceTargetTuple = (Math.Min(sourceIndex, targetIndex), Math.Max(sourceIndex, targetIndex));
                if (itemsSwapped.CanExecute(sourceTargetTuple))
                {
                    itemsSwapped.Execute(sourceTargetTuple);
                }
            }
        }

        private void ForEachItems(Action<DraggableBorder> action)
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
                action(draggableBorder);
            }
        }

        private ItemsControl GetItemsControl()
        {
            var itemsControl = Children.Count == 0 ? null : Children[0] as ItemsControl;
            return itemsControl;
        }
    }
}