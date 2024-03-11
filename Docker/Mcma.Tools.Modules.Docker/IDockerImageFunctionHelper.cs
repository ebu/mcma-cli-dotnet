namespace Mcma.Tools.Modules.Docker;

public interface IDockerImageFunctionHelper
{
    Task BuildAndPushFunctionImageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo);
}