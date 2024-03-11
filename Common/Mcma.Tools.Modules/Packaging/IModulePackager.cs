namespace Mcma.Tools.Modules.Packaging;

public interface IModulePackager
{
    Task PackageAsync(ModuleProviderContext providerContext, IModuleBuildSystem buildSystem);
}