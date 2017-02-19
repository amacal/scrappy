using System.Linq;
using System.Text;

namespace Scrappy.Noom
{
    public class NoomSegment : ISegment
    {
        private readonly INavigator navigator;
        private readonly IRequest request;
        private readonly int index;

        public NoomSegment(INavigator navigator, IRequest request, int index)
        {
            this.navigator = navigator;
            this.request = request;
            this.index = index;
        }

        public bool IsActive
        {
            get { return request.Path.Length == index + 1; }
        }

        public string Name
        {
            get
            {
                string name = request.Path.ElementAtOrDefault(index);

                if (name?.Length > 1)
                    return name.Trim('/');

                return "Home";
            }
        }

        public void Activate()
        {
            StringBuilder path = new StringBuilder();

            for (int i = 0; i <= index; i++)
                path.Append(request.Path[i]);

            if (path.Length == 0)
                path.Append("/");

            navigator.NavigateTo(path.ToString());
        }
    }
}