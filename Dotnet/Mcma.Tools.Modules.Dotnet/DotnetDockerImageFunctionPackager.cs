using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Docker;

namespace Mcma.Tools.Modules.Dotnet;

public class DotnetDockerImageFunctionPackager : IDotnetFunctionPackager
{
    public DotnetDockerImageFunctionPackager(IDotnetCli dotnetCli, IDockerImageFunctionHelper dockerImageHelper)
    {
        DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
        DockerImageHelper = dockerImageHelper ?? throw new ArgumentNullException(nameof(dockerImageHelper));
    }

    private IDotnetCli DotnetCli { get; }

    private IDockerImageFunctionHelper DockerImageHelper { get; }
        
    public string Type => DockerImageFunctionHelper.FunctionType;

    public async Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo)
    {
        var projectFolder = moduleProviderContext.GetFunctionPath(functionInfo);
        await DotnetCli.PublishAsync(projectFolder);
        await DockerImageHelper.BuildAndPushFunctionImageAsync(moduleProviderContext, functionInfo);
    }
}