using System.IO.Compression;
using Mcma.Tools.Dotnet;

namespace Mcma.Tools.Modules.Dotnet.Azure;

public class AzureFunctionAppPackager : IDotnetFunctionPackager
{
    public AzureFunctionAppPackager(IDotnetCli dotnetCli)
    {
        DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
    }
        
    private IDotnetCli DotnetCli { get; }
        
    public string Type => "AzureFunctionApp";

    public async Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo)
    {
        var projectFolder = moduleProviderContext.GetFunctionPath(functionInfo);
        var publishOutput = Path.Combine(projectFolder, "staging");
        try
        {
            await DotnetCli.PublishAsync(projectFolder, outputFolder: publishOutput);
        
            var outputZipFile = moduleProviderContext.GetFunctionOutputZipPath(functionInfo);

            if (File.Exists(outputZipFile))
                File.Delete(outputZipFile);

            ZipFile.CreateFromDirectory(publishOutput, outputZipFile);
        }
        finally
        {
            try
            {
                Directory.Delete(publishOutput, true);
            }
            catch
            {
                // nothing to do at this point...
            }
        }
    }
}