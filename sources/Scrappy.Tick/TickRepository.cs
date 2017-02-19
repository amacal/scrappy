using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrappy.Tick
{
    public class TickRepository
    {
        private readonly List<Entry> items;

        public TickRepository()
        {
            items = new List<Entry>();
        }

        public void Add(ITask task)
        {
            items.Add(new Entry
            {
                Task = task,
                Next = DateTime.Now
            });
        }

        public ITask NextTask()
        {
            DateTime now = DateTime.Now;
            Entry entry = items.OrderBy(x => x.Next).FirstOrDefault();

            if (entry?.Next < now)
            {
                entry.Next = now + entry.Task.Interval;
            }

            return entry?.Task;
        }

        public TimeSpan NextInterval()
        {
            DateTime now = DateTime.Now;
            Entry entry = items.OrderBy(x => x.Next).FirstOrDefault();

            if (entry == null)
            {
                return TimeSpan.FromMinutes(1);
            }

            if (entry.Next < now)
            {
                return TimeSpan.FromSeconds(1);
            }

            return entry.Next - now;
        }

        private class Entry
        {
            public ITask Task;
            public DateTime Next;
        }
    }
}