namespace Mcma.Management.Modules.Auth
{
    public interface IModuleRepositoryAuthenticatorProvider
    {
        string Type { get; }

        IModuleRepositoryAuthenticator GetAuthenticator(string authContext);
    }
}