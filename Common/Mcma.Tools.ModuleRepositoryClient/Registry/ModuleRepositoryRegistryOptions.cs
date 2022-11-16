using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Registry;

public class ModuleRepositoryRegistryOptions
{
    public ModuleRepositoryRegistryOptions()
    {
        FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mcma");
        LocalRepositoryPath = Path.Combine(FolderPath, "local_modules");
    }
        
    public string FolderPath { get; set; }

    public string DefaultRepositoryUrl { get; set; } = "https://modules.mcma.io/api";

    public string DefaultRepositoryAuthType { get; set; } = ModuleRepositoryAuthenticator.AuthType;

    public string DefaultRepositoryAuthContext { get; set; } = JObject.FromObject(new ModuleRepositoryAuthOptions()).ToString();

    public string LocalRepositoryPath { get; set; }
}