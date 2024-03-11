using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Registry;

public class ModuleRepositoryRegistryEntry
{
    public required string Name { get; set; }
        
    public required string Url { get; init; }
        
    public string? AuthType { get; set; }
        
    public string? AuthContext { get; set; }

    public JObject? Properties { get; init; }
}