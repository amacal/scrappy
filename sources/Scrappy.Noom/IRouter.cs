using System;
using System.Threading.Tasks;

namespace Scrappy.Noom
{
    public interface IRouter
    {
        void Register(string path, Func<IRequest, Task<IViewFactory>> handler);
    }
}