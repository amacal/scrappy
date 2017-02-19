using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Tick;

namespace Scrappy.Tasks
{
    public class ThirtyItemsTask : ITask
    {
        public string Name
        {
            get { return "thirty-items"; }
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromHours(1); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            ThirtyCrawler crawler = new ThirtyCrawler();
            IEnumerable<int> pages = Enumerable.Range(1, 10);

            foreach (int page in pages)
            {
                collection.Apply(await crawler.List(page));

                await repository.Update(collection);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}