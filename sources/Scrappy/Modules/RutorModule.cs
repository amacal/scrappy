using System;
using System.Linq;
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
            router.Register("/Rutor", OnMoviesAll);
            router.RegisterAsync("/Rutor/{year}", OnMoviesByYear);
            router.RegisterAsync("/Rutor/{year}/{title}", OnMovieByTitle);
            router.RegisterAsync("/Rutor/{year}/{title}/{id}", OnReleaseById);
        }

        private IViewFactory OnMoviesAll(IRequest request)
        {
            Func<Task<object>> result = async () =>
            {
                DataRepository repository = new DataRepository();
                RutorCollection collection = await repository.Get<RutorCollection>();

                int? page = request.Payload;

                return collection.Group(page.GetValueOrDefault(0)).ToArray();
            };

            return ControlView.PayloadAsync<RutorMovieListView>(result);
        }

        private async Task<IViewFactory> OnMoviesByYear(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            object payload = collection.Group(request.Parameters["year"]);
            IViewFactory factory = ControlView.Payload<RutorMovieListView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnMovieByTitle(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            string year = request.Parameters["year"];
            string title = request.Parameters["title"];

            object payload = collection.Details(year, title);
            IViewFactory factory = ControlView.Payload<RutorMovieDetailsView>(payload);

            return factory;
        }

        private async Task<IViewFactory> OnReleaseById(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get<RutorCollection>();

            string id = request.Parameters["id"];
            object payload = collection.Release(id);
            IViewFactory factory = ControlView.Payload<RutorReleaseView>(payload);

            return factory;
        }
    }
}