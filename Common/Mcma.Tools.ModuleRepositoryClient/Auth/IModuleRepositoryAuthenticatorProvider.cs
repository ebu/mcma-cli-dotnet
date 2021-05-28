namespace Mcma.Tools.ModuleRepositoryClient.Auth
{
    public interface IModuleRepositoryAuthenticatorProvider
    {
        string Type { get; }

        IModuleRepositoryAuthenticator GetAuthenticator(string authContext);
    }
}