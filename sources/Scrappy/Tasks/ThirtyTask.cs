using System;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Tick;

namespace Scrappy.Tasks
{
    public class ThirtyTask : ITask
    {
        public TimeSpan Interval
        {
            get { return TimeSpan.FromHours(1); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            ThirtyCrawler crawler = new ThirtyCrawler();
            ThirtyItem[] items = await crawler.List();

            collection.Apply(items);
            await repository.Update(collection);

            foreach (ThirtyItem item in collection.MissingDetails())
            {
                collection.Apply(await crawler.Details(item.Id));
            }

            await repository.Update(collection);
        }
    }
}