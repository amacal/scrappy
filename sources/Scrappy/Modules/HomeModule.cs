using System.Threading.Tasks;
using Noom;
using Scrappy.Views;

namespace Scrappy.Modules
{
    public class HomeModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/", OnList);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            return new ControlView<HomeView>(new[]
            {
                new { Title = "http://arutor.org", Path = "/Rutor" },
                new { Title = "http://p30downloads.com", Path = "/Thirty" }
            });
        }
    }
}