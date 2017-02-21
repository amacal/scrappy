using System.Threading.Tasks;
using System.Windows.Controls;

namespace Scrappy.Noom
{
    public class NoomDestination : IDestination
    {
        private readonly ContentControl control;
        private readonly ItemsControl items;

        public NoomDestination(ContentControl control)
        {
            this.control = control;
        }

        public NoomDestination(ContentControl control, ItemsControl items)
        {
            this.control = control;
            this.items = items;
        }

        public void Render(ISegment[] segments)
        {
            if (items != null)
            {
                items.ItemsSource = segments;
            }
        }

        public Task Render(IView view)
        {
            return view.Render(control);
        }
    }
}