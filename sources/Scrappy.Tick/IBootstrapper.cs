using System;
using System.Collections.Generic;

namespace Scrappy.Tick
{
    public interface IBootstrapper
    {
        IEnumerable<Type> FindAllTasks();
    }
}