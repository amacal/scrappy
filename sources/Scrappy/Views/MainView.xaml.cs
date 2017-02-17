using System;
using System.Windows;
using System.Windows.Input;
using Noom;
using Tick;

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
            TickScheduler.Initialize();
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