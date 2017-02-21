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

        public void Add(IFactory factory)
        {
            Family family = new Family
            {
                Next = DateTime.Now,
                Interval = factory.Interval
            };

            foreach (ITask task in factory.Create())
            {
                items.Add(new Entry
                {
                    Task = task,
                    Next = family.Next,
                    Family = family
                });
            }
        }

        public ITask NextTask()
        {
            DateTime now = DateTime.Now;
            Target target = items.Select(ToEffective).OrderBy(x => x.Next).FirstOrDefault();

            if (target?.Next < now)
            {
                target.Entry.Next = now + target.Entry.Task.Interval;

                if (target.Entry.Family != null)
                {
                    target.Entry.Family.Next = now + target.Entry.Family.Interval;
                }
            }

            return target?.Entry.Task;
        }

        public TimeSpan NextInterval()
        {
            DateTime now = DateTime.Now;
            Target target = items.Select(ToEffective).OrderBy(x => x.Next).FirstOrDefault();

            if (target == null)
            {
                return TimeSpan.FromMinutes(1);
            }

            if (target.Next < now)
            {
                return TimeSpan.FromSeconds(1);
            }

            return target.Next - now;
        }

        private static Target ToEffective(Entry entry)
        {
            if (entry.Family == null)
            {
                return new Target
                {
                    Entry = entry,
                    Next = entry.Next
                };
            }

            return new Target
            {
                Entry = entry,
                Next = entry.Next > entry.Family.Next ? entry.Next : entry.Family.Next
            };
        }

        private class Entry
        {
            public ITask Task;
            public DateTime Next;
            public Family Family;
        }

        private class Family
        {
            public DateTime Next;
            public TimeSpan Interval;
        }

        private class Target
        {
            public Entry Entry;
            public DateTime Next;
        }
    }
}