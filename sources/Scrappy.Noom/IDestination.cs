namespace Scrappy.Noom
{
    public interface IDestination
    {
        void Render(ISegment[] segments);

        void Render(IView view);
    }
}