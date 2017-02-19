using System.Collections.Generic;

namespace Scrappy.Noom
{
    public class NoomNavigator : INavigator
    {
        private readonly NoomRouter router;
        private readonly IDestination destination;
        private readonly IViewTools tools;

        public NoomNavigator(NoomRouter router, IDestination destination, IViewTools tools)
        {
            this.router = router;
            this.destination = destination;
            this.tools = tools;
        }

        public async void NavigateTo(string path)
        {
            NoomRequest request = new NoomRequest(path);
            NoomRouterEntry entry = router.Match(request);

            ISegment[] segments = GetSegments(request);
            IViewFactory factory = await entry.GetView(request);
            IView view = factory.Create(tools);

            destination.Render(view);
            destination.Render(segments);
        }

        public async void NavigateTo(string path, object payload)
        {
            NoomRequest request = new NoomRequest(path, payload);
            NoomRouterEntry entry = router.Match(request);

            ISegment[] segments = GetSegments(request);
            IViewFactory factory = await entry.GetView(request);
            IView view = factory.Create(tools);

            destination.Render(view);
            destination.Render(segments);
        }

        private ISegment[] GetSegments(NoomRequest request)
        {
            List<ISegment> segments = new List<ISegment>();

            for (int i = 0; i < request.Path.Length; i++)
            {
                segments.Add(new NoomSegment(this, request, i));
            }

            if (request.Path[0] != "/")
            {
                segments.Insert(0, new NoomSegment(this, request, -1));
            }

            return segments.ToArray();
        }
    }
}