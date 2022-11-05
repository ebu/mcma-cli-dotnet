using Mcma.Management.Docker;
using Mcma.Tools;
using Mcma.Tools.Cli;
using Mcma.Tools.Cli.Parsers;
using Mcma.Tools.ModuleRepositoryClient;
using Mcma.Tools.Modules;
using Mcma.Tools.Projects;
using Microsoft.Extensions.Hosting;

var hostBuilder =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) =>
                services.AddCliExecutor()
                        .AddModuleRepositoryClient()
                        .AddDockerImageFunctionPackaging()
                        .AddDotnetModulesAndProjects()
                        .AddGradleModulesAndProjects()
                        .AddMcmaModulesTool()
                        .AddMcmaProjectsTool()
                        .AddCustomConventions());

await hostBuilder.RunCommandLineApplicationAsync<McmaCli>(args);