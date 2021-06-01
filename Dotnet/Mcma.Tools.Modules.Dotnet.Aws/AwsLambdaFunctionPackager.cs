using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Packaging;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules.Dotnet.Aws
{
    public class AwsLambdaFunctionPackager : IFunctionPackager
    {
        public AwsLambdaFunctionPackager(IDotnetCli dotnetCli)
        {
            DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
        }
        
        private IDotnetCli DotnetCli { get; }
        
        public string Type => "AwsLambdaFunction";
        
        private async Task EnsureLambdaToolsAvailableAsync(ModuleContext moduleContext)
        {
            var toolManifestFile = Path.Combine(moduleContext.ProviderFolder, ".config", "dotnet-tools.json");
            var toolManifest = JObject.Parse(File.ReadAllText(toolManifestFile));

            var areAwsLambdaToolsInstalled = toolManifest["tools"]?["amazon.lambda.tools"] != null;
            if (!areAwsLambdaToolsInstalled)
            {
                if (!File.Exists(toolManifestFile))
                    await DotnetCli.RunCmdWithOutputAsync("new", "tool-manifest", "-o", moduleContext.ProviderFolder);
                
                Console.WriteLine("Installing dotnet CLI Lambda tools...");
                await DotnetCli.RunCmdWithOutputAsync("tool", "install", "amazon.lambda.tools", "--tool-manifest", toolManifestFile);
                Console.WriteLine("dotnet CLI Lambda tools installed successfully");
            }
            else
                Console.WriteLine("dotnet CLI Lambda tools already installed");
        }

        public async Task PackageAsync(ModuleContext moduleContext, FunctionInfo function)
        {
            await EnsureLambdaToolsAvailableAsync(moduleContext);

            var outputZipFile = moduleContext.GetFunctionOutputZipPath(function.Name);

            var initialDir = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(moduleContext.GetFunctionPath(function.Name));
        
                await DotnetCli.RunCmdWithOutputAsync("lambda", "package", Path.Combine(initialDir, outputZipFile));
            }
            finally
            {
                Directory.SetCurrentDirectory(initialDir);
            }
        }
    }
}