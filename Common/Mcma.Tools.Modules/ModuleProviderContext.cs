using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Mcma.Serialization;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules;

public class ModuleProviderContext
{
    private readonly Lazy<ModulePackage> _modulePackage;
    private readonly Lazy<IReadOnlyDictionary<string, string>> _variables;

    public ModuleProviderContext(ModuleContext moduleContext, string providerFolder, Provider provider = null)
    {
        ModuleContext = moduleContext ?? throw new ArgumentNullException(nameof(moduleContext));
        ProviderFolder = providerFolder ?? throw new ArgumentNullException(nameof(providerFolder));

        Provider = provider ?? new Provider(new DirectoryInfo(providerFolder).Name);

        _modulePackage =
            new Lazy<ModulePackage>(() => JsonFileHelper.GetJsonObjectFromFile(Path.Combine(ProviderFolder, "module-package.json")).ToObject<ModulePackage>());
        _variables = new Lazy<IReadOnlyDictionary<string, string>>(() =>
        {
            var variables = ModuleContext.Variables.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            
            foreach (var packageProp in ModulePackage.Properties ?? new Dictionary<string, string>())
                variables[packageProp.Key] = packageProp.Value;

            return new ReadOnlyDictionary<string, string>(variables);
        });
    }

    public ModuleContext ModuleContext { get; }

    public string ProviderFolder { get; }
        
    public Provider Provider { get; }

    public string OutputStagingFolder => Path.Combine(ProviderFolder, ".publish", ModuleContext.Version, "staging");

    public string OutputZipFile =>
        Path.Combine(ProviderFolder, ".publish", ModuleContext.Version.ToString(), $"{ModuleContext.Version}.zip");

    public ModulePackage ModulePackage => _modulePackage.Value;

    public IReadOnlyDictionary<string, string> Variables => _variables.Value;

    public string FunctionsOutputFolder => Path.Combine(OutputStagingFolder, "functions");
        
    public FunctionInfo GetFunction(string functionName)
        => ModulePackage.Functions.FirstOrDefault(f => f.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));

    public string GetFunctionPath(string functionName) => GetFunctionPath(GetFunction(functionName));
        
    public string GetFunctionPath(FunctionInfo function) => function != null ? Path.Combine(ProviderFolder, function.Path) : null;

    public string GetFunctionOutputZipPath(string functionName) => GetFunctionOutputZipPath(GetFunction(functionName));

    public string GetFunctionOutputZipPath(FunctionInfo function) =>
        function != null ? Path.Combine(FunctionsOutputFolder, function.Name + ".zip") : null;

    public Module GetProviderSpecificModule()
    {
        var module = JObject.FromObject(ModuleContext.Module).ToMcmaObject<Module>();
        module.Provider = Provider;
        return module;
    }

    public string ReplaceTokens(string content)
        => Variables.Aggregate(content, (current, variable) => current.Replace($"@@{variable.Key}@@", variable.Value));
}