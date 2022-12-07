using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Packaging;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules.Dotnet.Aws;

public class AwsLambdaDotnetFunctionPackager : IDotnetFunctionPackager
{
    public AwsLambdaDotnetFunctionPackager(IDotnetCli dotnetCli)
    {
        DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
    }
        
    private IDotnetCli DotnetCli { get; }

    public string Type => "AwsLambdaFunction";
        
    private async Task EnsureLambdaToolsAvailableAsync(ModuleProviderContext moduleProviderContext)
    {
        var toolManifestFile = Path.Combine(moduleProviderContext.ProviderFolder, ".config", "dotnet-tools.json");
        var toolManifest = File.Exists(toolManifestFile) ? JObject.Parse(File.ReadAllText(toolManifestFile)) : null;

        var areAwsLambdaToolsInstalled = toolManifest?["tools"]?["amazon.lambda.tools"] != null;
        if (!areAwsLambdaToolsInstalled)
        {
            if (!File.Exists(toolManifestFile))
                await DotnetCli.NewAsync("tool-manifest", moduleProviderContext.ProviderFolder);
                
            Console.WriteLine("Installing dotnet CLI Lambda tools...");
            await DotnetCli.InstallToolAsync("amazon.lambda.tools", toolManifestFile);
            Console.WriteLine("dotnet CLI Lambda tools installed successfully");
        }
        else
        {
            Console.WriteLine("Restoring dotnet CLI Lambda tools...");
            await DotnetCli.RestoreToolsAsync(toolManifestFile);
            Console.WriteLine("dotnet CLI Lambda tools restored successfully");
        }
    }

    public async Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo function)
    {
        await EnsureLambdaToolsAvailableAsync(moduleProviderContext);

        var outputZipFile = moduleProviderContext.GetFunctionOutputZipPath(function);

        var initialDir = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(moduleProviderContext.GetFunctionPath(function));
        
            await DotnetCli.RunCustomAsync("lambda", "package", Path.Combine(initialDir, outputZipFile));
        }
        finally
        {
            Directory.SetCurrentDirectory(initialDir);
        }
    }
}