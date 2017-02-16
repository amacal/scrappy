using System.Windows.Controls;

namespace Noom
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

        public void Render(IView view)
        {
            view.Render(control);
        }
    }
}