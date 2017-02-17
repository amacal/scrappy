using System.Collections.Generic;
using System.Linq;
using Scrappy.Core.Imdb;

namespace Scrappy.Core.Rutor
{
    public class RutorCollection
    {
        private readonly Dictionary<string, RutorItem> items;
        private readonly Dictionary<string, RutorDetails> details;
        private readonly Dictionary<string, ImdbDetails> imdbs;

        public RutorCollection(DataStore store)
        {
            items = store.Get<Dictionary<string, RutorItem>>("items");
            details = store.Get<Dictionary<string, RutorDetails>>("details");
            imdbs = store.Get<Dictionary<string, ImdbDetails>>("imdbs");
        }

        public void Visit(DataStore store)
        {
            store.Add("items", items);
            store.Add("details", details);
            store.Add("imdbs", imdbs);
        }

        public IEnumerable<dynamic> Group()
        {
            foreach (var group in items.Values.GroupBy(x => new { x.Year, x.Title }).OrderByDescending(x => x.Max(i => i.Id)))
            {
                foreach (var rItem in group)
                {
                    if (details.ContainsKey(rItem.Id))
                    {
                        RutorDetails rDetails = details[rItem.Id];

                        if (rDetails.Imdb != null && imdbs.ContainsKey(rDetails.Imdb))
                        {
                            ImdbDetails iDetails = imdbs[rDetails.Imdb];

                            if (iDetails.Image != null && iDetails.Summary != null)
                            {
                                yield return new
                                {
                                    Year = group.Key.Year,
                                    Title = group.Key.Title,
                                    Image = iDetails.Image,
                                    Summary = iDetails.Summary
                                };

                                break;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<RutorItem> MissingDetails()
        {
            List<RutorItem> found = new List<RutorItem>();

            foreach (RutorItem data in items.Values)
            {
                if (details.ContainsKey(data.Id) == false)
                {
                    found.Add(data);
                }
            }

            return found;
        }

        public IEnumerable<RutorDetails> MissingImdb()
        {
            List<RutorDetails> found = new List<RutorDetails>();

            foreach (RutorDetails data in details.Values)
            {
                if (data.Imdb != null && imdbs.ContainsKey(data.Imdb) == false)
                {
                    found.Add(data);
                }
            }

            return found;
        }

        public dynamic Details(dynamic group)
        {
            List<dynamic> entries = new List<dynamic>();

            foreach (RutorItem item in items.Values)
            {
                if (item.Title == group.Title)
                {
                    entries.Add(new
                    {
                        Id = item.Id,
                        Date = item.Date,
                        Hash = item.Hash,
                        Size = item.Size,
                        Peers = item.Peers,
                        Seeds = item.Seeds
                    });
                }
            }

            return new
            {
                Group = group,
                Entries = entries.OrderByDescending(x => x.Id).ToArray()
            };
        }

        public void Apply(IEnumerable<RutorItem> data)
        {
            foreach (RutorItem item in data)
            {
                items[item.Id] = item;
            }
        }

        public void Apply(RutorDetails data)
        {
            details[data.Id] = data;
        }

        public void Apply(ImdbDetails data)
        {
            imdbs[data.Id] = data;
        }
    }
}