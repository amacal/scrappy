using System;
using System.Collections.Generic;

namespace Tick
{
    public interface IBootstrapper
    {
        IEnumerable<Type> FindAllTasks();
    }
}