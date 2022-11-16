using System;
using System.IO;
using Newtonsoft.Json.Linq;

internal class UserProfileTokenStorage : IModuleRepositoryTokenStorage
{
    private static readonly string LocalFilePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mcma", "credentials.json");

    private ModuleRepositoryAuthTokens CurrentModuleRepositoryAuthTokens { get; set; }

    private static ModuleRepositoryAuthTokens Load()
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

    private static void Save(ModuleRepositoryAuthTokens moduleRepositoryAuthTokens)
        => File.WriteAllText(LocalFilePath, JObject.FromObject(moduleRepositoryAuthTokens).ToString());

    public ModuleRepositoryAuthTokens Get() => CurrentModuleRepositoryAuthTokens ??= Load();

    public void Set(ModuleRepositoryAuthTokens moduleRepositoryAuthTokens)
    {
        Save(moduleRepositoryAuthTokens);
        CurrentModuleRepositoryAuthTokens = moduleRepositoryAuthTokens;
    }
}