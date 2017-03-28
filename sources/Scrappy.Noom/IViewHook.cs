using System.Windows.Controls;

namespace Scrappy.Noom
{
    public interface IViewHook
    {
        void OnAttached(UserControl control);
    }
}