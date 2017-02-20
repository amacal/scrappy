using System;
using System.Collections.Generic;

namespace Scrappy.Noom
{
    public class NoomCache : ICache
    {
        private readonly Dictionary<object, object> items;

        public NoomCache()
        {
            items = new Dictionary<object, object>();
        }

        public T Resolve<K, T>(K key, Func<K, T> fallback)
        {
            if (items.ContainsKey(key) == false)
            {
                items.Add(key, fallback.Invoke(key));
            }

            return (T)items[key];
        }
    }
}