using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules;
using Mcma.Tools.Modules.Packaging;

namespace Mcma.Management.Docker
{
    public class DockerImagePackager : IFunctionPackager
    {
        public DockerImagePackager(IDockerCli dockerCli)
        {
            DockerCli = dockerCli ?? throw new ArgumentNullException(nameof(dockerCli));
        }

        private IDockerCli DockerCli { get; }
        
        public string Type => "DockerImage";

        public async Task PackageAsync(ModuleContext moduleContext, FunctionInfo functionInfo)
        {
            if (!functionInfo.Properties.ContainsKey("dockerImageId"))
                throw new Exception($"'dockerImageId' property not found in properties for '{functionInfo.Name}' in module-package.json. This is required for packaging for Kubernetes Deployments.");

            var dockerImageId = functionInfo.Properties["dockerImageId"];
            var projectFolder = moduleContext.GetFunctionPath(functionInfo.Name);

            var dockerUserName = Environment.GetEnvironmentVariable("DOCKER_USERNAME");
            var dockerPassword = Environment.GetEnvironmentVariable("DOCKER_PASSWORD");

            await DockerCli.RunCmdAsync("login", "-u", dockerUserName, "-p", dockerPassword);
            await DockerCli.RunCmdAsync("build", projectFolder, "-t", $"{dockerImageId}:{moduleContext.Version}");
            await DockerCli.RunCmdAsync("push", $"{dockerImageId}:{moduleContext.Version}");
        }
    }
}