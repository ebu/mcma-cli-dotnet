using Newtonsoft.Json.Linq;

namespace Mcma.Management.DataModel
{
    public class DeploymentAction
    {
        public DeploymentActionType Type { get; set; }
        
        public JObject Configuration { get; set; }
    }
}