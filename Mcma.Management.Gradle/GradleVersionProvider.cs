using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Mcma.Management.Gradle
{
    internal class GradleVersionProvider : IGradleVersionProvider
    {
        public GradleVersionProvider(HttpClient httpClient, IHtmlParser htmlParser, IOptions<GradleVersionProviderOptions> optionsWrapper)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            HtmlParser = htmlParser;

            var options = optionsWrapper.Value ?? new GradleVersionProviderOptions();
            HttpClient.BaseAddress = new Uri(options.DownloadUrl);
            DownloadLinksXPath = options.DownloadLinksXPath;
            DownloadLinkVersionRegex = options.DownloadLinkVersionRegex;
        }

        private HttpClient HttpClient { get; }

        private IHtmlParser HtmlParser { get; }
        
        private string DownloadLinksXPath { get; }
        
        private string DownloadLinkVersionRegex { get; }

        private async Task<HttpResponseMessage> ExecuteWithRetriesAsync(HttpMethod method, string url, bool streamResponse = false)
        {
            var retryWait = TimeSpan.FromMilliseconds(250);

            HttpResponseMessage resp;
            do
            {
                resp = await HttpClient.SendAsync(new HttpRequestMessage(method, url),
                                                  streamResponse
                                                      ? HttpCompletionOption.ResponseHeadersRead
                                                      : HttpCompletionOption.ResponseContentRead);
                if (resp.IsSuccessStatusCode) continue;
                
                await Task.Delay(retryWait);
                retryWait *= 2;
            }
            while (!resp.IsSuccessStatusCode && retryWait < TimeSpan.FromMinutes(1));

            if (retryWait >= TimeSpan.FromMinutes(1))
                resp.EnsureSuccessStatusCode();

            return resp;
        }

        private async Task<string> DownloadVersionAsync(string fileName, string destinationDir, IProgress<int> progressReporter)
        {
            var downloadTo = Path.Combine(destinationDir, fileName);
            var resp = await ExecuteWithRetriesAsync(HttpMethod.Get, fileName, true);

            await using var downloadStream = await resp.Content.ReadAsStreamAsync();
            await using var writeStream = File.Create(downloadTo);
            
            progressReporter?.Report(0);

            var totalBytesRead = 0;
            var buffer = new Memory<byte>(new byte[65536]);
            
            int bytesRead;
            do
            {
                bytesRead = await downloadStream.ReadAsync(buffer);
                if (bytesRead <= 0) continue;
                
                await writeStream.WriteAsync(buffer);
                totalBytesRead += bytesRead;
                progressReporter?.Report((int)(totalBytesRead / downloadStream.Length * 100));
            }
            while (bytesRead > 0);

            return downloadTo;
        }

        public async Task<string> ResolveLatestVersionAsync()
        {
            var resp = await ExecuteWithRetriesAsync(HttpMethod.Get, "");

            var html = await resp.Content.ReadAsStringAsync();
            var links = HtmlParser.FindLinks(html, DownloadLinksXPath);

            var firstMatch = links.Select(x => Regex.Match(x, DownloadLinkVersionRegex)).First();

            return firstMatch.Groups[1].Captures[0].Value;
        }

        public async Task GetVersionAsync(string version, string destinationDir, IProgress<int> progressReporter)
        {
            var fileName = $"gradle-{version}-bin.zip";

            var downloadedZip = await DownloadVersionAsync(fileName, destinationDir, progressReporter);
            
            ZipFile.ExtractToDirectory(downloadedZip, destinationDir);
            
            File.Delete(downloadedZip);
        }
    }
}