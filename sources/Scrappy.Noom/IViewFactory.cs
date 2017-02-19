namespace Scrappy.Noom
{
    public interface IViewFactory
    {
        IView Create(IViewTools tools);
    }
}