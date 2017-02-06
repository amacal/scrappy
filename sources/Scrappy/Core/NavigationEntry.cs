using System;

namespace Scrappy.Core
{
    public class NavigationEntry
    {
        public string Title { get; set; }
        public Action Execute { get; set; }
    }
}