namespace Mcma.Tools.ModuleRepositoryClient.Auth;

internal interface IModuleRepositoryTokenStorage
{
    ModuleRepositoryAuthTokens? Get();

    void Set(ModuleRepositoryAuthTokens? moduleRepositoryAuthTokens);
}