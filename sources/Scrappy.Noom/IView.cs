using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scrappy.Noom
{
    public interface IView
    {
        Task Render(ContentControl destination, IViewHook hook, IRequest request);
    }
}