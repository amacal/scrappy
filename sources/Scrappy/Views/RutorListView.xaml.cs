using System;
using System.Windows;
using System.Windows.Input;
using Scrappy.Core;

namespace Scrappy.Views
{
    public partial class RutorListView
    {
        private Action<DataGroup> onSelected;

        public RutorListView()
        {
            InitializeComponent();
        }

        public void Group(DataCollection collection)
        {
            items.ItemsSource = collection.Group();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            DataGroup item = source?.DataContext as DataGroup;

            onSelected?.Invoke(item);
        }

        public void WhenSelected(Action<DataGroup> callback)
        {
            onSelected = callback;
        }
    }
}