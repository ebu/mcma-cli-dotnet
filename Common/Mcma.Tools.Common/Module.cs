using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mcma.Tools;

public class Module
{
    public string Namespace { get; set; }
        
    public string Name { get; set; }

    public Provider Provider { get; set; }

    public Version Version { get; set; }
        
    public string DisplayName { get; set; }
        
    public string Description { get; set; }
        
    public string[] Tags { get; set; }
        
    public string Icon { get; set; }
        
    public string Website { get; set; }
        
    public string Repository { get; set; }

    public string ToJson() => JObject.FromObject(this, JsonSerializer.CreateDefault(new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    })).ToString(Formatting.Indented);
}