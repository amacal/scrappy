using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Scrappy.Tick;

namespace Scrappy.Tasks
{
    public class ThirtyItemsTask : IFactory
    {
        public TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(15); }
        }

        public IEnumerable<ITask> Create()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new Instance(i);
            }
        }

        private class Instance : ITask
        {
            private readonly int page;

            public Instance(int page)
            {
                this.page = page;
            }

            public string Name
            {
                get { return $"thirty-items-{page}"; }
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

                collection.Apply(await crawler.List(page));
                await repository.Update(collection);
            }
        }
    }
}