﻿namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

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
                            var solidColorBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#c2cef6");
                            solidColorBrush.Opacity = 0.3;

                            item.Background = solidColorBrush;
                        }
                        else
                        {
                            item.Background = Brushes.Transparent;
                        }
                    });
        }

        private void DraggableBorder_StopMove(object sender, EventArgs e)
        {
            SwapItemsOnCollision(sender);
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
                            ItemsSwapped?.Execute((draggableBorder.Index, item.Index));

                            SetDraggableItemsIndexes();
                        }
                    });
        }

        private void SetDraggableItemsIndexes()
        {
            var index = 0;
            ForEachItems(draggableBorder => draggableBorder.Index = index++);
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
            return Children.Count == 0 ? null : Children[0] as ItemsControl;
        }
    }
}