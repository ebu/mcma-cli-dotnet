using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Registry
{
    internal class ModuleRepositoryRegistry : IModuleRepositoryRegistry
    {
        public ModuleRepositoryRegistry(IOptions<ModuleRepositoryRegistryOptions> options)
        {
            Options = options.Value ?? new ModuleRepositoryRegistryOptions();
            RepositoriesByName = new Lazy<Dictionary<string, (string, string, string)>>(Load);
        }

        private ModuleRepositoryRegistryOptions Options { get; }

        private string RepositoriesJsonFile => Path.Combine(Options.FolderPath, "repositories.json");

        private Lazy<Dictionary<string, (string url, string authType, string authContext)>> RepositoriesByName { get; }

        private JObject DefaultJson => new()
        {
            ["default"] = new JObject
            {
                ["url"] = Options.DefaultRepositoryUrl,
                ["authType"] = Options.DefaultRepositoryAuthType,
                ["authContext"] = Options.DefaultRepositoryAuthContext
            },
            ["local"] = new JObject
            {
                ["url"] = Options.LocalRepositoryPath
            }
        };

        private Dictionary<string, (string url, string authType, string authContext)> Load()
        {
            if (!File.Exists(RepositoriesJsonFile))
                File.WriteAllText(RepositoriesJsonFile, DefaultJson.ToString(Formatting.Indented));

            var json = JObject.Parse(File.ReadAllText(RepositoriesJsonFile));

            return json.Properties().ToDictionary(x => x.Name,
                                                  x => (
                                                           x["url"]?.Value<string>(),
                                                           x["authType"]?.Value<string>(),
                                                           x["authContext"]?.Value<string>()
                                                       ),
                                                  StringComparer.OrdinalIgnoreCase);
        }
        
        public ModuleRepositoryRegistryEntry Get(string name)
        {
            if (!RepositoriesByName.Value.ContainsKey(name))
                throw new Exception($"Repository with name '{name}' not configured.");

            var (url, authType, authContext) = RepositoriesByName.Value[name];

            return new ModuleRepositoryRegistryEntry(name, url, authType, authContext);
        }
    }
}