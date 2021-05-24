using Mcma.Cli;
using Mcma.Management;
using Mcma.Management.Aws;
using Mcma.Management.Azure;
using Microsoft.Extensions.Hosting;

var hostBuilder =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) =>
                services.AddMcmaManagement()
                        .AddMcmaAwsModuleManagement()
                        .AddMcmaAzureModuleManagement());

await hostBuilder.RunCommandLineApplicationAsync<McmaCli>(args);