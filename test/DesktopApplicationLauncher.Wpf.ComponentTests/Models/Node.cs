namespace DesktopApplicationLauncher.Wpf.ComponentTests.Models
{
    using System;
    using System.Collections.Generic;

    public sealed class Node<TItem>
    {
        private IList<Node<TItem>> _children;

        public TItem Item { get; }

        public Node(TItem item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public IList<Node<TItem>> Children
        {
            get => _children ??= new List<Node<TItem>>();
            set => _children = value;
        }
    }
}