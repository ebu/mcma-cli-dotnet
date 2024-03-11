using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Auth;

internal class UserProfileTokenStorage : IModuleRepositoryTokenStorage
{
    private static readonly string LocalFilePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mcma", "credentials.json");

    private ModuleRepositoryAuthTokens? CurrentModuleRepositoryAuthTokens { get; set; }

    private static ModuleRepositoryAuthTokens? Load()
    {
        try
        {
            return JObject.Parse(File.ReadAllText(LocalFilePath)).ToObject<ModuleRepositoryAuthTokens>();
        }
        catch
        {
            return new ModuleRepositoryAuthTokens();
        }
    }

    private static void Save(ModuleRepositoryAuthTokens? moduleRepositoryAuthTokens)
        => File.WriteAllText(LocalFilePath, moduleRepositoryAuthTokens.HasValue ? JObject.FromObject(moduleRepositoryAuthTokens).ToString() : default);

    public ModuleRepositoryAuthTokens? Get() => CurrentModuleRepositoryAuthTokens ??= Load();

    public void Set(ModuleRepositoryAuthTokens? moduleRepositoryAuthTokens)
    {
        Save(moduleRepositoryAuthTokens);
        CurrentModuleRepositoryAuthTokens = moduleRepositoryAuthTokens;
    }
}