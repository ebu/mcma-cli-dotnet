using System;

namespace Mcma.Tools.ModuleRepositoryClient.Registry
{
    public class ModuleRepositoryRegistryEntry
    {
        public ModuleRepositoryRegistryEntry(string name, string url, string authType = null, string authContext = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            AuthType = authType;
        }
        
        public string Name { get; }
        
        public string Url { get; }
        
        public string AuthType { get; }
        
        public string AuthContext { get; }
    }
}