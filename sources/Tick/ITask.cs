using System;
using System.Threading.Tasks;

namespace Tick
{
    public interface ITask
    {
        string Name { get; }

        TimeSpan Interval { get; }

        Task Execute();
    }
}