using System.Threading.Tasks;
using Mcma.Tools.Modules.Packaging;

namespace Mcma.Tools.Modules;

public interface IModuleBuildSystem
{
    string Name { get; }
        
    bool UseForModule(ModuleContext moduleContext);

    Task InitializeAsync(NewModuleParameters parameters);

    string[] GetGitIgnorePaths(ModuleType moduleType,
                               ModuleContext moduleContext,
                               NewModuleParameters parameters);

    Task CreateProviderProjectsAsync(ModuleType moduleType,
                                     ModuleContext moduleContext,
                                     NewModuleParameters parameters,
                                     NewProviderModuleParameters providerParams,
                                     string srcFolder);
        
    Task SetMcmaVersionAsync(ModuleContext moduleContext, Version mcmaVersion);

    Task PackageFunctionAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo);
}