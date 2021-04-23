using Newtonsoft.Json.Linq;

namespace Mcma.Management.DataModel
{
    public class ProviderConfiguration
    {
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        public JObject DefaultConfig { get; set; } 
    }
}