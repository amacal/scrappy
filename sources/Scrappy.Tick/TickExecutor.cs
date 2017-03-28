using System;
using System.Threading;

namespace Scrappy.Tick
{
    public class TickExecutor
    {
        private readonly TickRepository repository;
        private readonly IFeedback feedback;
        private readonly Timer timer;

        public TickExecutor(TickRepository repository, IFeedback feedback)
        {
            this.repository = repository;
            this.feedback = feedback;

            this.timer = new Timer(OnTick);
        }

        public void Start()
        {
            timer.Change(repository.NextInterval(), Timeout.InfiniteTimeSpan);
        }

        private async void OnTick(object state)
        {
            ITask task = repository.NextTask();
            DateTime started = DateTime.Now;

            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            feedback.OnStarted(task, started);

            try
            {
                TimeSpan next = await task?.Execute();

                feedback.OnCompleted(task, DateTime.Now - started);
                repository.Schedule(task, next);

                timer.Change(repository.NextInterval(), Timeout.InfiniteTimeSpan);
            }
            catch (Exception ex)
            {
                feedback.OnFailed(task, DateTime.Now - started, ex);
                timer.Change(repository.NextInterval(), Timeout.InfiniteTimeSpan);
            }
        }
    }
}