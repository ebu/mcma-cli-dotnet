using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Packaging
{
    public interface IModulePackager
    {
        Task PackageProviderModuleAsync(ModuleContext moduleContext);
        
        Task PackageProviderModuleAsync(string provider, Version version);

        Task PackageAllProviderModulesAsync(Version version);
    }
}