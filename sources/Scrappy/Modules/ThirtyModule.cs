using System.Threading.Tasks;
using Noom;
using Scrappy.Core;
using Scrappy.Core.Thirty;
using Scrappy.Views.Thirty;

namespace Scrappy.Modules
{
    public class ThirtyModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Thirty", OnList);
            router.Register("/Thirty/{id}", OnDetails);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            object payload = collection.List();
            IViewFactory factory = new ControlView<ThirtyListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            ThirtyCollection collection = await repository.Get<ThirtyCollection>();

            object payload = collection.Details(request.Parameters["id"]);
            IViewFactory factory = new ControlView<ThirtyDetailsView>(payload);

            return factory;
        }
    }
}