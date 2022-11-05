using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;

namespace Mcma.Tools.Modules.Dotnet
{
    public class DotnetModuleBuildSystem : IModuleBuildSystem
    {
        public const string Name = "dotnet";

        public DotnetModuleBuildSystem(IDotnetCli dotnetCli,
                                       IEnumerable<IDotnetFunctionPackager> functionPackagers,
                                       IEnumerable<IDotnetNewProviderModuleTemplate> templates)
        {
            DotnetCli = dotnetCli ?? throw new ArgumentNullException(nameof(dotnetCli));
            FunctionPackagers = functionPackagers?.ToArray() ?? Array.Empty<IDotnetFunctionPackager>();
            Templates = templates?.ToArray() ?? Array.Empty<IDotnetNewProviderModuleTemplate>();
        }

        private IDotnetCli DotnetCli { get; }

        private IEnumerable<IDotnetFunctionPackager> FunctionPackagers { get; }

        private IDotnetNewProviderModuleTemplate[] Templates { get; }

        string IModuleBuildSystem.Name => Name;

        public bool UseForModule(ModuleContext moduleContext) =>
            Directory.EnumerateFiles(moduleContext.RootFolder, "*.sln", SearchOption.TopDirectoryOnly).Any();

        public async Task InitializeAsync(NewModuleParameters parameters)
        {   
            await DotnetCli.InstallTemplateAsync("Mcma.Modules.Templates");
            
            var slnName = $"{parameters.NamespaceInPascalCase}.Mcma.Modules.{parameters.NameInPascalCase}";
            await DotnetCli.NewAsync("sln", parameters.ModuleDirectory, ("name", slnName));
        }

        public string[] GetGitIgnorePaths(ModuleType moduleType, ModuleContext moduleContext, NewModuleParameters parameters)
            => new[]
            {
                "bin",
                "obj",
                "dist",
                ".terraform",
                ".publish",
                "local.settings.json",
                "dotnet-tools.json"
            };

        public async Task CreateProviderProjectsAsync(ModuleType moduleType,
                                                      ModuleContext moduleContext,
                                                      NewModuleParameters parameters,
                                                      NewProviderModuleParameters providerParams,
                                                      string srcFolder)
        {
            var providerTemplate = Templates.FirstOrDefault(x => x.ModuleType == moduleType && x.Provider == providerParams.Provider);
            if (providerTemplate == null)
                throw new Exception($"Provider '{providerParams.Provider}' is not supported.");

            await providerTemplate.CreateProjectsAsync(srcFolder, parameters, providerParams);

            var slnFile = Path.Combine(parameters.ModuleDirectory, $"{parameters.NamespaceInPascalCase}.Mcma.Modules.{parameters.NameInPascalCase}");

            foreach (var projectFile in Directory.EnumerateFiles(srcFolder, "*.csproj", SearchOption.AllDirectories))
                await DotnetCli.AddProjectToSolutionAsync(slnFile, projectFile, providerParams.Provider);
        }

        public async Task SetMcmaVersionAsync(ModuleContext moduleContext, Version mcmaVersion)
        {
            foreach (var project in DotnetProject.GetAllForModuleContext(moduleContext))
                project.SetVersionOnMcmaPackageReferences(mcmaVersion);

            await DotnetCli.RestoreAsync(moduleContext.RootFolder);
        }

        public async Task PackageFunctionAsync(ModuleProviderContext moduleProviderContext, FunctionInfo function)
        {
            var packager = FunctionPackagers.FirstOrDefault(fp => string.Equals(fp.Type, function.Type, StringComparison.OrdinalIgnoreCase));
            if (packager == null)
                throw new Exception($"No packager found for type '{function.Type}'");
                    
            await packager.PackageAsync(moduleProviderContext, function);
        }
    }
}