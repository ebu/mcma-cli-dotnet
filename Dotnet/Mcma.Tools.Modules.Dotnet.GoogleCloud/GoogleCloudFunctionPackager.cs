using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules.Packaging;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud
{
    public class GoogleCloudFunctionPackager : IFunctionPackager
    {
        public string Type => "GoogleCloudFunction";

        public Task PackageAsync(ModuleContext moduleContext, FunctionInfo functionInfo)
        {
            throw new NotImplementedException();
        }
    }
}