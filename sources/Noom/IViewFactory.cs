namespace Noom
{
    public interface IViewFactory
    {
        IView Create(IViewTools tools);
    }
}