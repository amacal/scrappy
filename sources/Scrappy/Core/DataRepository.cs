using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scrappy.Core
{
    public class DataRepository
    {
        private static readonly string Path = @"d:\\scrappy.json";

        public async Task<DataCollection> Get()
        {
            string data = File.Exists(Path) ? File.ReadAllText(Path) : "{}";
            IDictionary<string, object> items = JsonConvert.DeserializeObject<IDictionary<string, object>>(data);
            DataStore store = new DataStore(items);

            return new DataCollection(store);
        }

        public async Task Update(DataCollection collection)
        {
            IDictionary<string, object> items = new Dictionary<string, object>();
            DataStore store = new DataStore(items);

            collection.Visit(store);

            string data = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(Path, data);
        }
    }
}