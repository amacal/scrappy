using System;
using System.Threading.Tasks;

namespace Scrappy.Tick
{
    public interface ITask
    {
        string Name { get; }

        TimeSpan Interval { get; }

        Task<TimeSpan> Execute();
    }
}