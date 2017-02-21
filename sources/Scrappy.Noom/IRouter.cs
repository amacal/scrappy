using System;
using System.Threading.Tasks;

namespace Scrappy.Noom
{
    public interface IRouter
    {
        void Register(string path, Func<IRequest, IViewFactory> handler);

        void RegisterAsync(string path, Func<IRequest, Task<IViewFactory>> handler);
    }
}