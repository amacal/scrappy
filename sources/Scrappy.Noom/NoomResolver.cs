using System;
using TinyIoC;

namespace Scrappy.Noom
{
    public class NoomResolver : IResolver
    {
        private readonly TinyIoCContainer container;

        public NoomResolver(TinyIoCContainer container)
        {
            this.container = container;
        }

        public T Resolve<T>()
            where T : class
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}