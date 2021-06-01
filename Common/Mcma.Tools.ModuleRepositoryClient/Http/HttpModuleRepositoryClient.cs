using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mcma.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Http
{
    internal class HttpModuleRepositoryClient : IModuleRepositoryClient
    {
        public HttpModuleRepositoryClient(McmaHttpClient mcmaHttpClient, HttpClient httpClient, string url)
        {
            McmaHttpClient = mcmaHttpClient ?? throw new ArgumentNullException(nameof(mcmaHttpClient));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }
        
        private McmaHttpClient McmaHttpClient { get; }

        private HttpClient HttpClient { get; }

        private string Url { get; }

        public async Task PublishAsync(JObject moduleJson, string modulePackageFilePath)
        {
            var publishResp = await McmaHttpClient.PostAsync($"{Url.TrimEnd('/')}/modules/publish", new StringContent(moduleJson.ToString(), Encoding.UTF8, "application/json"));
            if (!publishResp.IsSuccessStatusCode)
            {
                var errorMessage = $"Response status code does not indicate success: {(int)publishResp.StatusCode} ({publishResp.ReasonPhrase})";
                try
                {
                    var errorBody = await publishResp.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(errorBody))
                    {
                        try
                        {
                            errorBody = JObject.Parse(errorBody).ToString(Formatting.Indented);
                        }
                        catch
                        {
                            // body isn't json - just use it as text
                        }
                        errorMessage += Environment.NewLine + "Body:" + Environment.NewLine + errorBody;
                    }
                }
                finally
                {
                    if (publishResp.Content is IDisposable disposableContent)
                        disposableContent.Dispose();
                }
        
                throw new HttpRequestException(errorMessage);
            }
    
            var publishRespJson = JObject.Parse(await publishResp.Content.ReadAsStringAsync());
    
            var publishUrl = publishRespJson["publishUrl"]?.Value<string>();
    
            using var uploadStream = File.OpenRead(modulePackageFilePath); 
            var uploadResp = await HttpClient.PutAsync(publishUrl, new StreamContent(uploadStream));
            uploadResp.EnsureSuccessStatusCode();
        }
    }
}