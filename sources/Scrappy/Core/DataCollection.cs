namespace Scrappy.Core
{
    public interface DataCollection
    {
        string Name { get; }

        void Get(DataStore store);

        void Update(DataStore store);
    }
}