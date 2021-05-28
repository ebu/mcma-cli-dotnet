using System.Threading.Tasks;

namespace Mcma.Management.Modules.Packaging
{
    public interface IFunctionPackager
    {
        string Type { get; }
        
        Task PackageAsync(ModuleContext moduleContext, FunctionInfo functionInfo);
    }
}