namespace Mcma.Tools.Modules.Packaging;

public interface IFunctionPackager
{
    Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo);
}