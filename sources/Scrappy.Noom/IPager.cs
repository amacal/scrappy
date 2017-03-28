namespace Scrappy.Noom
{
    public interface IPager
    {
        bool IsAvailable { get; }

        string Name { get; }

        void Activate();
    }
}