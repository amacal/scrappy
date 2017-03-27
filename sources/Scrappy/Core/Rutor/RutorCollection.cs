using System;
using System.Collections.Generic;
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
            DateTime threshold = DateTime.Now - TimeSpan.FromDays(7);

            foreach (RutorItem data in items.Values.OrderByDescending(x => x.Id))
            {
                if (details.ContainsKey(data.Id) == false)
                {
                    yield return data;
                }
            }

            foreach (RutorItem data in items.Values.OrderByDescending(x => x.Id))
            {
                if (details.ContainsKey(data.Id) && details[data.Id].Timestamp.GetValueOrDefault() < threshold)
                {
                    yield return data;
                }
            }
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
            string imdb = null;

            List<dynamic> entries = new List<dynamic>();

            foreach (RutorItem item in items.Values)
            {
                if (item.Title == title && item.Year == year)
                {
                    if (details.ContainsKey(item.Id))
                    {
                        id = item.Id;
                        imdb = imdb ?? details[id].Imdb;

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
                    Image = imdbs[imdb].Image,
                    Summary = imdbs[imdb].Summary
                },
                Entries = entries.OrderByDescending(x => x.Size.Length).ThenByDescending(x => x.Size).ToArray()
            };
        }

        public dynamic Release(string id)
        {
            string imdb = null;

            foreach (RutorItem item in items.Values)
            {
                if (item.Title == items[id].Title && item.Year == items[id].Year)
                {
                    if (details.ContainsKey(item.Id))
                    {
                        imdb = imdb ?? details[item.Id].Imdb;
                    }
                }
            }

            return new
            {
                Group = new
                {
                    Year = items[id].Year,
                    Title = items[id].Title,
                    Image = imdbs[imdb].Image,
                    Summary = imdbs[imdb].Summary
                },
                Release = new
                {
                    Id = items[id].Id,
                    Date = items[id].Date,
                    Hash = items[id].Hash,
                    Size = items[id].Size,
                    Peers = items[id].Peers,
                    Seeds = items[id].Seeds,
                    Media = details[id].Media ?? new string[0],
                    Timestamp = details[id].Timestamp?.ToString("yyyy-MM-dd") ?? String.Empty
                }
            };
        }

        public void Apply(IEnumerable<RutorItem> data)
        {
            foreach (RutorItem item in data)
            {
                items[item.Id] = item;
            }
        }

        public void Apply(string id, RutorDetails data)
        {
            if (data == RutorDetails.Removed)
            {
                details.Remove(id);
                items.Remove(id);
            }
            else
            {
                details[data.Id] = data;
            }
        }

        public void Apply(ImdbDetails data)
        {
            imdbs[data.Id] = data;
        }
    }
}