using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace Noom
{
    public class NoomBootstrapper : IBootstrapper
    {
        public IEnumerable<Type> FindAllModules()
        {
            foreach (Type type in GetExportedTypes())
            {
                if (typeof(IModule).IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        public IEnumerable<Type> FindAllViews()
        {
            foreach (Type type in GetExportedTypes())
            {
                if (typeof(UserControl).IsAssignableFrom(type))
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