using System.Threading.Tasks;
using Noom;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Views;
using RutorDetailsView = Scrappy.Views.Rutor.RutorDetailsView;
using RutorListView = Scrappy.Views.Rutor.RutorListView;

namespace Scrappy.Modules
{
    public class RutorModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Rutor", OnList);
            router.Register("/Rutor/{title}", OnDetails);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Group();
            IViewFactory factory = new ControlView<RutorListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Details(request.Payload);
            IViewFactory factory = new ControlView<RutorDetailsView>(payload);

            return factory;
        }
    }
}