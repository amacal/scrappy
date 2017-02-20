using System;
using System.Windows.Controls;

namespace Scrappy.Noom
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
                bool isStatic = typeof(IStatic).IsAssignableFrom(typeof(TControl));
                UserControl control = GetInstance();

                destination.Content = null;

                if (payload != null)
                {
                    if (control.DataContext == null || isStatic == false)
                    {
                        control.DataContext = payload;
                    }
                }

                destination.Content = control;
            }

            private UserControl GetInstance()
            {
                Type target = typeof(TControl);

                if (typeof(ICachable).IsAssignableFrom(target))
                {
                    return tools.Cache.Resolve(target, GetInstance);
                }

                return GetInstance(target);
            }

            private UserControl GetInstance(Type type)
            {
                return tools.Resolver.Resolve<TControl>();
            }
        }
    }
}