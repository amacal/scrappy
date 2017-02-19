using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrappy.Core
{
    public static class Logger
    {
        private static readonly List<LoggerEntry> entries;
        private static Action<dynamic> onEntry;

        static Logger()
        {
            entries = new List<LoggerEntry>();
        }

        public static void Info(string message)
        {
            lock (entries)
            {
                LoggerEntry entry = new LoggerEntry
                {
                    Timestamp = DateTime.Now,
                    Message = message
                };

                entries.Add(entry);
                onEntry?.Invoke(ToEntry(entry));

                if (entries.Count > 100)
                {
                    entries.RemoveAt(0);
                }
            }
        }

        private static object ToEntry(LoggerEntry entry)
        {
            return new
            {
                Timestamp = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = entry.Message
            };
        }

        public static void Subscribe(Action<IEnumerable<dynamic>> getAll, Action<dynamic> callback)
        {
            lock (entries)
            {
                onEntry = callback;
                getAll.Invoke(entries.Select(ToEntry).ToArray());
            }
        }
    }
}