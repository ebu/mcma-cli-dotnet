using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Mcma.Client.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

internal class ModuleRepositoryAuthenticator : IAuthenticator
{
    public const string AuthType = "ModuleRepository";
    
    public ModuleRepositoryAuthenticator(AuthenticatorKey key,
                                         IModuleRepositoryTokenStorage tokenStorage,
                                         IModuleRepositoryTokensReceiver tokensReceiver,
                                         HttpClient httpClient,
                                         IOptionsSnapshot<ModuleRepositoryAuthOptions> optionsSnapshot)
    {
        TokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        TokensReceiver = tokensReceiver ?? throw new ArgumentNullException(nameof(tokensReceiver));
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        Options = optionsSnapshot?.Get(key.ToString()) ?? new ModuleRepositoryAuthOptions();
    }

    private IModuleRepositoryTokenStorage TokenStorage { get; }

    private IModuleRepositoryTokensReceiver TokensReceiver { get; }

    private HttpClient HttpClient { get; }

    private ModuleRepositoryAuthOptions Options { get; }

    private JwtSecurityTokenHandler TokenHandler { get; } = new();

    private bool IsValidToken(string token)
        => !string.IsNullOrWhiteSpace(token) &&
           TokenHandler.ReadJwtToken(token) is { } parsedToken &&
           parsedToken.ValidTo.ToUniversalTime() > DateTime.UtcNow;

    private async Task<ModuleRepositoryAuthTokens> GetTokensFromRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var resp =
            await HttpClient.PostAsync(Options.TokenUrl,
                                       new FormUrlEncodedContent(new Dictionary<string, string>
                                       {
                                           ["grant_type"] = "refresh_token",
                                           ["client_id"] = Options.ClientId,
                                           ["refresh_token"] = refreshToken
                                       }),
                                       cancellationToken);

        if (!resp.IsSuccessStatusCode)
            return null;

        try
        {
            var tokensJson = JObject.Parse(await resp.Content.ReadAsStringAsync(cancellationToken));

            return new ModuleRepositoryAuthTokens
            {
                AccessToken = tokensJson["access_token"]?.Value<string>(),
                IdToken = tokensJson["id_token"]?.Value<string>(),
                RefreshToken = refreshToken
            };
        }
        catch
        {
            return null;
        }
    }

    private async Task<ModuleRepositoryAuthTokens> GetTokensFromUserLoginAsync(CancellationToken cancellationToken)
    {
        var connectionId = await TokensReceiver.StartAsync(Options.WebSocketCallbackUrl, cancellationToken);

        Process.Start(new ProcessStartInfo($"{Options.LoginUrl}?authCallbackId={Uri.EscapeDataString(connectionId)}") { UseShellExecute = true });

        return await TokensReceiver.WaitForTokensAsync(cancellationToken);
    }

    private async Task<ModuleRepositoryAuthTokens> GetTokensAsync(ModuleRepositoryAuthTokens tokens, CancellationToken cancellationToken)
    {
        var refreshToken = tokens?.RefreshToken;

        if (!IsValidToken(refreshToken))
            return await GetTokensFromUserLoginAsync(cancellationToken);
        
        var tokensFromRefresh = await GetTokensFromRefreshTokenAsync(refreshToken, cancellationToken);
        if (tokensFromRefresh != null)
            return tokensFromRefresh;
        
        return await GetTokensFromUserLoginAsync(cancellationToken);
    }
    
    public async Task AuthenticateAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var tokens = TokenStorage.Get();

        if (!IsValidToken(tokens?.AccessToken))
        {
            tokens = await GetTokensAsync(tokens, cancellationToken);
            
            TokenStorage.Set(tokens);
        }

        request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {tokens!.AccessToken}");
    }
}