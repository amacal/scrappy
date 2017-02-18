using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Scrappy.Views.Thirty
{
    public partial class ThirtyDetailsView
    {
        public ThirtyDetailsView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            dynamic context = source?.DataContext;

            Process.Start(context.Path);
        }
    }
}