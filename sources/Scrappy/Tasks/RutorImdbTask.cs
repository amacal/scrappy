using System;
using System.Linq;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Imdb;
using Scrappy.Core.Rutor;
using Scrappy.Tick;

namespace Scrappy.Tasks
{
    public class RutorImdbTask : ITask
    {
        public string Name
        {
            get { return "rutor-imdb"; }
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromSeconds(10); }
        }

        public async Task Execute()
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            ImdbCrawler crawler = new ImdbCrawler();
            RutorDetails missing = collection.MissingImdb().FirstOrDefault();

            if (missing != null)
            {
                collection.Apply(await crawler.Details(missing.Imdb));
                await repository.Update(collection);
            }
        }
    }
}