using System.Windows.Controls;

namespace Noom
{
    public interface IView
    {
        void Render(ContentControl destination);
    }
}