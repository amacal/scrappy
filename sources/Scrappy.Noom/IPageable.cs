namespace Scrappy.Noom
{
    public interface IPageable
    {
        bool CanPrev();

        void OnPrev();

        bool CanNext();

        void OnNext();
    }
}