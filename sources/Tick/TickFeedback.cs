using System;

namespace Tick
{
    public class TickFeedback : IFeedback
    {
        public virtual void OnStarted(ITask task, DateTime timestamp)
        {
        }

        public virtual void OnCompleted(ITask task, TimeSpan duration)
        {
        }

        public virtual void OnFailed(ITask task, TimeSpan duration, Exception reason)
        {
        }
    }
}