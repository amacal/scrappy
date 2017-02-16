namespace Noom
{
    public class NoomRouterPath
    {
        private readonly NoomRouterSegment[] segments;

        public NoomRouterPath(string path)
        {
            segments = NoomRouterSegment.Parse(path);
        }

        public bool Accept(IRequest request)
        {
            if (request.Path.Length != segments.Length)
                return false;

            for (int i = 0; i < segments.Length; i++)
                if (segments[i].Accept(request.Path[i]) == false)
                    return false;

            return true;
        }

        public NoomParameters GetParameters(IRequest request)
        {
            NoomParameters parameters = new NoomParameters();

            for (int i = 0; i < segments.Length; i++)
            {
                segments[i].Collect(parameters, request.Path[i]);
            }

            return parameters;
        }
    }
}