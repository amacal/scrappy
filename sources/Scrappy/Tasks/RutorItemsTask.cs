using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Tick;

namespace Scrappy.Tasks
{
    public class RutorItemsTask : IFactory
    {
        public TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(15); }
        }

        public IEnumerable<ITask> Create()
        {
            yield return new Instance("1080p%20BDRemux");
            yield return new Instance("1080p%20BDRip");
        }

        private class Instance : ITask
        {
            private readonly string query;

            public Instance(string query)
            {
                this.query = query;
            }

            public string Name
            {
                get { return $"rutor-items-{Math.Abs(query.GetHashCode())}"; }
            }

            public TimeSpan Interval
            {
                get { return TimeSpan.FromHours(1); }
            }

            public async Task<TimeSpan> Execute()
            {
                DataRepository repository = new DataRepository();
                RutorCollection collection = await repository.Get<RutorCollection>();

                RutorCrawler rutor = new RutorCrawler();
                RutorItem[] items = await rutor.List(query);

                collection.Apply(items);
                await repository.Update(collection);

                return TimeSpan.FromHours(1);
            }
        }
    }
}