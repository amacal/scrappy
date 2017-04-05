namespace Scrappy.Noom
{
    public class NoomPager : IPager
    {
        private readonly bool next;
        private readonly IPageable target;
        private readonly IRequest request;

        public NoomPager(bool next, IPageable target, IRequest request)
        {
            this.next = next;
            this.target = target;
            this.request = request;
        }

        public bool IsAvailable
        {
            get { return next ? target.CanNext(request) : target.CanPrev(request); }
        }

        public string Name
        {
            get { return next ? "Next" : "Prev"; }
        }

        public void Activate()
        {
            if (next)
                target.OnNext(request);
            else
                target.OnPrev(request);
        }
    }
}