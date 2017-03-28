using System;

namespace Scrappy.Noom
{
    public class NoomPager : IPager
    {
        private readonly bool next;
        private readonly IPageable target;

        public NoomPager(bool next, IPageable target)
        {
            this.next = next;
            this.target = target;
        }

        public bool IsAvailable
        {
            get { return next ? target.CanNext() : target.CanPrev(); }
        }

        public string Name
        {
            get { return next ? "Next" : "Prev"; }
        }

        public void Activate()
        {
            if (next)
                target.OnNext();
            else
                target.OnPrev();
        }
    }
}