using System;
using System.Windows.Controls;
using TinyIoC;

namespace Noom
{
    public static class NoomStarter
    {
        public static void Initialize(ContentControl control)
        {
            Initialize(new NoomDestination(control), new NoomBootstrapper());
        }

        public static void Initialize(ContentControl control, ItemsControl items)
        {
            Initialize(new NoomDestination(control, items), new NoomBootstrapper());
        }

        public static void Initialize(ContentControl control, IBootstrapper bootstrapper)
        {
            Initialize(new NoomDestination(control), bootstrapper);
        }

        public static void Initialize(ContentControl control, ItemsControl items, IBootstrapper bootstrapper)
        {
            Initialize(new NoomDestination(control, items), bootstrapper);
        }

        private static void Initialize(IDestination destination, IBootstrapper bootstrapper)
        {
            NoomRouter router = new NoomRouter();
            NoomNavigator navigator = new NoomNavigator(router, destination);
            TinyIoCContainer container = new TinyIoCContainer();

            container.Register<IRouter>(router);
            container.Register<INavigator>(navigator);

            foreach (Type type in bootstrapper.FindAllModules())
            {
                container.Register(typeof(IModule), type);
            }

            foreach (IModule module in container.ResolveAll<IModule>())
            {
                module.Register(router);
            }

            navigator.NavigateTo("/");
        }
    }
}