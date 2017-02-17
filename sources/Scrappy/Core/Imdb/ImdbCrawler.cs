using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Scrappy.Core.Imdb
{
    public class ImdbCrawler
    {
        public Task<ImdbDetails> Details(string id)
        {
            Regex summary = new Regex(@"<div class=""summary_text"" itemprop=""description"">(?<text>[^<]+)<", RegexOptions.Singleline);
            Regex image = new Regex(@"<div class=""poster[^<]+<a[^<]+<img[^>]+""(?<uri>https://images-na.ssl-images-amazon.com/images/[^""]+)""", RegexOptions.Singleline);

            return Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    string uri = $"http://www.imdb.com/title/tt{id}/";
                    string output = client.DownloadString(uri);

                    Match summaryMatch = summary.Match(output);
                    Match imageMatch = image.Match(output);

                    if (summaryMatch.Success && imageMatch.Success)
                    {
                        return new ImdbDetails
                        {
                            Id = id,
                            Summary = HttpUtility.HtmlDecode(summaryMatch.Groups["text"].Value.Trim()),
                            Image = imageMatch.Groups["uri"].Value
                        };
                    }
                }

                return new ImdbDetails
                {
                    Id = id
                };
            });
        }
    }
}