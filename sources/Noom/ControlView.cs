using System;
using System.Windows;
using System.Windows.Controls;

namespace Noom
{
    public class ControlView : IView
    {
        private readonly Func<FrameworkElement> create;
        private readonly object payload;

        public ControlView(Func<FrameworkElement> create)
        {
            this.create = create;
        }

        public ControlView(Func<FrameworkElement> create, object payload)
        {
            this.create = create;
            this.payload = payload;
        }

        public void Render(ContentControl destination)
        {
            FrameworkElement control = create();

            control.DataContext = payload;
            destination.Content = control;
        }
    }

    public class ControlView<TControl> : IView
        where TControl : FrameworkElement, new()
    {
        private readonly object payload;

        public ControlView()
        {
        }

        public ControlView(object payload)
        {
            this.payload = payload;
        }

        public void Render(ContentControl destination)
        {
            TControl control = new TControl();

            control.DataContext = payload;
            destination.Content = control;
        }
    }
}