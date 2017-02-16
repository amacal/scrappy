using System;
using System.Windows;
using System.Windows.Input;
using Noom;
using Scrappy.Core;

namespace Scrappy.Views
{
    public partial class RutorListView
    {
        private readonly INavigator navigator;

        public RutorListView(INavigator navigator)
        {
            this.navigator = navigator;
            this.InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            DataGroup item = source?.DataContext as DataGroup;

            navigator.NavigateTo($"/{item.Title}", item);
        }
    }
}