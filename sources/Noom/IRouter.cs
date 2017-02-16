using System;
using System.Threading.Tasks;

namespace Noom
{
    public interface IRouter
    {
        void Register(string path, Func<IRequest, Task<IView>> handler);
    }
}