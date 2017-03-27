using System.Windows;
using System.Windows.Input;
using Scrappy.Noom;

namespace Scrappy.Views.Rutor
{
    public partial class RutorMovieDetailsView
    {
        private readonly INavigator navigator;

        public RutorMovieDetailsView(INavigator navigator)
        {
            this.navigator = navigator;
            this.InitializeComponent();
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            dynamic context = source?.DataContext;
            dynamic parent = this.DataContext;

            navigator.NavigateTo($"/Rutor/{parent.Group.Year}/{parent.Group.Title}/{context.Id}");
        }
    }
}