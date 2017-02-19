using System;

namespace Scrappy.Noom
{
    public class NoomRequest : IRequest
    {
        private readonly object payload;
        private readonly NoomParameters parameters;
        private readonly string[] parts;

        public NoomRequest(string path)
            : this(path, null)
        {
        }

        public NoomRequest(string path, object payload)
        {
            this.payload = payload;
            this.parameters = new NoomParameters();

            parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = "/" + parts[i];
            }

            if (parts.Length == 0)
            {
                parts = new[] { "/" };
            }
        }

        public NoomRequest(NoomRequest request, NoomParameters parameters)
        {
            this.parameters = parameters;
            this.parts = request.parts;
            this.payload = request.payload;
        }

        public string[] Path
        {
            get { return parts; }
        }

        public IParameters Parameters
        {
            get { return parameters; }
        }

        public dynamic Payload
        {
            get { return payload; }
        }
    }
}