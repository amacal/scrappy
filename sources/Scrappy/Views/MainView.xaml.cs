using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Scrappy.Core;

namespace Scrappy.Views
{
    public partial class MainView
    {
        private DataCollection collection;

        public MainView()
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

            navigator.Push(ToRutor());

            collection = await repository.Get();
            IEnumerable<RutorItem> found = await rutor.List();

            collection.Apply(found);
            await repository.Update(collection);

            rutorList.Group(collection);
            rutorList.WhenSelected(item =>
            {
                NavigationEntry entry = ToDetails(item);
                rutorDetails.DataContext = collection.Details(item);

                Enable(rutorDetails);
                navigator.Push(entry);
            });

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

            rutorList.Group(collection);

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

            rutorList.Group(collection);
        }

        private NavigationEntry ToRutor()
        {
            return new NavigationEntry
            {
                Title = "Rutor",
                Execute = () =>
                {
                    Enable(rutorList);
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
                    rutorDetails.DataContext = collection.Details(item);
                    Enable(rutorDetails);
                }
            };
        }

        private void Enable(FrameworkElement view)
        {
            foreach (UIElement child in owner.Children)
            {
                if (ReferenceEquals(child, view) == false &&
                    ReferenceEquals(child, navigator) == false)
                {
                    child.Visibility = Visibility.Collapsed;
                }
            }

            view.Visibility = Visibility.Visible;
        }
    }
}