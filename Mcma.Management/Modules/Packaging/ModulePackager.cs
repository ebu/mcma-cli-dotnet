using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mcma.Management.Modules.Packaging
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
                    dest = additionalFile.Value<string>();
                    break;
                default:
                    throw new Exception("Invalid value in 'additionalFiles'. Value must be an object or a string.");
            }

            var srcPath = Path.Combine(moduleContext.RootFolder, src);
            var destPath = Path.Combine(moduleContext.OutputStagingFolder, dest);
            
            if (!File.Exists(srcPath))
                throw new Exception($"File not found at '{srcPath}'");
            
            Directory.CreateDirectory(Path.GetDirectoryName(destPath));

            if (TfExt.Equals(Path.GetExtension(srcPath), StringComparison.OrdinalIgnoreCase) || JsonExt.Equals(Path.GetExtension(srcPath)))
            {
                var content = File.ReadAllText(srcPath);

                File.WriteAllText(destPath, moduleContext.ReplaceTokens(content));
            }
            else
                File.Copy(srcPath, destPath, true);
        }

        public async Task PackageModuleAsync(ModuleContext moduleContext)
        {
            try
            {   
                foreach (var function in moduleContext.ModulePackage.Functions)
                {
                    var packager = FunctionPackagers.FirstOrDefault(p => p.Type.Equals(function.Type, StringComparison.OrdinalIgnoreCase));
                    if (packager == null)
                        throw new Exception($"No packager found for type '{function.Type}'");
                    
                    await packager.PackageAsync(moduleContext, function);
                }
                
                var files = moduleContext.ModulePackage.Files;
                if (files != null)
                    foreach (var file in files)
                        CopyFile(moduleContext, file);
                
                if (File.Exists(moduleContext.OutputZipFile))
                    File.Delete(moduleContext.OutputZipFile);
        
                ZipFile.CreateFromDirectory(moduleContext.OutputStagingFolder, moduleContext.OutputZipFile);
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

        public async Task PackageAsync(Version version)
        {   
            var outputFolders = new List<string>();
            try
            {
                foreach (var moduleFile in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "module.json", SearchOption.AllDirectories))
                {
                    if (moduleFile.IndexOf(".publish", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;
                        
                    var moduleFolder = Path.GetDirectoryName(moduleFile);
                    var moduleContext = new ModuleContext(moduleFolder, version);

                    await PackageModuleAsync(moduleContext);
                    
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