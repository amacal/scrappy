﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scrappy.Tick
{
    public class TickBootstrapper : IBootstrapper
    {
        public IEnumerable<Type> FindAllTasks()
        {
            foreach (Type type in GetExportedTypes())
            {
                if (typeof(ITask).IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        public IEnumerable<Type> FindAllFactories()
        {
            foreach (Type type in GetExportedTypes())
            {
                if (typeof(IFactory).IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetExportedTypes()
        {
            return Assembly.GetEntryAssembly().GetExportedTypes();
        }
    }
}