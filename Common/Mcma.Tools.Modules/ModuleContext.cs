using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules
{
    public class ModuleContext
    {
        private readonly Lazy<ModulePackage> _modulePackage;
        private readonly Lazy<Module> _module;

        public ModuleContext(string providerFolder, Version version)
        {
            RootFolder = Directory.GetCurrentDirectory();
            ProviderFolder = providerFolder ?? throw new ArgumentNullException(nameof(providerFolder));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            
            OutputStagingFolder = Path.Combine(providerFolder, ".publish", version.ToString(), "staging");
            OutputZipFile = Path.Combine(providerFolder, ".publish", version.ToString(), $"{version}.zip");

            Variables = new Dictionary<string, string>
            {
                [nameof(Version)] = Version.ToString()
            };

            _modulePackage = new Lazy<ModulePackage>(() => GetJson(Path.Combine(ProviderFolder, "module-package.json")).ToObject<ModulePackage>());
            _module = new Lazy<Module>(() => GetJson(Path.Combine(RootFolder, "module.json")).ToObject<Module>());
        }

        public string RootFolder { get; }

        public string ProviderFolder { get; }

        public string OutputStagingFolder { get; }
        
        public string OutputZipFile { get; }

        public Version Version { get; }

        public ModulePackage ModulePackage => _modulePackage.Value;

        public Module Module => _module.Value;

        public string FunctionsOutputFolder => Path.Combine(OutputStagingFolder, "functions");

        public Dictionary<string, string> Variables { get; }

        private JObject GetJson(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"File not found at {path}");

            var text = File.ReadAllText(path);
            try
            {
                return JObject.Parse(ReplaceTokens(text));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse json from {path}", ex);
            }
        }

        public string GetFunctionPath(string functionName)
        {
            var function = ModulePackage.Functions.FirstOrDefault();
            return function != null ? Path.Combine(ProviderFolder, function.Path) : null;
        }

        public string GetFunctionOutputZipPath(string functionName)
        {
            var function = ModulePackage.Functions.FirstOrDefault();
            return function != null ? Path.Combine(FunctionsOutputFolder, function.Name + ".zip") : null;
        }

        public string ReplaceTokens(string content)
            => Variables.Aggregate(content, (current, variable) => current.Replace($"@@{variable.Key}@@", variable.Value));

        public static ModuleContext ForProviderInCurrentDirectory(string provider, Version version)
        {
            var moduleFilePath = Path.Combine(Directory.GetCurrentDirectory(), provider, "module-package.json");
            if (!File.Exists(moduleFilePath))
                throw new Exception($"module-package.json not found at {moduleFilePath}");
            
            return new ModuleContext(Path.GetDirectoryName(moduleFilePath), version);
        }

        public static IEnumerable<ModuleContext> ForAllInCurrentDirectory(Version version)
            => Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "module-package.json", SearchOption.AllDirectories)
                        .Select(moduleFilePath => new ModuleContext(Path.GetDirectoryName(moduleFilePath), version));
    }
}