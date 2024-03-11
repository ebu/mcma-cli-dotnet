using Mcma.Tools.Git;
using Mcma.Tools.ModuleRepositoryClient;
using Mcma.Tools.Modules.Packaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.Modules;

internal class McmaModulesTool : IMcmaModulesTool
{
    public McmaModulesTool(IModulePackager packager,
                           IModuleRepositoryClientManager repositoryClientManager,
                           IGitCli git,
                           IEnumerable<IModuleBuildSystem>? buildSystems,
                           IEnumerable<IModuleTerraformScriptProvider>? terraformScriptProviders) 
    {
        Packager = packager ?? throw new ArgumentNullException(nameof(packager));
        RepositoryClientManager = repositoryClientManager ?? throw new ArgumentNullException(nameof(repositoryClientManager));
        Git = git ?? throw new ArgumentNullException(nameof(git));
        BuildSystems = buildSystems?.ToArray() ?? [];
        TerraformScriptProviders = terraformScriptProviders?.ToArray() ?? [];
    }

    private IModulePackager Packager { get; }

    private IModuleBuildSystem[] BuildSystems { get; }

    private IModuleRepositoryClientManager RepositoryClientManager { get; }

    private IGitCli Git { get; }

    private IEnumerable<IModuleTerraformScriptProvider> TerraformScriptProviders { get; }

    private IModuleBuildSystem GetBuildSystem(ModuleContext moduleContext)
    {
        var buildSystem = BuildSystems.FirstOrDefault(bs => bs.UseForModule(moduleContext));
        if (buildSystem == null)
            throw new Exception("Supported build system for module not found.");
        return buildSystem;
    }
        
    public async Task NewAsync(string rootFolder, string buildSystem, ModuleType moduleType, NewModuleParameters parameters)
    {
        var moduleBuildSystem = BuildSystems.FirstOrDefault(bs => bs.Name.Equals(buildSystem, StringComparison.OrdinalIgnoreCase));
        if (moduleBuildSystem == null)
            throw new Exception($"Invalid build system '{buildSystem}'.");

        var moduleContext = new ModuleContext(rootFolder);
            
        Directory.CreateDirectory(parameters.ModuleDirectory);

        await moduleBuildSystem.InitializeAsync(parameters);

        foreach (var providerParams in parameters.Providers)
        {
            var tfScriptProvider = TerraformScriptProviders.FirstOrDefault(x => x.ModuleType == moduleType && x.Provider == providerParams.Provider);
            if (tfScriptProvider == null)
                throw new Exception($"Provider '{providerParams.Provider}' is not supported.");

            var module = new Module
            {
                Namespace = parameters.NamespaceInPascalCase,
                Name = parameters.NameInKebabCase,
                Version = Version.Initial(),
                DisplayName = parameters.DisplayName,
                Description = parameters.Description,
                Provider = providerParams.Provider
            };

            var providerFolder = parameters.GetProviderDir(providerParams.Provider);
            Directory.CreateDirectory(providerFolder);
                
            var moduleJsonPath = Path.Combine(providerFolder, "module.json");
            var moduleJson = JObject.FromObject(module).ToString(Formatting.Indented);
            await File.WriteAllTextAsync(moduleJsonPath, moduleJson);

            var modulePackageJsonPath = Path.Combine(providerFolder, "module-package.json");
            var modulePackageJson = JObject.FromObject(new { functions = new { } }).ToString(Formatting.Indented);
            await File.WriteAllTextAsync(modulePackageJsonPath, modulePackageJson);
                
            var moduleTfPath = Path.Combine(providerFolder, "module.tf");
            var moduleTf = tfScriptProvider.GetModuleTf(providerParams.Args);
            await File.WriteAllTextAsync(moduleTfPath, moduleTf);
                
            var variablesTfPath = Path.Combine(providerFolder, "variables.tf");
            var variablesTf = tfScriptProvider.GetVariablesTf(providerParams.Args);
            await File.WriteAllTextAsync(variablesTfPath, variablesTf);
                
            var outputsTfPath = Path.Combine(providerFolder, "outputs.tf");
            var outputsTf = tfScriptProvider.GetOutputsTf(providerParams.Args);
            await File.WriteAllTextAsync(outputsTfPath, outputsTf);

            var srcFolder = parameters.GetProviderSrcDir(providerParams.Provider);
            Directory.CreateDirectory(srcFolder);
                
            await moduleBuildSystem.CreateProviderProjectsAsync(moduleType, moduleContext, parameters, providerParams, srcFolder);
        }

        await Git.InitAsync(parameters.ModuleDirectory);
        await Git.AddAsync(parameters.ModuleDirectory);
    }

    public async Task PackageProviderAsync(string rootFolder, Provider provider)
    {
        var moduleContext = new ModuleContext(rootFolder);
            
        var providerContext = moduleContext.GetProvider(provider);
            
        await Packager.PackageAsync(providerContext, GetBuildSystem(moduleContext));
    }

    public async Task PackageAsync(string rootFolder, Version? version = null)
    {
        var moduleContext = new ModuleContext(rootFolder);
        if (version is not null)
            moduleContext.Version = version;

        foreach (var providerContext in moduleContext.GetProviders())
            await Packager.PackageAsync(providerContext, GetBuildSystem(moduleContext));

        if (version is not null)
            moduleContext.Save();
    }

    public async Task PublishProviderAsync(string rootFolder, Provider provider, string repositoryName)
    {
        var moduleContext = new ModuleContext(rootFolder);
            
        var providerContext = moduleContext.GetProvider(provider);
            
        await Packager.PackageAsync(providerContext, GetBuildSystem(moduleContext));

        var client = RepositoryClientManager.GetClient(repositoryName);

        await client.PublishAsync(providerContext.GetProviderSpecificModule(), providerContext.OutputZipFile);
    }

    public async Task PublishAsync(string rootFolder, string repositoryName, Version? version = null)
    {
        var moduleContext = new ModuleContext(rootFolder);
        if (version is not null)
            moduleContext.Version = version;

        foreach (var providerContext in moduleContext.GetProviders())
        {
            await Packager.PackageAsync(providerContext, GetBuildSystem(moduleContext));

            var client = RepositoryClientManager.GetClient(repositoryName);

            await client.PublishAsync(providerContext.GetProviderSpecificModule(), providerContext.OutputZipFile);
        }

        if (version is not null)
            moduleContext.Save();
    }

    public async Task SetMcmaVersionAsync(string rootFolder, Version mcmaVersion)
    {
        var moduleContext = new ModuleContext(rootFolder);
            
        await GetBuildSystem(moduleContext).SetMcmaVersionAsync(moduleContext, mcmaVersion);
    }
}