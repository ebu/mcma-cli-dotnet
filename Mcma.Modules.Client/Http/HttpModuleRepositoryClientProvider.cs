using System;
using System.Net.Http;
using Mcma.Management.Modules.Auth;

namespace Mcma.Management.Modules.Http
{
    internal class HttpModuleRepositoryClientProvider : IModuleRepositoryClientProvider
    {
        public HttpModuleRepositoryClientProvider(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        private IHttpClientFactory HttpClientFactory { get; }

        public bool IsSupportedUrl(string url) => url?.StartsWith("http", StringComparison.OrdinalIgnoreCase) ?? false;

        public IModuleRepositoryClient GetClient(string url, IModuleRepositoryAuthenticator authenticator) =>
            new HttpModuleRepositoryClient(HttpClientFactory.CreateClient(), url, authenticator);
    }
}