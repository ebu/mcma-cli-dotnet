using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules.Packaging
{
    public class ModulePackager : IModulePackager
    {
        private const string TfExt = ".tf";
        private const string JsonExt = ".json";

        public ModulePackager(IEnumerable<IFunctionPackager> functionPackagers)
        {
            FunctionPackagers = functionPackagers?.ToArray() ?? new IFunctionPackager[0];
        }
        
        private IFunctionPackager[] FunctionPackagers { get; }

        private static void CopyFile(ModuleContext moduleContext, JToken additionalFile)
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

            var srcDir = new DirectoryInfo(moduleContext.ProviderFolder);
            var srcDirWrapper = new DirectoryInfoWrapper(srcDir);
            var matches = matcher.Execute(srcDirWrapper);
            
            var destRootDir = Path.Combine(moduleContext.OutputStagingFolder, dest);
            Directory.CreateDirectory(destRootDir);
            
            Console.WriteLine($"Copying files matching pattern '{src}' in {moduleContext.ProviderFolder} to {destRootDir}...");

            foreach (var match in matches.Files)
            {
                var srcPath = Path.Combine(moduleContext.ProviderFolder, match.Path);
                var destPath = Path.Combine(destRootDir, match.Path);
                
                var destDir = Path.GetDirectoryName(destPath);
                Directory.CreateDirectory(destDir);
            
                if (TfExt.Equals(Path.GetExtension(srcPath), StringComparison.OrdinalIgnoreCase) || JsonExt.Equals(Path.GetExtension(srcPath)))
                {
                    Console.WriteLine($"Replacing tokens in content in {srcPath}");
                    var content = File.ReadAllText(srcPath);

                    Console.WriteLine($"Writing content to {destPath}");
                    File.WriteAllText(destPath, moduleContext.ReplaceTokens(content));
                }
                else
                {
                    Console.WriteLine($"Copying {srcPath} to {destPath}");
                    File.Copy(srcPath, destPath, true);
                }
            }
        }

        public async Task PackageProviderModuleAsync(ModuleContext moduleContext)
        {
            Console.WriteLine($"Packaging module at {moduleContext.ProviderFolder}...");
            try
            {   
                foreach (var function in moduleContext.ModulePackage.Functions)
                {
                    var packager = FunctionPackagers.FirstOrDefault(p => p.Type.Equals(function.Type, StringComparison.OrdinalIgnoreCase));
                    if (packager == null)
                        throw new Exception($"No packager found for type '{function.Type}'");
                    
                    Console.WriteLine($"Packaging function '{function.Name}' with packager of type '{packager.Type}'");
                    
                    if (function.Properties?.Any() ?? false)
                        foreach (var functionProp in function.Properties)
                            moduleContext.Variables[function.Name + ":" + functionProp.Key.CamelCaseToPascalCase()] = functionProp.Value;
                    
                    await packager.PackageAsync(moduleContext, function);
                    
                    Console.WriteLine($"Function '{function.Name}' packaged successfully.");
                }
                
                var files = moduleContext.ModulePackage.Files;
                if (files != null)
                    foreach (var file in files)
                        CopyFile(moduleContext, file);

                File.WriteAllText(
                    Path.Combine(moduleContext.OutputStagingFolder, "module.json"),
                    moduleContext.Module.ToJson().ToString(Formatting.Indented));
                
                if (File.Exists(moduleContext.OutputZipFile))
                    File.Delete(moduleContext.OutputZipFile);
        
                Console.WriteLine($"Zipping output to {moduleContext.OutputZipFile}...");
                ZipFile.CreateFromDirectory(moduleContext.OutputStagingFolder, moduleContext.OutputZipFile);
                Console.WriteLine($"Successfully created package zip at {moduleContext.OutputZipFile}");
            }
            catch
            {
                try
                {
                    Directory.Delete(moduleContext.OutputStagingFolder, true);
                }
                catch
                {
                    // clean up isn't critical
                }
                throw;
            }
        }

        public Task PackageProviderModuleAsync(string provider, Version version)
            => PackageProviderModuleAsync(ModuleContext.ForProviderInCurrentDirectory(provider, version));

        public async Task PackageAllProviderModulesAsync(Version version)
        {   
            var outputFolders = new List<string>();
            try
            {
                foreach (var moduleContext in ModuleContext.ForAllInCurrentDirectory(version))
                {
                    await PackageProviderModuleAsync(moduleContext);
                    
                    outputFolders.Add(moduleContext.OutputStagingFolder);
                }
                    
                File.WriteAllText("version", version.ToString());
            }
            catch
            {
                foreach (var outputFolder in outputFolders)
                {
                    try
                    {
                        Directory.Delete(outputFolder, true);
                    }
                    catch
                    {
                        // clean up isn't critical
                    }
                }
                
                throw;
            }
        }
    }
}