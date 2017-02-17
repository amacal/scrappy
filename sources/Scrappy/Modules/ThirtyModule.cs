using System.Threading.Tasks;
using Noom;
using Scrappy.Core;
using Scrappy.Core.Rutor;
using Scrappy.Views;

namespace Scrappy.Modules
{
    public class ThirtyModule : IModule
    {
        public void Register(IRouter router)
        {
            router.Register("/Thirty", OnList);
        }

        private async Task<IViewFactory> OnList(IRequest request)
        {
            DataRepository repository = new DataRepository();
            RutorCollection collection = await repository.Get();

            object payload = collection.Group();
            IViewFactory factory = new ControlView<RutorListView>(payload);

            return factory;
        }
    }
}