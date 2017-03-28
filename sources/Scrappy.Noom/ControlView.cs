using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scrappy.Noom
{
    public static class ControlView
    {
        public static IViewFactory Using<TControl>()
            where TControl : UserControl
        {
            return new ControlView<TControl>(null);
        }

        public static IViewFactory Payload<TControl>(object payload)
            where TControl : UserControl
        {
            Func<Task<object>> callback = () =>
            {
                TaskCompletionSource<object> result = new TaskCompletionSource<object>();

                result.SetResult(payload);
                return result.Task;
            };

            return new ControlView<TControl>(callback);
        }

        public static IViewFactory PayloadAsync<TControl>(Func<Task<object>> payload)
            where TControl : UserControl
        {
            return new ControlView<TControl>(payload);
        }
    }

    internal class ControlView<TControl> : IViewFactory
        where TControl : UserControl
    {
        private readonly Func<Task<object>> payload;

        public ControlView(Func<Task<object>> payload)
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
            private readonly Func<Task<object>> payload;

            public Instance(IViewTools tools, Func<Task<object>> payload)
            {
                this.tools = tools;
                this.payload = payload;
            }

            public async Task Render(ContentControl destination, IViewHook hook)
            {
                bool isStatic = typeof(IStatic).IsAssignableFrom(typeof(TControl));
                UserControl control = GetInstance();

                if (payload != null)
                {
                    if (control.DataContext == null || isStatic == false)
                    {
                        object data = await payload.Invoke();

                        destination.Content = null;
                        control.DataContext = data;
                    }
                }

                destination.Content = control;
                hook.OnAttached(control);
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