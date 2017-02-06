using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Scrappy.Core;

namespace Scrappy.Views
{
    public partial class View
    {
        private DataCollection collection;
        private NavigationCollection navigator;

        public View()
        {
            this.InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.InitializeBinding();
        }

        private async void InitializeBinding()
        {
            DataRepository repository = new DataRepository();
            RutorCrawler rutor = new RutorCrawler();
            ImdbCrawler imdb = new ImdbCrawler();

            navigator = new NavigationCollection();
            navigation.ItemsSource = navigator.Push(ToRutor());

            collection = await repository.Get();
            IEnumerable<RutorItem> found = await rutor.List();

            collection.Apply(found);
            await repository.Update(collection);

            items.ItemsSource = collection.Group();

            foreach (RutorItem data in collection.MissingDetails())
            {
                RutorDetails details = await rutor.Details(data.Id);

                if (details != null)
                {
                    collection.Apply(details);
                    await repository.Update(collection);
                    await Task.Delay(1000);
                }
            }

            items.ItemsSource = collection.Group();

            foreach (RutorDetails data in collection.MissingImdb())
            {
                ImdbDetails details = await imdb.Details(data.Imdb);

                if (details != null)
                {
                    collection.Apply(details);
                    await repository.Update(collection);
                    await Task.Delay(1000);
                }
            }

            items.ItemsSource = collection.Group();
        }

        private NavigationEntry ToRutor()
        {
            return new NavigationEntry
            {
                Title = "Rutor",
                Execute = () =>
                {
                    details.Visibility = Visibility.Collapsed;
                    items.Visibility = Visibility.Visible;
                }
            };
        }

        private NavigationEntry ToDetails(DataGroup item)
        {
            return new NavigationEntry
            {
                Title = item.Title,
                Execute = () =>
                {
                    items.Visibility = Visibility.Collapsed;
                    details.DataContext = collection.Details(item);
                    details.Visibility = Visibility.Visible;
                }
            };
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            DataGroup item = source?.DataContext as DataGroup;

            items.Visibility = Visibility.Collapsed;
            details.DataContext = collection.Details(item);
            details.Visibility = Visibility.Visible;
            navigation.ItemsSource = navigator.Push(ToDetails(item));
        }

        private void UIElement_OnMouseDown2(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            NavigationEntry entry = source?.DataContext as NavigationEntry;

            navigation.ItemsSource = navigator.Pop(entry);
        }
    }
}