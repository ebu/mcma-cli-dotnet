using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Mcma.Management.Modules;
using Mcma.Management.Modules.Packaging;
using Mcma.Management.Utils;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppPackager : IFunctionPackager
    {
        public AzureFunctionAppPackager(IDotnetCli dotnetCli)
        {
            DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
        }
        
        private IDotnetCli DotnetCli { get; }
        
        public string Type => "AzureFunctionApp";

        public async Task PackageAsync(ModuleContext moduleContext, FunctionInfo functionInfo)
        {
            var projectFolder = moduleContext.GetFunctionPath(functionInfo.Name);
            var publishOutput = Path.Combine(projectFolder, "staging");
            try
            {
                await DotnetCli.RunCmdWithOutputAsync("publish", projectFolder, "-o", publishOutput);
        
                Directory.CreateDirectory(moduleContext.FunctionsOutputFolder);
        
                var outputZipFile = moduleContext.GetFunctionOutputZipPath(functionInfo.Name);

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
}