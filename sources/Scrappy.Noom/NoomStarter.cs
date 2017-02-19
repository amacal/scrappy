using System;
using System.Windows.Controls;
using TinyIoC;

namespace Scrappy.Noom
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
            TinyIoCContainer container = new TinyIoCContainer();
            NoomResolver resolver = new NoomResolver(container);

            NoomTools tools = new NoomTools(resolver);
            NoomRouter router = new NoomRouter();

            NoomNavigator navigator = new NoomNavigator(router, destination, tools);

            container.Register<IRouter>(router);
            container.Register<INavigator>(navigator);
            container.Register<IResolver>(resolver);
            container.RegisterMultiple(typeof(IModule), bootstrapper.FindAllModules());

            foreach (Type type in bootstrapper.FindAllViews())
            {
                container.Register(type);
            }

            foreach (IModule module in container.ResolveAll<IModule>())
            {
                module.Register(router);
            }

            navigator.NavigateTo("/");
        }
    }
}