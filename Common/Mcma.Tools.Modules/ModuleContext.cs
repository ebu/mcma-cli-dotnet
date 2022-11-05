using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Mcma.Tools.Modules;

public class ModuleContext
{
    private readonly Lazy<Module> _module;
    private readonly Lazy<ReadOnlyDictionary<string, string>> _variables;

    public ModuleContext(string rootFolder, params (string, string)[] variables)
    {
        RootFolder = rootFolder ?? throw new ArgumentNullException();
        ModuleJsonFilePath = Path.Combine(RootFolder, "module.json");

        _module = new Lazy<Module>(() => JsonFileHelper.GetJsonObjectFromFile(ModuleJsonFilePath).ToObject<Module>());
        _variables = new Lazy<ReadOnlyDictionary<string, string>>(
            () =>
                new ReadOnlyDictionary<string, string>(
                    variables.Select(x => new KeyValuePair<string, string>(x.Item1, x.Item2))
                             .Concat(new[] { new KeyValuePair<string, string>(nameof(Version), Version) })
                             .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase)));
    }
        
    public string RootFolder { get; }
        
    public string ModuleJsonFilePath { get; }

    public Module Module => _module.Value;

    public IReadOnlyDictionary<string, string> Variables => _variables.Value;

    public Version Version
    {
        get => Module.Version;
        set => Module.Version = value;
    }

    public IEnumerable<string> GetProviderFolders()
        => Directory.EnumerateFiles(RootFolder, "module-package.json", SearchOption.AllDirectories);
        
    public ModuleProviderContext GetProvider(string provider)
    {
        var moduleFilePath = Path.Combine(RootFolder, provider, "module-package.json");
        if (!File.Exists(moduleFilePath))
            throw new Exception($"module-package.json not found at {moduleFilePath}");
            
        return new ModuleProviderContext(this, Path.GetDirectoryName(moduleFilePath));
    }

    public IEnumerable<ModuleProviderContext> GetProviders()
        => GetProviderFolders().Select(moduleFilePath => new ModuleProviderContext(this, Path.GetDirectoryName(moduleFilePath)));
    
    public void Save()
        => File.WriteAllText(ModuleJsonFilePath, Module.ToJson());
}