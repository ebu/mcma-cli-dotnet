using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Management.Modules;
using Mcma.Management.Modules.Packaging;
using Mcma.Management.Utils;

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
            var (stdOut, _) = await DotnetCli.RunCmdWithoutOutputAsync("tool", "list");
    
            var areAwsLambdaToolsInstalled =
                stdOut.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                      .Any(x => x.StartsWith("amazon.lambda.tools", StringComparison.OrdinalIgnoreCase));

            if (!areAwsLambdaToolsInstalled)
            {
                var toolManifest = Path.Combine(moduleContext.ProviderFolder, ".config", "dotnet-tools.json");
                if (!File.Exists(toolManifest))
                    await DotnetCli.RunCmdWithOutputAsync("new", "tool-manifest", "-o", moduleContext.ProviderFolder);
                
                Console.WriteLine("Installing dotnet CLI Lambda tools...");
                await DotnetCli.RunCmdWithOutputAsync("tool", "install", "amazon.lambda.tools", "--tool-manifest", toolManifest);
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