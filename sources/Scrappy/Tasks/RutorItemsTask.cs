using System;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Imdb;
using Scrappy.Core.Rutor;
using Tick;

namespace Scrappy.Tasks
{
    public class RutorItemsTask : ITask
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
        }
    }
}