using System.Windows.Controls;

namespace Scrappy.Noom
{
    public interface IView
    {
        void Render(ContentControl destination);
    }
}