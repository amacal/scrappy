using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private void OnContextMenu(object sender, MouseButtonEventArgs e)
        {
            Border source = sender as Border;
            bool? selected = source?.Tag as bool?;

            if (source != null)
                source.Tag = selected != true;

            if (Equals(source.Tag, true))
            {
                source.BorderBrush = Brushes.Red;
            }
            else
            {
                source.BorderBrush = Brushes.Transparent;
            }
        }

        bool IPageable.CanPrev(IRequest request)
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            return page > 0;
        }

        void IPageable.OnPrev(IRequest request)
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            navigator.NavigateTo(request.ToString(), page - 1);
        }

        bool IPageable.CanNext(IRequest request)
        {
            dynamic context = DataContext;
            int count = context.Length;

            return count == 21;
        }

        void IPageable.OnNext(IRequest request)
        {
            dynamic context = DataContext;
            int page = context[0].Page;

            navigator.NavigateTo(request.ToString(), page + 1);
        }
    }
}