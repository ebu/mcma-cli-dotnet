namespace Mcma.Tools.ModuleRepositoryClient.Auth;

internal interface IModuleRepositoryTokensReceiver
{
    Task<string> StartAsync(string wssEndpoint, CancellationToken cancellationToken);

    Task<ModuleRepositoryAuthTokens> WaitForTokensAsync(CancellationToken cancellationToken);
}