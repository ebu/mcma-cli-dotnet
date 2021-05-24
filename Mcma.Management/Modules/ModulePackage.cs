using Newtonsoft.Json.Linq;

namespace Mcma.Management.Modules
{
    public class ModulePackage
    {
        public JArray Files { get; set; }

        public FunctionInfo[] Functions { get; set; }
    }
}