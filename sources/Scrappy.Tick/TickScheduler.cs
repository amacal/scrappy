using System;

namespace Scrappy.Tick
{
    public static class TickScheduler
    {
        public static void Initialize()
        {
            Initialize(new TickBootstrapper(), new TickFeedback());
        }

        public static void Initialize(IBootstrapper bootstrapper)
        {
            Initialize(bootstrapper, new TickFeedback());
        }

        public static void Initialize(IFeedback feedback)
        {
            Initialize(new TickBootstrapper(), feedback);
        }

        public static void Initialize(IBootstrapper bootstrapper, IFeedback feedback)
        {
            TickRepository repository = new TickRepository();
            TickExecutor executor = new TickExecutor(repository, feedback);

            foreach (Type type in bootstrapper.FindAllTasks())
            {
                repository.Add((ITask)Activator.CreateInstance(type));
            }

            executor.Start();
        }
    }
}