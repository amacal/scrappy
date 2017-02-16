using System;
using System.Windows;
using System.Windows.Input;
using Noom;

namespace Scrappy.Views
{
    public partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            NoomStarter.Initialize(content, path);
            base.OnInitialized(e);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            ISegment segment = source?.DataContext as ISegment;

            segment.Activate();
        }
    }
}