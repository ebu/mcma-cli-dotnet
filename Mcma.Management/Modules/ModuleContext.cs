using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Mcma.Management.Modules
{
    public class ModuleContext
    {
        private readonly Lazy<ModulePackage> _modulePackage;
        private readonly Lazy<JObject> _moduleJsonTemplate;

        public ModuleContext(string rootFolder, Version version)
        {
            RootFolder = rootFolder;
            Version = version;
            
            OutputStagingFolder = Path.Combine(rootFolder, ".publish", version.ToString(), "staging");
            OutputZipFile = Path.Combine(rootFolder, ".publish", version.ToString(), $"{version}.zip");

            Variables = new Dictionary<string, string>
            {
                [nameof(Version)] = Version.ToString()
            };

            _modulePackage = new Lazy<ModulePackage>(() => GetObjectFromFile<ModulePackage>("module-package.json"));
            _moduleJsonTemplate = new Lazy<JObject>(() => GetJsonFromFile("module.json"));
        }

        public string RootFolder { get; }

        public string OutputStagingFolder { get; }
        
        public string OutputZipFile { get; }

        public Version Version { get; }

        public ModulePackage ModulePackage => _modulePackage.Value;

        public string FunctionsOutputFolder => Path.Combine(OutputStagingFolder, "functions");

        public Dictionary<string, string> Variables { get; }

        private string GetTextFromFile(string fileName, bool required = true)
        {
            var path = Path.Combine(RootFolder, fileName);
            
            if (File.Exists(path))
                return File.ReadAllText(path);
            
            if (!required)
                return default;

            throw new Exception($"Unable to package module. {fileName} not found in {RootFolder}");
        }

        private JObject GetJsonFromFile(string fileName, bool required = true)
        {
            var text = GetTextFromFile(fileName, required);
            try
            {
                return JObject.Parse(text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse json from {fileName} in {RootFolder}", ex);
            }
        }

        private T GetObjectFromFile<T>(string fileName, bool required = true) where T : class
            => GetJsonFromFile(fileName, required)?.ToObject<T>();

        public string GetFunctionPath(string functionName)
        {
            var function = ModulePackage.Functions.FirstOrDefault();
            return function != null ? Path.Combine(RootFolder, function.Path) : null;
        }

        public string GetFunctionOutputZipPath(string functionName)
        {
            var function = ModulePackage.Functions.FirstOrDefault();
            return function != null ? Path.Combine(FunctionsOutputFolder, function.Name + ".zip") : null;
        }

        public string ReplaceTokens(string content)
            => Variables.Aggregate(content, (current, variable) => current.Replace($"@@{variable.Key}@@", variable.Value));

        public JObject GetModuleJson()
            => JObject.Parse(ReplaceTokens(_moduleJsonTemplate.Value.ToString()));
    }
}