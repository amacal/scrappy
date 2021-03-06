﻿using System;
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

        public static void Initialize(ContentControl control, ItemsControl items, ItemsControl paging)
        {
            Initialize(new NoomDestination(control, items, paging), new NoomBootstrapper());
        }

        public static void Initialize(ContentControl control, ItemsControl paging, IBootstrapper bootstrapper)
        {
            Initialize(new NoomDestination(control), bootstrapper);
        }

        public static void Initialize(ContentControl control, ItemsControl items, ItemsControl paging, IBootstrapper bootstrapper)
        {
            Initialize(new NoomDestination(control, items, paging), bootstrapper);
        }

        private static void Initialize(IDestination destination, IBootstrapper bootstrapper)
        {
            TinyIoCContainer container = new TinyIoCContainer();
            NoomResolver resolver = new NoomResolver(container);

            NoomCache cache = new NoomCache();
            NoomTools tools = new NoomTools(resolver, cache);
            NoomRouter router = new NoomRouter();

            NoomNavigator navigator = new NoomNavigator(router, destination, tools);

            container.Register<ICache>(cache);
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