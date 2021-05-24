using System.Threading.Tasks;

namespace Mcma.Management.Modules.Packaging
{
    public interface IModulePackager
    {
        Task PackageModuleAsync(ModuleContext moduleContext);

        Task PackageAsync(Version version);
    }
}