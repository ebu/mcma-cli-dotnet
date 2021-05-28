using System.Collections.Generic;

namespace Mcma.Tools.Modules
{
    public class FunctionInfo
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Path { get; set; }

        public IDictionary<string, string> Properties { get; set; }
    }
}