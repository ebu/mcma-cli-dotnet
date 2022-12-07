using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Tools.Gradle;

namespace Mcma.Tools.Modules.Gradle;

public class GradleFunctionPackager : IGradleFunctionPackager
{
    public GradleFunctionPackager(IGradleWrapperCli gradleWrapperCli)
    {
        GradleWrapperCli = gradleWrapperCli ?? throw new ArgumentNullException(nameof(gradleWrapperCli));
    }

    private IGradleWrapperCli GradleWrapperCli { get; }
    
    public async Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo)
    {
        var providerDirRelative =
            Path.GetRelativePath(moduleProviderContext.RootFolder, moduleProviderContext.ProviderFolder);
        
        var functionDirRelative = Path.Combine(providerDirRelative, functionInfo.Path);
        
        var buildKey = functionDirRelative.Replace("/", ":");

        await GradleWrapperCli.ExecuteTaskAsync($"{buildKey}:build", "-p", moduleProviderContext.RootFolder);

        var outputZipFilePath = Path.Combine(functionDirRelative, "build/dist/function.zip"); 
        
        File.Copy(outputZipFilePath, moduleProviderContext.GetFunctionOutputZipPath(functionInfo));
    }
}