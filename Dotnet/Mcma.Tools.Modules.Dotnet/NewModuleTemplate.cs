using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates
{
    public abstract class NewModuleTemplate<T> : INewModuleTemplate where T : INewProviderModuleTemplate
    {
        protected NewModuleTemplate(IDotnetCli dotnetCli, IEnumerable<T> providerTemplates)
        {
            DotnetCli = dotnetCli;
            ProviderTemplates = providerTemplates?.ToArray() ?? new T[0];
        }
        
        private IDotnetCli DotnetCli { get; }
        
        private T[] ProviderTemplates { get; }
        
        public abstract string Type { get; }

        public async Task CreateInstanceAsync(NewModuleParameters parameters)
        {
            await DotnetCli.RunCmdWithOutputAsync("new", "-i", "Mcma.Modules.Templates");
            Directory.CreateDirectory(parameters.ModuleDirectory);

            var slnName = $"{parameters.NamespaceInPascalCase}.Mcma.Modules.{parameters.NameInPascalCase}";

            await DotnetCli.RunCmdWithOutputAsync("new",
                                                  "sln",
                                                  "-n",
                                                  slnName,
                                                  "-o",
                                                  parameters.ModuleDirectory);

            var slnFile = Path.Combine(parameters.ModuleDirectory, $"{slnName}.sln");
            
            File.WriteAllText("version", Version.Initial().ToString());

            var module = new Module
            {
                Namespace = parameters.NamespaceInPascalCase,
                Name = parameters.NameInKebabCase,
                Version = Version.Initial().ToString(),
                DisplayName = parameters.DisplayName,
                Description = parameters.Description
            };

            foreach (var providerParams in parameters.Providers)
            {
                var providerTemplate = ProviderTemplates.FirstOrDefault(x => x.Provider == providerParams.Provider);
                if (providerTemplate == null)
                    throw new Exception($"Provider '{providerParams.Provider}' is not supported.");
                
                module.Provider = providerParams.Provider;

                var providerFolder = parameters.GetProviderDir(providerParams.Provider);
                Directory.CreateDirectory(providerFolder);
                
                var moduleJsonPath = Path.Combine(providerFolder, "module.json");
                var moduleJson = JObject.FromObject(module).ToString(Formatting.Indented);
                File.WriteAllText(moduleJsonPath, moduleJson);

                var modulePackageJsonPath = Path.Combine(providerFolder, "module-package.json");
                var modulePackageJson = JObject.FromObject(new { functions = new { } }).ToString(Formatting.Indented);
                File.WriteAllText(modulePackageJsonPath, modulePackageJson);
                
                var moduleTfPath = Path.Combine(providerFolder, "module.tf");
                var moduleTf = providerTemplate.GetModuleTf(providerParams.Args);
                File.WriteAllText(moduleTfPath, moduleTf);
                
                var variablesTfPath = Path.Combine(providerFolder, "variables.tf");
                var variablesTf = providerTemplate.GetVariablesTf(providerParams.Args);
                File.WriteAllText(variablesTfPath, variablesTf);
                
                var outputsTfPath = Path.Combine(providerFolder, "outputs.tf");
                var outputsTf = providerTemplate.GetOutputsTf(providerParams.Args);
                File.WriteAllText(outputsTfPath, outputsTf);

                var srcFolder = parameters.GetProviderSrcDir(providerParams.Provider);
                Directory.CreateDirectory(srcFolder);
                
                await CreateProjectsAsync(providerTemplate, srcFolder, parameters, providerParams);
                
                foreach (var projectFile in Directory.EnumerateFiles(srcFolder, "*.csproj", SearchOption.AllDirectories))
                {
                    Console.WriteLine("Running dotnet sln " + string.Join(" ", slnFile, "add", projectFile, "-s", providerParams.Provider));
                    await DotnetCli.RunCmdWithOutputAsync("sln", slnFile, "add", projectFile, "-s", providerParams.Provider);
                }
            }
        }

        protected abstract Task CreateProjectsAsync(T providerTemplate,
                                                    string srcFolder,
                                                    NewModuleParameters moduleParameters,
                                                    NewProviderModuleParameters providerParameters);
    }
}