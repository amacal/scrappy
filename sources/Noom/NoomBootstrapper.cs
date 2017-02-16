using System;
using System.Collections.Generic;
using System.Reflection;

namespace Noom
{
    public class NoomBootstrapper : IBootstrapper
    {
        public IEnumerable<Type> FindAllModules()
        {
            foreach (Type type in Assembly.GetEntryAssembly().GetExportedTypes())
            {
                if (typeof(IModule).IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }
    }
}