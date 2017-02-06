using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Scrappy.Core
{
    public class DataStore
    {
        private readonly IDictionary<string, object> items;

        public DataStore(IDictionary<string, object> items)
        {
            this.items = items;
        }

        public void Add<T>(string key, T item)
        {
            items.Add(key, item);
        }

        public T Get<T>(string key)
            where T : class, new()
        {
            object value;

            if (items.TryGetValue(key, out value))
                return ((JToken)value).ToObject<T>();

            return new T();
        }
    }
}