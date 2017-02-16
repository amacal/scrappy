using System;
using System.Threading.Tasks;
using System.Windows;
using Noom;
using Scrappy.Core;
using Scrappy.Views;

namespace Scrappy.Modules
{
    public class RutorModule : IModule
    {
        private readonly INavigator navigator;

        public RutorModule(INavigator navigator)
        {
            this.navigator = navigator;
        }

        public void Register(IRouter router)
        {
            router.Register("/", OnList);
            router.Register("/{title}", OnDetails);
        }

        private async Task<IView> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            DataCollection collection = await repository.Get();

            object payload = collection.Group();
            Func<FrameworkElement> create = () => new RutorListView(navigator);

            return new ControlView(create, payload);
        }

        private async Task<IView> OnDetails(IRequest request)
        {
            DataRepository repository = new DataRepository();
            DataCollection collection = await repository.Get();

            object payload = collection.Details(request.Payload);
            Func<FrameworkElement> create = () => new RutorDetailsView();

            return new ControlView(create, payload);
        }
    }
}