using System.Windows;
using System.Windows.Input;
using Scrappy.Core;

namespace Scrappy.Views
{
    public partial class NavigatorView
    {
        private readonly NavigationCollection collection;

        public NavigatorView()
        {
            this.InitializeComponent();
            this.collection = new NavigationCollection();
        }

        public void Push(NavigationEntry entry)
        {
            navigation.ItemsSource = collection.Push(entry);
        }

        public void Pop(NavigationEntry entry)
        {
            navigation.ItemsSource = collection.Pop(entry);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            NavigationEntry entry = source?.DataContext as NavigationEntry;

            navigation.ItemsSource = collection.Pop(entry);
        }
    }
}