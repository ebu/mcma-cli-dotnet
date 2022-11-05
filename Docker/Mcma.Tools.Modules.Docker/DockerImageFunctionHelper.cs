using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules;

namespace Mcma.Management.Docker
{
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
            var projectFolder = moduleProviderContext.GetFunctionPath(functionInfo.Name);

            var dockerUserName = Environment.GetEnvironmentVariable("DOCKER_USERNAME");
            var dockerPassword = Environment.GetEnvironmentVariable("DOCKER_PASSWORD");

            await DockerCli.RunCmdAsync("login", "-u", dockerUserName, "-p", dockerPassword);
            await DockerCli.RunCmdAsync("build", projectFolder, "-t", $"{dockerImageId}:{moduleProviderContext.ModuleContext.Version}");
            await DockerCli.RunCmdAsync("push", $"{dockerImageId}:{moduleProviderContext.ModuleContext.Version}");
        }
    }
}