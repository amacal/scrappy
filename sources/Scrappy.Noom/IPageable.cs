namespace Scrappy.Noom
{
    public interface IPageable
    {
        bool CanPrev(IRequest request);

        void OnPrev(IRequest request);

        bool CanNext(IRequest request);

        void OnNext(IRequest request);
    }
}