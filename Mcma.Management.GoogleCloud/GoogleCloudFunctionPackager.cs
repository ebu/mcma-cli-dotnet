using System;
using System.Threading.Tasks;
using Mcma.Management.Modules;
using Mcma.Management.Modules.Packaging;

namespace Mcma.Management.GoogleCloud
{
    public class GoogleCloudFunctionPackager : IFunctionPackager
    {
        public string Type => "GoogleCloud";

        public Task PackageAsync(ModuleContext moduleContext, FunctionInfo functionInfo)
        {
            throw new NotImplementedException();
        }
    }
}