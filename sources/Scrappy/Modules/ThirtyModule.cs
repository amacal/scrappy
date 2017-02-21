using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Scrappy.Noom;
using Scrappy.Views.Thirty;

namespace Scrappy.Modules
{
    public class ThirtyModule : IModule
    {
        public void Register(IRouter router)
        {
            router.RegisterAsync("/Thirty", OnList);
            router.RegisterAsync("/Thirty/{id}", OnDetails);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            object payload = collection.List();
            IViewFactory factory = ControlView.Payload<ThirtyListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            object payload = collection.Details(request.Parameters["id"]);
            IViewFactory factory = ControlView.Payload<ThirtyDetailsView>(payload);

            return factory;
        }
    }
}