using System.Windows;
using System.Windows.Input;
using Noom;

namespace Scrappy.Views.Thirty
{
    public partial class ThirtyListView
    {
        private readonly INavigator navigator;

        public ThirtyListView(INavigator navigator)
        {
            this.navigator = navigator;
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            dynamic context = source?.DataContext;

            navigator.NavigateTo($"/Thirty/{context.Id}");
        }
    }
}