namespace Mcma.Management.Gradle
{
    internal interface IHtmlParser
    {
        string[] FindLinks(string html, string anchorTagXPath);
    }
}