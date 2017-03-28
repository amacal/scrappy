using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scrappy.Noom
{
    public class NoomDestination : IDestination, IViewHook
    {
        private readonly ContentControl target;
        private readonly ItemsControl items;
        private readonly ItemsControl paging;

        public NoomDestination(ContentControl target)
        {
            this.target = target;
        }

        public NoomDestination(ContentControl target, ItemsControl items, ItemsControl paging)
        {
            this.target = target;
            this.items = items;
            this.paging = paging;
        }

        public void Render(ISegment[] segments)
        {
            if (items != null)
            {
                items.ItemsSource = segments;
            }
        }

        public void Render(IPager[] pagers)
        {
            if (paging != null)
            {
                paging.ItemsSource = pagers;
            }
        }

        public Task Render(IView view)
        {
            return view.Render(target, this);
        }

        public void OnAttached(UserControl control)
        {
            IPageable destination = control as IPageable;
            List<IPager> pagers = new List<IPager>();

            if (destination != null)
            {
                pagers.Add(new NoomPager(false, destination));
                pagers.Add(new NoomPager(true, destination));
            }

            paging.ItemsSource = pagers.ToArray();
        }
    }
}