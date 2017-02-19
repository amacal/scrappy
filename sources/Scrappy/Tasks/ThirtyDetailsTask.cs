using System;
using System.Linq;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Scrappy.Tick;

namespace Scrappy.Tasks
{
    public class ThirtyDetailsTask : ITask
    {
        public string Name
        {
            get { return "thirty-details"; }
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(10); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            ThirtyCrawler crawler = new ThirtyCrawler();
            ThirtyItem missing = collection.MissingDetails().FirstOrDefault();

            if (missing != null)
            {
                collection.Apply(await crawler.Details(missing.Id));
                await repository.Update(collection);
            }
        }
    }
}