using System.Threading.Tasks;
using Scrappy.Noom;
using Scrappy.Views.Logs;

namespace Scrappy.Modules
{
    public class LogsModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Logs", OnList);
        }

        private IViewFactory OnList(IRequest request)
        {
            return ControlView.Using<LogsView>();
        }
    }
}