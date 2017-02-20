using System;

namespace Scrappy.Noom
{
    public interface ICache
    {
        T Resolve<K, T>(K key, Func<K, T> fallback);
    }
}