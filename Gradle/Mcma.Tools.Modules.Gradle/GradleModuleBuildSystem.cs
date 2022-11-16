using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mcma.Tools.Gradle;

namespace Mcma.Tools.Modules.Gradle;

public class GradleModuleBuildSystem : IModuleBuildSystem
{
    public const string Name = "gradle";

    public GradleModuleBuildSystem(IGradleCli gradleCli,
                                   IGradleWrapperCli gradleWrapperCli,
                                   IEnumerable<IGradleFunctionPackager> functionPackagers,
                                   IEnumerable<IGradleNewProviderModuleTemplate> templates)
    {
        GradleCli = gradleCli ?? throw new ArgumentNullException(nameof(gradleCli));
        GradleWrapperCli = gradleWrapperCli ?? throw new ArgumentNullException(nameof(gradleWrapperCli));
        FunctionPackagers = functionPackagers?.ToArray() ?? Array.Empty<IGradleFunctionPackager>();
        Templates = templates?.ToArray() ?? Array.Empty<IGradleNewProviderModuleTemplate>();
    }

    private IGradleCli GradleCli { get; }
        
    private IGradleWrapperCli GradleWrapperCli { get; }

    private IEnumerable<IGradleFunctionPackager> FunctionPackagers { get; }

    private IGradleNewProviderModuleTemplate[] Templates { get; }
        
    private HttpClient HttpClient { get; } = new();

    string IModuleBuildSystem.Name => Name;
        
    private static string WrapperFileName { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "gradlew.bat" : "gradlew";

    private static string GetWrapperFilePath(ModuleContext moduleContext) => Path.Combine(moduleContext.RootFolder, WrapperFileName);

    private static bool WrapperFileExists(ModuleContext moduleContext) => File.Exists(GetWrapperFilePath(moduleContext));

    private static async Task ExtractResourcesAsync(NewModuleParameters parameters, string prefix, string localFolder)
    {
        var type = typeof(GradleModuleBuildSystem);
        var assembly = type.Assembly;

        var resourcePrefix = $"{type.Namespace}.{prefix}.";
        var taskResourceNames = assembly.GetManifestResourceNames()
                                        .Where(x => x.StartsWith(resourcePrefix, StringComparison.OrdinalIgnoreCase));

        foreach (var taskResourceName in taskResourceNames)
        {
            using var taskResourceStream = assembly.GetManifestResourceStream(taskResourceName);
            if (taskResourceStream == null)
                continue;

            var localFilePath = Path.Combine(parameters.ModuleDirectory, localFolder, taskResourceName.Substring(resourcePrefix.Length));

            using var localFileStream = File.Create(localFilePath);
            await taskResourceStream.CopyToAsync(localFileStream);
        }
    }

    public bool UseForModule(ModuleContext moduleContext) => WrapperFileExists(moduleContext);

    public async Task InitializeAsync(NewModuleParameters parameters)
    {
        await GradleCli.InstallWrapperAsync();

        await ExtractResourcesAsync(parameters, "Tasks", "");
        await ExtractResourcesAsync(parameters, "Config", "");
    }

    public string[] GetGitIgnorePaths(ModuleType moduleType, ModuleContext moduleContext, NewModuleParameters parameters)
        => new[]
        {
            ".gradle",
            ".terraform",
            "build",
            "node_modules",
            "gradle.properties",
            "aws-credentials.json"
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

        var projectsToAdd = string.Join(",\n        ",
                                        Directory.EnumerateFiles(srcFolder, "build.gradle", SearchOption.AllDirectories)
                                                 .Select(buildFile => buildFile.Substring(srcFolder.Length).Trim(Path.DirectorySeparatorChar)
                                                                               .Replace(Path.DirectorySeparatorChar, ':')));
            
        var settingsFile = Path.Combine(parameters.ModuleDirectory, "settings.gradle");

        if (!File.Exists(settingsFile))
            File.WriteAllText(settingsFile, "include ");
            
        File.AppendAllText(settingsFile, projectsToAdd);
    }

    public Task SetMcmaVersionAsync(ModuleContext moduleContext, Version mcmaVersion)
        => GradleWrapperCli.ExecuteTaskAsync("setMcmaVersion", "-p", moduleContext.RootFolder);

    public async Task PackageFunctionAsync(ModuleProviderContext moduleProviderContext, FunctionInfo function)
    {
        var packager = FunctionPackagers.FirstOrDefault(fp => string.Equals(fp.Type, function.Type, StringComparison.OrdinalIgnoreCase));
        if (packager == null)
            throw new Exception($"No packager found for type '{function.Type}'");
                    
        await packager.PackageAsync(moduleProviderContext, function);
    }

    private Task BuildAsync(ModuleProviderContext moduleProviderContext)
        => GradleWrapperCli.ExecuteTaskAsync($"{moduleProviderContext.Provider.ToString().ToLower()}:build",
                                             "-p",
                                             moduleProviderContext.ModuleContext.RootFolder);

    private Task BuildAsync(ModuleContext moduleContext)
        => GradleWrapperCli.ExecuteTaskAsync("build", "-p", moduleContext.RootFolder);
}