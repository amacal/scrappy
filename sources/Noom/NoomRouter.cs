using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Noom
{
    public class NoomRouter : IRouter
    {
        private readonly Dictionary<NoomRouterPath, NoomRouterEntry> items;

        public NoomRouter()
        {
            items = new Dictionary<NoomRouterPath, NoomRouterEntry>();
        }

        public void Register(string path, Func<IRequest, Task<IView>> handler)
        {
            NoomRouterPath key = new NoomRouterPath(path);
            NoomRouterEntry entry = new NoomRouterEntry(key, handler);

            items.Add(key, entry);
        }

        public NoomRouterEntry Match(IRequest request)
        {
            foreach (NoomRouterPath path in items.Keys)
            {
                if (path.Accept(request))
                {
                    return items[path];
                }
            }

            return null;
        }
    }
}