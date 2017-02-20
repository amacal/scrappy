namespace Scrappy.Noom
{
    public class NoomTools : IViewTools
    {
        private readonly IResolver resolver;
        private readonly ICache cache;

        public NoomTools(IResolver resolver, ICache cache)
        {
            this.resolver = resolver;
            this.cache = cache;
        }

        public IResolver Resolver
        {
            get { return resolver; }
        }

        public ICache Cache
        {
            get { return cache; }
        }
    }
}