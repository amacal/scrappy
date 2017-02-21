using System.Threading.Tasks;
using Scrappy.Noom;
using Scrappy.Views.Home;

namespace Scrappy.Modules
{
    public class HomeModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/", OnList);
        }

        private IViewFactory OnList(IRequest request)
        {
            return ControlView.Payload<HomeView>(new[]
            {
                new { Title = "http://arutor.org", Path = "/Rutor" },
                new { Title = "http://p30downloads.com", Path = "/Thirty" },
                new { Title = "Logs", Path = "/Logs" }
            });
        }
    }
}