using System;

namespace Tick
{
    public static class TickScheduler
    {
        public static void Initialize()
        {
            Initialize(new TickBootstrapper());
        }

        public static void Initialize(IBootstrapper bootstrapper)
        {
            TickRepository repository = new TickRepository();
            TickExecutor executor = new TickExecutor(repository);

            foreach (Type type in bootstrapper.FindAllTasks())
            {
                repository.Add((ITask)Activator.CreateInstance(type));
            }

            executor.Start();
        }
    }
}