using System.Windows;
using System.Windows.Input;
using Scrappy.Noom;

namespace Scrappy.Views.Rutor
{
    public partial class RutorMovieListView : ICachable
    {
        private readonly INavigator navigator;

        public RutorMovieListView(INavigator navigator)
        {
            this.navigator = navigator;
            this.InitializeComponent();
        }

        private void OnMovieClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            dynamic context = source?.DataContext;

            navigator.NavigateTo($"/Rutor/{context.Year}/{context.Title}");
        }
    }
}