using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scrappy.Core
{
    public class DataRepository
    {
        private static readonly string Root = @"d:\\";

        public async Task<TCollection> Get<TCollection>()
            where TCollection : DataCollection, new()
        {
            TCollection collection = new TCollection();

            string data = "{}";
            string path = Path.Combine(Root, $"{collection.Name}.json");

            if (File.Exists(path))
            {
                using (FileStream stream = File.OpenRead(path))
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = await reader.ReadToEndAsync();
                }
            }

            IDictionary<string, object> items = Deserialize(data);
            DataStore store = new DataStore(items);

            collection.Get(store);
            return collection;
        }

        private IDictionary<string, object> Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(data);
        }

        public async Task Update(DataCollection collection)
        {
            IDictionary<string, object> items = new Dictionary<string, object>();
            DataStore store = new DataStore(items);

            collection.Update(store);

            string data = JsonConvert.SerializeObject(items, Formatting.Indented);
            string path = Path.Combine(Root, $"{collection.Name}.json");

            using (FileStream stream = File.OpenWrite(path))
            {
                stream.SetLength(0);

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(data);
                    await writer.FlushAsync();
                }
            }
        }
    }
}