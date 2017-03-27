using System;

namespace Scrappy.Core.Rutor
{
    public class RutorDetails
    {
        public static readonly RutorDetails Removed = new RutorDetails();

        public string Id;
        public string Imdb;
        public string[] Media;
        public DateTime? Timestamp;
    }
}