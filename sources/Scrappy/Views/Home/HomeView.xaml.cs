﻿using System.Windows;
using System.Windows.Input;
using Scrappy.Noom;

namespace Scrappy.Views.Home
{
    public partial class HomeView
    {
        private readonly INavigator navigator;

        public HomeView(INavigator navigator)
        {
            this.navigator = navigator;
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            dynamic context = source?.DataContext;

            navigator.NavigateTo(context.Path);
        }
    }
}