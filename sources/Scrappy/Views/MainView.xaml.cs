using System;
using System.Windows;
using System.Windows.Input;
using Scrappy.Core;
using Scrappy.Noom;
using Scrappy.Tick;

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
            TickScheduler.Initialize(new TickToLogger());
            NoomStarter.Initialize(content, path);

            base.OnInitialized(e);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            ISegment segment = source?.DataContext as ISegment;

            segment.Activate();
        }

        private class TickToLogger : IFeedback
        {
            public void OnStarted(ITask task, DateTime timestamp)
            {
                Logger.Info($"Task '{task.Name}' started.");
            }

            public void OnCompleted(ITask task, TimeSpan duration)
            {
                Logger.Info($"Task '{task.Name}' completed within '{duration.Seconds:F1}' seconds.");
            }

            public void OnFailed(ITask task, TimeSpan duration, Exception reason)
            {
                Logger.Info($"Task '{task.Name}' failed after '{duration.Seconds:F1}' seconds; reason='{reason.Message}'");
            }
        }
    }
}