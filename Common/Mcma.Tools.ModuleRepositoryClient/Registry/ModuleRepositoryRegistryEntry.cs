using System;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Registry;

public class ModuleRepositoryRegistryEntry
{
    public string Name { get; set; }
        
    public string Url { get; set; }
        
    public string AuthType { get; set; }
        
    public string AuthContext { get; set; }

    public JObject Properties { get; set; }
}