﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Scrappy.Core.Thirty
{
    public class ThirtyCrawler
    {
        public Task<ThirtyItem[]> List(int page)
        {
            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;

                    string uri = $"http://p30download.com/fa/tutorial/page/{page}";
                    string output = client.DownloadString(uri);

                    return ParseList(output).ToArray();
                }
            });
        }

        private static IEnumerable<ThirtyItem> ParseList(string data)
        {
            Regex articleRegex = new Regex(@"<article.*?</article>", RegexOptions.Singleline);
            Regex row = new Regex(@"<h1>\s*<a href=""http://p30download.com/fa/entry/(?<id>[0-9]+)/"" title=""(?<title>[^\""]+)"".*?<time pubdate=""pubdate"" datetime=""(?<date>.{10})", RegexOptions.Singleline);
            Regex latin = new Regex("[^a-zA-Z0-9 \\-\\:]");
            Regex spaces = new Regex("( ){2,}");

            foreach (Match articleMatch in articleRegex.Matches(data))
            {
                Match match = row.Match(articleMatch.Value);

                if (match.Success)
                {
                    string id = match.Groups["id"].Value;
                    string title = spaces.Replace(latin.Replace(match.Groups["title"].Value, ""), " ");
                    string date = match.Groups["date"].Value;

                    int index = title.LastIndexOf('-');
                    if (index > 0)
                    {
                        title = title.Substring(0, index);
                    }

                    title = title.Trim(' ', '-');
                    if (title.Length > 10)
                    {
                        yield return new ThirtyItem
                        {
                            Id = id,
                            Title = title,
                            Date = date
                        };
                    }
                }
            }
        }

        public Task<ThirtyDetails> Details(string id)
        {
            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    string uri = $"http://p30download.com/fa/entry/{id}/";
                    string output = client.DownloadString(uri);

                    return ParseDetails(id, output);
                }
            });
        }

        private ThirtyDetails ParseDetails(string id, string data)
        {
            Regex imageRegex = new Regex(@"<img itemprop=""primaryImageOfPage"" src=""(?<link>[^""]+)""", RegexOptions.Singleline);
            Regex descriptionRegex = new Regex(@"<div class=""excerpt"">\s*<p>(?<value>.+?)</p>", RegexOptions.Singleline);
            Regex linksRegex = new Regex(@"href=""(?<link>http://cdn.p30download.com/[^&]+&f=(?<file>[^""]+))""", RegexOptions.Singleline);

            Match imageMatch = imageRegex.Match(data);
            Match descriptionMatch = descriptionRegex.Match(data);
            MatchCollection linksMatches = linksRegex.Matches(data);

            if (imageMatch.Success && descriptionMatch.Success && linksMatches.Count > 0)
            {
                return new ThirtyDetails
                {
                    Id = id,
                    Description = HttpUtility.HtmlDecode(descriptionMatch.Groups["value"].Value),
                    Image = imageMatch.Groups["link"].Value,
                    Links = linksMatches.OfType<Match>().Select(ToLink).ToArray()
                };
            }

            return new ThirtyDetails
            {
                Id = id
            };
        }

        private static ThirtyLink ToLink(Match match)
        {
            return new ThirtyLink
            {
                Name = match.Groups["file"].Value.Replace("_p30download.com", ""),
                Path = match.Groups["link"].Value
            };
        }
    }
}