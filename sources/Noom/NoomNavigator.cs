using System.Collections.Generic;

namespace Noom
{
    public class NoomNavigator : INavigator
    {
        private readonly NoomRouter router;
        private readonly IDestination destination;

        public NoomNavigator(NoomRouter router, IDestination destination)
        {
            this.router = router;
            this.destination = destination;
        }

        public async void NavigateTo(string path)
        {
            NoomRequest request = new NoomRequest(path);
            NoomRouterEntry entry = router.Match(request);
            List<ISegment> segments = new List<ISegment>();

            for (int i = 0; i < request.Path.Length; i++)
            {
                segments.Add(new NoomSegment(this, request, i));
            }

            if (request.Path[0] != "/")
            {
                segments.Insert(0, new NoomSegment(this, request, -1));
            }

            destination.Render(await entry.GetView(request));
            destination.Render(segments.ToArray());
        }

        public async void NavigateTo(string path, object payload)
        {
            NoomRequest request = new NoomRequest(path, payload);
            NoomRouterEntry entry = router.Match(request);
            List<ISegment> segments = new List<ISegment>();

            for (int i = 0; i < request.Path.Length; i++)
            {
                segments.Add(new NoomSegment(this, request, i));
            }

            if (request.Path[0] != "/")
            {
                segments.Insert(0, new NoomSegment(this, request, -1));
            }

            destination.Render(await entry.GetView(request));
            destination.Render(segments.ToArray());
        }
    }
}