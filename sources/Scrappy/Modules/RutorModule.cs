using System.Threading.Tasks;
using Noom;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Views.Rutor;

namespace Scrappy.Modules
{
    public class RutorModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Rutor", OnList);
            router.Register("/Rutor/{year}", OnListYear);
            router.Register("/Rutor/{year}/{title}", OnDetails);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Group();
            IViewFactory factory = new ControlView<RutorListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnListYear(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Group(request.Parameters["year"]);
            IViewFactory factory = new ControlView<RutorListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            string year = request.Parameters["year"];
            string title = request.Parameters["title"];

            object payload = collection.Details(year, title);
            IViewFactory factory = new ControlView<RutorDetailsView>(payload);

            return factory;
        }
    }
}