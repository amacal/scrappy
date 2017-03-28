using System.Windows;
using System.Windows.Input;
using Scrappy.Noom;

namespace Scrappy.Views.Rutor
{
    public partial class RutorMovieListView : ICachable, IPageable
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

        bool IPageable.CanPrev()
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            return page > 0;
        }

        void IPageable.OnPrev()
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            navigator.NavigateTo($"/Rutor", page - 1);
        }

        bool IPageable.CanNext()
        {
            dynamic context = DataContext;
            int count = context.Length;

            return count == 21;
        }

        void IPageable.OnNext()
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            navigator.NavigateTo($"/Rutor", page + 1);
        }
    }
}