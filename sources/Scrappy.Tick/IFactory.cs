using System;
using System.Collections.Generic;

namespace Scrappy.Tick
{
    public interface IFactory
    {
        TimeSpan Interval { get; }

        IEnumerable<ITask> Create();
    }
}