using System;
using System.Net.Http;
using Mcma.Client.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

internal static class ModuleRepositoryAuthRegistryExtensions
{
    private static Func<IServiceProvider, ModuleRepositoryAuthenticator> GetFactory(AuthenticatorKey key)
        => svcProvider => new ModuleRepositoryAuthenticator(key,
                                                            svcProvider.GetRequiredService<IModuleRepositoryTokenStorage>(),
                                                            svcProvider.GetRequiredService<IModuleRepositoryTokensReceiver>(),
                                                            svcProvider.GetRequiredService<HttpClient>(),
                                                            svcProvider.GetRequiredService<IOptionsSnapshot<ModuleRepositoryAuthOptions>>());

    internal static AuthenticatorRegistry AddModuleRepositoryAuth(this AuthenticatorRegistry authenticatorRegistry,
                                                  Action<ModuleRepositoryAuthOptions> configureOptions = null)
    {
        var key = new AuthenticatorKey(ModuleRepositoryAuthenticator.AuthType);

        authenticatorRegistry.Services.Configure(key.ToString(), configureOptions ?? (_ => { }));

        authenticatorRegistry.Services
                             .AddSingleton<IModuleRepositoryTokenStorage, UserProfileTokenStorage>()
                             .AddSingleton<IModuleRepositoryTokensReceiver, ModuleRepositoryTokensReceiver>();

        return authenticatorRegistry.Add(key, GetFactory(key));
    }

    public static bool TryAddModuleRepositoryAuth(this AuthenticatorRegistry authenticatorRegistry, Action<ModuleRepositoryAuthOptions> configureOptions = null)
    {
        var key = new AuthenticatorKey(ModuleRepositoryAuthenticator.AuthType);

        if (!authenticatorRegistry.TryAdd(key, GetFactory(key)))
            return false;

        authenticatorRegistry.Services.Configure(key.ToString(), configureOptions ?? (_ => { }));

        return true;
    }
}