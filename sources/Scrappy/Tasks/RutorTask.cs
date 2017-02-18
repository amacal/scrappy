using System;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Imdb;
using Scrappy.Core.Rutor;
using Tick;

namespace Scrappy.Tasks
{
    public class RutorTask : ITask
    {
        public TimeSpan Interval
        {
            get { return TimeSpan.FromHours(1); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            RutorCrawler rutor = new RutorCrawler();
            RutorItem[] items = await rutor.List();

            collection.Apply(items);
            await repository.Update(collection);

            foreach (RutorItem item in collection.MissingDetails())
            {
                collection.Apply(await rutor.Details(item.Id));
            }

            await repository.Update(collection);
            ImdbCrawler imdb = new ImdbCrawler();

            foreach (RutorDetails details in collection.MissingImdb())
            {
                collection.Apply(await imdb.Details(details.Imdb));
            }

            await repository.Update(collection);
        }
    }
}