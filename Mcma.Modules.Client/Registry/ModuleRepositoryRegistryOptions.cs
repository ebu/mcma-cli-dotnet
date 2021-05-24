using System;
using System.IO;

namespace Mcma.Management.Modules.Registry
{
    public class ModuleRepositoryRegistryOptions
    {
        public ModuleRepositoryRegistryOptions()
        {
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mcma");
            LocalRepositoryPath = Path.Combine(FolderPath, "local_modules");
        }
        
        public string FolderPath { get; set; }

        public string DefaultRepositoryUrl { get; set; } = "https://modules.mcma.io/api";

        public string DefaultRepositoryAuthType { get; set; } = "AWS";

        public string DefaultRepositoryAuthContext { get; set; } = "{ 'profile': 'default' }";

        public string LocalRepositoryPath { get; set; }
    }
}