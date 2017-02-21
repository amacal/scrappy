using System;
using System.Threading.Tasks;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Noom;
using Scrappy.Views.Rutor;

namespace Scrappy.Modules
{
    public class RutorModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Rutor", OnList);
            router.RegisterAsync("/Rutor/{year}", OnListYear);
            router.RegisterAsync("/Rutor/{year}/{title}", OnDetails);
        }

        private IViewFactory OnList(IRequest request)
        {
            Func<Task<object>> result = async () =>
            {
                DataRepository repository = new DataRepository();
                RutorCollection collection = await repository.Get<RutorCollection>();

                return collection.Group();
            };

            return ControlView.PayloadAsync<RutorListView>(result);
        }

        private async Task<IViewFactory> OnListYear(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Group(request.Parameters["year"]);
            IViewFactory factory = ControlView.Payload<RutorListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            string year = request.Parameters["year"];
            string title = request.Parameters["title"];

            object payload = collection.Details(year, title);
            IViewFactory factory = ControlView.Payload<RutorDetailsView>(payload);

            return factory;
        }
    }
}