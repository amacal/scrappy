using System;

namespace Noom
{
    public interface IResolver
    {
        T Resolve<T>() where T : class;

        object Resolve(Type type);
    }
}