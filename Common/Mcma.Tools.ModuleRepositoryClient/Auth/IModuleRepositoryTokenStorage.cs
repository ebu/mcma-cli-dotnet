internal interface IModuleRepositoryTokenStorage
{
    ModuleRepositoryAuthTokens Get();

    void Set(ModuleRepositoryAuthTokens moduleRepositoryAuthTokens);
}