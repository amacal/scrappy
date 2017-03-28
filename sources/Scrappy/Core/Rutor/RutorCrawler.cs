using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Scrappy.Core.Rutor
{
    public class RutorCrawler
    {
        public Task<RutorItem[]> List(string query)
        {
            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;

                    string uri = $"http://arutor.org/search/0/0/0/0/{query}";
                    string output = client.DownloadString(uri);

                    return ParseList(output).ToArray();
                }
            });
        }

        private static IEnumerable<RutorItem> ParseList(string data)
        {
            Regex row = new Regex(@"<tr class=""(tum|gai)"">[^<]*<td>(?<date>[^<]+)</td>.*?btih:(?<hash>[0-9a-f]{40}).*?<a href=""/torrent/(?<id>[0-9]{6})/[^""]+"">(?<name>[^<]+)<.*?>(?<size>[0-9]{1,3}(\.[0-9]{1,2})?&nbsp;GB)<.*?<span class=""green"">(?<seeds>[0-9]+).*?<span class=""red"">(?<peers>[0-9]+).*?</tr>", RegexOptions.Singleline);
            Regex title = new Regex(@"(?<name>.*?) \((?<year>(19|20)[0-9]{2})\)");

            foreach (Match rMatch in row.Matches(data))
            {
                string[] parts = rMatch.Groups["name"].Value.Split('/');
                Match tMatch = title.Match(parts.Last());

                if (tMatch.Success && parts.Length > 1)
                {
                    string tit = tMatch.Groups["name"].Value.Trim();
                    string size = HttpUtility.HtmlDecode(rMatch.Groups["size"].Value);
                    string date = rMatch.Groups["date"].Value;

                    yield return new RutorItem
                    {
                        Id = rMatch.Groups["id"].Value,
                        Date = NormalizeDate(date),
                        Hash = rMatch.Groups["hash"].Value,
                        Title = HttpUtility.HtmlDecode(tit),
                        Year = tMatch.Groups["year"].Value,
                        Size = NormalizeSize(size),
                        Seeds = rMatch.Groups["seeds"].Value,
                        Peers = rMatch.Groups["peers"].Value
                    };
                }
            }
        }

        private static string NormalizeDate(string data)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("ru-ru");
            DateTime value = DateTime.Parse(data, culture);

            return value.ToString("yyyy-MM-dd");
        }

        private static string NormalizeSize(string data)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            decimal value = Decimal.Parse(data.Substring(0, data.Length - 3), culture);

            return value.ToString("F2") + " GB";
        }

        public Task<RutorDetails> Details(string id)
        {
            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;

                    try
                    {
                        string uri = $"http://arutor.org/torrent/{id}";
                        string output = client.DownloadString(uri);

                        return ParseDetails(id, output);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = ex.Response as HttpWebResponse;

                        if (response?.StatusCode == HttpStatusCode.NotFound)
                            return RutorDetails.Removed;

                        throw;
                    }
                }
            });
        }

        private RutorDetails ParseDetails(string id, string data)
        {
            Regex imdb = new Regex("http://s.rutor.info/imdb/pic/(?<id>[0-9]{7,8}).gif");
            Regex video = new Regex("Видео(:|\\s|</b>)+(?<data>[^<]{4,})<", RegexOptions.Singleline);
            Regex audio = new Regex("(Аудио|Звук)((#|№)?[0-9]|\\s)*(:|\\s|</b>)+(?<data>[^<\\|]{4,})(<|\\|)", RegexOptions.Singleline);

            Match imdbMatch = imdb.Match(data);
            Match videoMatch = video.Match(data);
            MatchCollection audioMatch = audio.Matches(data);

            List<string> media = new List<string>();
            Func<string, string> replace = x =>
            {
                x = x.Replace("Кбит/с", "Kbps");
                x = x.Replace("кадр/с", "fps");
                x = Regex.Replace(x, "\\p{IsCyrillic}", "");
                x = x.Trim(' ', ':', ',');

                return x;
            };

            if (videoMatch.Success)
            {
                media.Add(replace(videoMatch.Groups["data"].Value));
            }

            foreach (Match match in audioMatch)
            {
                media.Add(replace(match.Groups["data"].Value));
            }

            if (imdbMatch.Success)
            {
                media.RemoveAll(String.IsNullOrWhiteSpace);
                media.RemoveAll(x => x.Length <= 3);

                return new RutorDetails
                {
                    Id = id,
                    Imdb = imdbMatch.Groups["id"].Value,
                    Media = media.ToArray(),
                    Timestamp = DateTime.Now
                };
            }

            return new RutorDetails
            {
                Id = id,
                Media = media.ToArray(),
                Timestamp = DateTime.Now
            };
        }
    }
}