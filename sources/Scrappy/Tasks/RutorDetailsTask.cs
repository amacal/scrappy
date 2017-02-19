using System;
using System.Linq;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Tick;

namespace Scrappy.Tasks
{
    public class RutorDetailsTask : ITask
    {
        public TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(10); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            RutorCrawler crawler = new RutorCrawler();
            RutorItem missing = collection.MissingDetails().FirstOrDefault();

            if (missing != null)
            {
                collection.Apply(await crawler.Details(missing.Id));
                await repository.Update(collection);
            }
        }
    }
}