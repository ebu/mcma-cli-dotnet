using Mcma.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient.Registry;

internal class ModuleRepositoryRegistry : IModuleRepositoryRegistry
{
    public ModuleRepositoryRegistry(IOptions<ModuleRepositoryRegistryOptions> options)
    {
        Options = options.Value;
        Entries = new Lazy<Dictionary<string, ModuleRepositoryRegistryEntry>>(Load);
    }

    private ModuleRepositoryRegistryOptions Options { get; }

    private string RepositoriesJsonFile => Path.Combine(Options.FolderPath, "repositories.json");

    private Lazy<Dictionary<string, ModuleRepositoryRegistryEntry>> Entries { get; }

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

    private Dictionary<string, ModuleRepositoryRegistryEntry> Load()
    {
        if (!File.Exists(RepositoriesJsonFile))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RepositoriesJsonFile)!);
            File.WriteAllText(RepositoriesJsonFile, DefaultJson.ToString(Formatting.Indented));
        }

        var json = JObject.Parse(File.ReadAllText(RepositoriesJsonFile));

        return json.Properties().ToDictionary(prop => prop.Name,
                                              prop =>
                                              {
                                                  var entry = prop.Value.ToObject<ModuleRepositoryRegistryEntry>();
                                                  if (entry == null)
                                                      throw new Exception($"Invalid module repository entry '{prop.Name}'");
                                                  entry.Name = prop.Name;
                                                  return entry;
                                              },
                                              StringComparer.OrdinalIgnoreCase);
    }

    private void Save()
        =>
            File.WriteAllText(RepositoriesJsonFile,
                              Entries.Value
                                     .Aggregate(new JObject(),
                                                (json, kvp) =>
                                                {
                                                    var entryJson = kvp.Value.ToMcmaJsonObject();
                                                    entryJson.Remove(nameof(ModuleRepositoryRegistryEntry.Name));
                                                    json[kvp.Key] = entryJson;
                                                    return json;
                                                })
                                     .ToString(Formatting.Indented));

    public bool TryAdd(ModuleRepositoryRegistryEntry entry)
    {
        if (!Entries.Value.TryAdd(entry.Name, entry))
            return false;

        Save();
        return true;
    }

    public bool TryUpdate(string name, Action<ModuleRepositoryRegistryEntry> update)
    {
        if (!Entries.Value.ContainsKey(name))
            return false;

        update(Entries.Value[name]);
        Save();
        return true;
    }

    public ModuleRepositoryRegistryEntry Get(string name)
    {
        if (!Entries.Value.ContainsKey(name))
            throw new Exception($"Repository with name '{name}' not configured.");

        return Entries.Value[name];
    }
}