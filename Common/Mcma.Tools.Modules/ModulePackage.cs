using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules
{
    public class ModulePackage
    {
        public JArray Files { get; set; }

        public FunctionInfo[] Functions { get; set; }
    }
}