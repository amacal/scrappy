using System.Collections.Generic;
using System.Linq;

namespace Scrappy.Core.Thirty
{
    public class ThirtyCollection : DataCollection
    {
        private Dictionary<string, ThirtyItem> items;
        private Dictionary<string, ThirtyDetails> details;

        string DataCollection.Name
        {
            get { return "thirty"; }
        }

        void DataCollection.Get(DataStore store)
        {
            items = store.Get<Dictionary<string, ThirtyItem>>("items");
            details = store.Get<Dictionary<string, ThirtyDetails>>("details");
        }

        void DataCollection.Update(DataStore store)
        {
            store.Add("items", items);
            store.Add("details", details);
        }

        public IEnumerable<dynamic> List()
        {
            foreach (ThirtyItem item in items.Values.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id))
            {
                if (details.ContainsKey(item.Id))
                {
                    yield return new
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Image = details[item.Id].Image,
                        Summary = details[item.Id].Description
                    };
                }
            }
        }

        public dynamic Details(string id)
        {
            List<object> links = new List<object>();

            foreach (ThirtyLink link in details[id].Links)
            {
                links.Add(new
                {
                    Name = link.Name,
                    Path = link.Path
                });
            }

            return new
            {
                Id = id,
                Title = items[id].Title,
                Image = details[id].Image,
                Summary = details[id].Description,
                Links = links.ToArray()
            };
        }

        public IEnumerable<ThirtyItem> MissingDetails()
        {
            List<ThirtyItem> found = new List<ThirtyItem>();

            foreach (ThirtyItem data in items.Values)
            {
                if (details.ContainsKey(data.Id) == false)
                {
                    found.Add(data);
                }
            }

            return found;
        }

        public void Apply(IEnumerable<ThirtyItem> data)
        {
            foreach (ThirtyItem item in data)
            {
                items[item.Id] = item;
            }
        }

        public void Apply(ThirtyDetails data)
        {
            details[data.Id] = data;
        }
    }
}