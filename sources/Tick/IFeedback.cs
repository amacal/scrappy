using System;

namespace Tick
{
    public interface IFeedback
    {
        void OnStarted(ITask task, DateTime timestamp);

        void OnCompleted(ITask task, TimeSpan duration);

        void OnFailed(ITask task, TimeSpan duration, Exception reason);
    }
}