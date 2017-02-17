using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scrappy.Core.Rutor;

namespace Scrappy.Core
{
    public class DataRepository
    {
        private static readonly string Path = @"d:\\scrappy.json";

        public async Task<RutorCollection> Get()
        {
            string data = "{}";

            if (File.Exists(Path))
            {
                using (FileStream stream = File.OpenRead(Path))
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = await reader.ReadToEndAsync();
                }
            }

            IDictionary<string, object> items = Deserialize(data);
            DataStore store = new DataStore(items);

            return new RutorCollection(store);
        }

        private IDictionary<string, object> Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(data);
        }

        public async Task Update(RutorCollection collection)
        {
            IDictionary<string, object> items = new Dictionary<string, object>();
            DataStore store = new DataStore(items);

            collection.Visit(store);

            string data = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(Path, data);
        }
    }
}