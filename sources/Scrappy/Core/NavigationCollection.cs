using System.Collections.Generic;

namespace Scrappy.Core
{
    public class NavigationCollection
    {
        private readonly List<NavigationEntry> entries;

        public NavigationCollection()
        {
            entries = new List<NavigationEntry>();
        }

        public IEnumerable<NavigationEntry> Push(NavigationEntry entry)
        {
            entries.Add(entry);

            return entries.ToArray();
        }

        public IEnumerable<NavigationEntry> Pop(NavigationEntry entry)
        {
            for (int index = entries.Count - 1; index >= 0; index--)
            {
                NavigationEntry found = entries[index];

                if (found.Equals(entry))
                {
                    break;
                }

                entries.RemoveAt(index);
                entry.Execute.Invoke();
            }

            return entries.ToArray();
        }
    }
}