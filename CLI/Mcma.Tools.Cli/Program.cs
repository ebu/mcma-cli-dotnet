using Mcma.Management.Docker;
using Mcma.Tools;
using Mcma.Tools.Cli;
using Mcma.Tools.Dotnet;
using Mcma.Tools.ModuleRepositoryClient;
using Mcma.Tools.Modules;
using Mcma.Tools.Modules.Dotnet.Aws;
using Mcma.Tools.Modules.Dotnet.Azure;
using Mcma.Tools.Modules.Dotnet.GoogleCloud;
using Mcma.Tools.Projects;
using Microsoft.Extensions.Hosting;

var hostBuilder =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) =>
                services.AddCliExecutor()
                        .AddDotnetCli()
                        .AddModuleRepositoryClient()
                        .AddMcmaModulesTool()
                        .AddMcmaProjectsTool()
                        .AddAwsDotnetModules()
                        .AddAzureDotnetModules()
                        .AddGoogleCloudDotnetModules()
                        .AddDockerModules());

await hostBuilder.RunCommandLineApplicationAsync<McmaCli>(args);