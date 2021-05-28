using Mcma.Cli;
using Mcma.Management;
using Mcma.Management.Aws;
using Mcma.Management.Azure;
using Mcma.Management.Docker;
using Microsoft.Extensions.Hosting;

var hostBuilder =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) =>
                services.AddMcmaManagement()
                        .AddMcmaAwsModuleManagement()
                        .AddMcmaAzureModuleManagement()
                        .AddMcmaDockerImagePackaging());

await hostBuilder.RunCommandLineApplicationAsync<McmaCli>(args);