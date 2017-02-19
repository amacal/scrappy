using System.Windows.Controls;

namespace Noom
{
    public class ControlView<TControl> : IViewFactory
        where TControl : UserControl
    {
        private readonly object payload;

        public ControlView()
        {
        }

        public ControlView(object payload)
        {
            this.payload = payload;
        }

        public IView Create(IViewTools tools)
        {
            return new Instance(tools, payload);
        }

        private class Instance : IView
        {
            private readonly IViewTools tools;
            private readonly object payload;

            public Instance(IViewTools tools, object payload)
            {
                this.tools = tools;
                this.payload = payload;
            }

            public void Render(ContentControl destination)
            {
                TControl control = tools.Resolver.Resolve<TControl>();

                destination.Content = null;

                if (payload != null)
                {
                    control.DataContext = payload;
                }

                destination.Content = control;
            }
        }
    }
}