using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mcma.Tools.ModuleRepositoryClient.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Http
{
    internal class HttpModuleRepositoryClient : IModuleRepositoryClient
    {
        public HttpModuleRepositoryClient(HttpClient httpClient, string url, IModuleRepositoryAuthenticator authenticator)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Authenticator = authenticator;
        }
        
        private HttpClient HttpClient { get; }

        private string Url { get; }

        private IModuleRepositoryAuthenticator Authenticator { get; }

        public async Task PublishAsync(JObject moduleJson, string modulePackageFilePath)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Url}modules/publish")
            {
                Content = new StringContent(moduleJson.ToString(), Encoding.UTF8, "application/json")
            };
    
            if (Authenticator != null)
                await Authenticator.AddAuthenticationAsync(request);
    
            var publishResp = await HttpClient.SendAsync(request);
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