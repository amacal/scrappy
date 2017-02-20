namespace Scrappy.Noom
{
    public interface IViewTools
    {
        IResolver Resolver { get; }

        ICache Cache { get; }
    }
}