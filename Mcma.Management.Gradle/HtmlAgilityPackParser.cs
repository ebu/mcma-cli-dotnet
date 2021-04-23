using System.Linq;
using HtmlAgilityPack;

namespace Mcma.Management.Gradle
{
    internal class HtmlAgilityPackParser : IHtmlParser
    {
        public string[] FindLinks(string html, string anchorTagXPath)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var anchorElements = htmlDoc.DocumentNode.SelectNodes(anchorTagXPath);

            return anchorElements.Select(a => a.Attributes["href"].Value).ToArray();
        }
    }
}