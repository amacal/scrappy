using System;
using System.Threading.Tasks;

namespace Noom
{
    public class NoomRouterEntry
    {
        private readonly NoomRouterPath path;
        private readonly Func<IRequest, Task<IView>> handler;

        public NoomRouterEntry(NoomRouterPath path, Func<IRequest, Task<IView>> handler)
        {
            this.path = path;
            this.handler = handler;
        }

        public Task<IView> GetView(NoomRequest request)
        {
            NoomParameters parameters = path.GetParameters(request);
            NoomRequest parametrized = new NoomRequest(request, parameters);

            return handler.Invoke(parametrized);
        }
    }
}