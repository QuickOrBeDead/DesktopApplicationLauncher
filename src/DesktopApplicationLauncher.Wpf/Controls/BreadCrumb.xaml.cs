namespace DesktopApplicationLauncher.Wpf.Controls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// Interaction logic for BreadCrumb.xaml
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    public partial class BreadCrumb : UserControl
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IEnumerable), typeof(BreadCrumb), new FrameworkPropertyMetadata(null, OnItemsChanged));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(BreadCrumb), new PropertyMetadata(null));

        public IEnumerable Items
        {
            get => (IEnumerable)GetValue(ItemsProperty);
            set
            {
                if (value == null)
                {
                    ClearValue(ItemsProperty);
                }
                else
                {
                    SetValue(ItemsProperty, value);
                }
            }
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BreadCrumb)d).SetItems((IEnumerable)e.NewValue);
        }

        public BreadCrumb()
        {
            InitializeComponent();
        }

        private void SetItems(IEnumerable items)
        {
            var children = ContentPanel.Children;
            children.Clear();

            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                var linkButton = new Button { Content = item.ToString() };
                linkButton.Click += LinkButton_Click;
                linkButton.Tag = item;
                children.Add(linkButton);

                children.Add(new Label { Content = "/" });
            }

            if (children.Count > 0)
            {
                children.RemoveAt(children.Count - 1);
            }

            if (children.Count > 0)
            {
                children[^1].IsEnabled = false;
            }
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            Command?.Execute(((Button)sender).Tag);
        }
    }
}
