namespace Scrappy.Noom
{
    public class NoomTools : IViewTools
    {
        private readonly IResolver resolver;

        public NoomTools(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public IResolver Resolver
        {
            get { return resolver; }
        }
    }
}