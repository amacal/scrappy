using System.Windows;
using System.Windows.Input;
using Scrappy.Noom;

namespace Scrappy.Views.Rutor
{
    public partial class RutorListView : ICachable
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
            dynamic context = source?.DataContext;

            navigator.NavigateTo($"/Rutor/{context.Year}/{context.Title}");
        }
    }
}