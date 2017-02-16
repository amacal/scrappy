using System.Collections.Generic;

namespace Noom
{
    public class NoomParameters : IParameters
    {
        private readonly Dictionary<string, string> items;

        public NoomParameters()
        {
            items = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            items.Add(key, value);
        }

        public string this[string index]
        {
            get { return items[index]; }
        }
    }
}