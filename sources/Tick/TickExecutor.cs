using System.Threading;

namespace Tick
{
    public class TickExecutor
    {
        private readonly TickRepository repository;
        private readonly Timer timer;

        public TickExecutor(TickRepository repository)
        {
            this.repository = repository;
            this.timer = new Timer(OnTick);
        }

        public void Start()
        {
            timer.Change(repository.NextInterval(), Timeout.InfiniteTimeSpan);
        }

        private async void OnTick(object state)
        {
            ITask task = repository.NextTask();
            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            await task?.Execute();
            timer.Change(repository.NextInterval(), Timeout.InfiniteTimeSpan);
        }
    }
}