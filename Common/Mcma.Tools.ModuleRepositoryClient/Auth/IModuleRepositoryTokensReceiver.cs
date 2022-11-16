using System.Threading;
using System.Threading.Tasks;

internal interface IModuleRepositoryTokensReceiver
{
    Task<string> StartAsync(string wssEndpoint, CancellationToken cancellationToken);

    Task<ModuleRepositoryAuthTokens> WaitForTokensAsync(CancellationToken cancellationToken);
}