using System.Threading.Tasks;

namespace Scrappy.Noom
{
    public interface IDestination
    {
        void Render(ISegment[] segments);

        Task Render(IView view, IRequest request);
    }
}