using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Mcma.Management.DataModel
{
    public class DeploymentConfiguration
    {
        public string Name { get; set; }
        
        public IDictionary<string, string> Variables { get; set; }
        
        public IDictionary<string, JObject> Providers { get; set; }
    }
}