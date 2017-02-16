using System;
using System.Collections.Generic;

namespace Noom
{
    public interface IBootstrapper
    {
        IEnumerable<Type> FindAllModules();
    }
}