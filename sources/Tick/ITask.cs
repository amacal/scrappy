using System;
using System.Threading.Tasks;

namespace Tick
{
    public interface ITask
    {
        TimeSpan Interval { get; }

        Task Execute();
    }
}