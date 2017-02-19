using System;
using System.Collections.Generic;

namespace Scrappy.Noom
{
    public interface IBootstrapper
    {
        IEnumerable<Type> FindAllModules();

        IEnumerable<Type> FindAllViews();
    }
}