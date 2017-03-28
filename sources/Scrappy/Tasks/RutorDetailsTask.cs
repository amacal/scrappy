using System;
using System.Linq;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Tick;

namespace Scrappy.Tasks
{
    public class RutorDetailsTask : ITask
    {
        public string Name
        {
            get { return "rutor-details"; }
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMinutes(10); }
        }

        public async Task<TimeSpan> Execute()
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            RutorCrawler crawler = new RutorCrawler();
            RutorItem missing = collection.MissingDetails().FirstOrDefault();

            if (missing != null)
            {
                collection.Apply(missing.Id, await crawler.Details(missing.Id));
                await repository.Update(collection);

                return TimeSpan.FromSeconds(10);
            }

            return TimeSpan.FromMinutes(10);
        }
    }
}