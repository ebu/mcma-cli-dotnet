using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules.Packaging;

public class ModulePackager : IModulePackager
{
    private static readonly string[] TextFileExts = { ".tf", ".json", ".yaml", ".txt", ".xml", ".config", ".html", ".md", ".ini" };

    private static void CopyFile(ModuleProviderContext moduleProviderContext, JToken additionalFile)
    {
        string src, dest;
        switch (additionalFile.Type)
        {
            case JTokenType.Object:
                src = additionalFile[nameof(src)].Value<string>();
                dest = additionalFile[nameof(dest)].Value<string>();
                break;
            case JTokenType.String:
                src = additionalFile.Value<string>();
                dest = string.Empty;
                break;
            default:
                throw new Exception("Invalid value in 'additionalFiles'. Value must be an object or a string.");
        }

        var matcher = new Matcher().AddInclude(src).AddExclude(".publish");

        var srcDir = new DirectoryInfo(moduleProviderContext.ProviderFolder);
        var srcDirWrapper = new DirectoryInfoWrapper(srcDir);
        var matches = matcher.Execute(srcDirWrapper);
            
        var destRootDir = Path.Combine(moduleProviderContext.OutputStagingFolder, dest);
        Directory.CreateDirectory(destRootDir);
            
        Console.WriteLine($"Copying files matching pattern '{src}' in {moduleProviderContext.ProviderFolder} to {destRootDir}...");

        foreach (var match in matches.Files)
        {
            var srcPath = Path.Combine(moduleProviderContext.ProviderFolder, match.Path);
            var destPath = Path.Combine(destRootDir, match.Path);
                
            var destDir = Path.GetDirectoryName(destPath);
            Directory.CreateDirectory(destDir);
            
            if (TextFileExts.Any(x => x.Equals(Path.GetExtension(srcPath), StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Replacing tokens in content in {srcPath}");
                var content = File.ReadAllText(srcPath);

                Console.WriteLine($"Writing content to {destPath}");
                File.WriteAllText(destPath, moduleProviderContext.ReplaceTokens(content));
            }
            else
            {
                Console.WriteLine($"Copying {srcPath} to {destPath}");
                File.Copy(srcPath, destPath, true);
            }
        }
    }

    public async Task PackageAsync(ModuleProviderContext moduleProviderContext, IModuleBuildSystem buildSystem)
    {
        Console.WriteLine($"Packaging module at {moduleProviderContext.ProviderFolder}...");
        try
        {   
            foreach (var function in moduleProviderContext.ModulePackage.Functions)
            {
                Console.WriteLine($"Packaging function '{function.Name}' of type '{function.Type}'");
                    
                await buildSystem.PackageFunctionAsync(moduleProviderContext, function);
                    
                Console.WriteLine($"Function '{function.Name}' packaged successfully.");
            }
                
            var files = moduleProviderContext.ModulePackage.Files;
            if (files != null)
                foreach (var file in files)
                    CopyFile(moduleProviderContext, file);

            File.WriteAllText(
                Path.Combine(moduleProviderContext.OutputStagingFolder, "module.json"),
                moduleProviderContext.GetProviderSpecificModule().ToJson());
                
            if (File.Exists(moduleProviderContext.OutputZipFile))
                File.Delete(moduleProviderContext.OutputZipFile);
        
            Console.WriteLine($"Zipping output to {moduleProviderContext.OutputZipFile}...");
            ZipFile.CreateFromDirectory(moduleProviderContext.OutputStagingFolder, moduleProviderContext.OutputZipFile);
            Console.WriteLine($"Successfully created package zip at {moduleProviderContext.OutputZipFile}");
        }
        catch
        {
            try
            {
                Directory.Delete(moduleProviderContext.OutputStagingFolder, true);
            }
            catch
            {
                // clean up isn't critical
            }
            throw;
        }
    }
}