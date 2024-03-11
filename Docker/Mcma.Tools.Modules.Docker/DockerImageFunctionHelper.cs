namespace Mcma.Tools.Modules.Docker;

public class DockerImageFunctionHelper : IDockerImageFunctionHelper
{
    public const string FunctionType = "DockerImage";
            
    public DockerImageFunctionHelper(IDockerCli dockerCli)
    {
        DockerCli = dockerCli ?? throw new ArgumentNullException(nameof(dockerCli));
    }

    private IDockerCli DockerCli { get; }

    public async Task BuildAndPushFunctionImageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo)
    {
        if (functionInfo.Properties == null || !functionInfo.Properties.ContainsKey("dockerImageId"))
            throw new Exception($"'dockerImageId' property not found in properties for '{functionInfo.Name}' in module-package.json. This is required for packaging for Kubernetes Deployments.");

        var dockerImageId = functionInfo.Properties["dockerImageId"];
        var projectFolder = moduleProviderContext.GetFunctionPath(functionInfo);

        var dockerUserName = Environment.GetEnvironmentVariable("DOCKER_USERNAME") ?? throw new Exception("DOCKER_USERNAME env var not set");
        var dockerPassword = Environment.GetEnvironmentVariable("DOCKER_PASSWORD") ?? throw new Exception("DOCKER_PASSWORD env var not set");

        await DockerCli.RunCmdAsync("login", "-u", dockerUserName, "-p", dockerPassword);
        await DockerCli.RunCmdAsync("build", projectFolder, "-t", $"{dockerImageId}:{moduleProviderContext.Version}");
        await DockerCli.RunCmdAsync("push", $"{dockerImageId}:{moduleProviderContext.Version}");
    }
}