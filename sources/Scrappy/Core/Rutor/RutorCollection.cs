﻿using System.Collections.Generic;
using System.Linq;
using Scrappy.Core.Imdb;

namespace Scrappy.Core.Rutor
{
    public class RutorCollection : DataCollection
    {
        private Dictionary<string, RutorItem> items;
        private Dictionary<string, RutorDetails> details;
        private Dictionary<string, ImdbDetails> imdbs;

        string DataCollection.Name
        {
            get { return "rutor"; }
        }

        void DataCollection.Get(DataStore store)
        {
            items = store.Get<Dictionary<string, RutorItem>>("items");
            details = store.Get<Dictionary<string, RutorDetails>>("details");
            imdbs = store.Get<Dictionary<string, ImdbDetails>>("imdbs");
        }

        void DataCollection.Update(DataStore store)
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

        public IEnumerable<dynamic> Group(string year)
        {
            foreach (var group in items.Values.Where(x => x.Year == year).GroupBy(x => new { x.Year, x.Title }).OrderByDescending(x => x.Max(i => i.Id)))
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

        public dynamic Details(string year, string title)
        {
            string id = null;
            List<dynamic> entries = new List<dynamic>();

            foreach (RutorItem item in items.Values)
            {
                if (item.Title == title && item.Year == year)
                {
                    if (details.ContainsKey(item.Id))
                    {
                        id = item.Id;
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
            }

            return new
            {
                Group = new
                {
                    Year = year,
                    Title = title,
                    Image = imdbs[details[id].Imdb].Image,
                    Summary = imdbs[details[id].Imdb].Summary
                },
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