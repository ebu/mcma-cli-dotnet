using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mcma.Tools.Gradle;

namespace Mcma.Tools.Modules.Gradle;

public class GradleModuleBuildSystem : IModuleBuildSystem
{
    public const string Name = "gradle";

    public GradleModuleBuildSystem(IGradleCli gradleCli,
                                   IGradleWrapperCli gradleWrapperCli,
                                   IGradleFunctionPackager functionPackager,
                                   IEnumerable<IGradleNewProviderModuleTemplate> templates)
    {
        GradleCli = gradleCli ?? throw new ArgumentNullException(nameof(gradleCli));
        GradleWrapperCli = gradleWrapperCli ?? throw new ArgumentNullException(nameof(gradleWrapperCli));
        FunctionPackager = functionPackager ?? throw new ArgumentNullException(nameof(functionPackager));
        Templates = templates?.ToArray() ?? Array.Empty<IGradleNewProviderModuleTemplate>();
    }

    private IGradleCli GradleCli { get; }

    private IGradleWrapperCli GradleWrapperCli { get; }

    private IGradleFunctionPackager FunctionPackager { get; }

    private IGradleNewProviderModuleTemplate[] Templates { get; }

    string IModuleBuildSystem.Name => Name;

    private static string WrapperFileName { get; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "gradlew.bat" : "gradlew";

    private static string GetWrapperFilePath(ModuleContext moduleContext)
        => Path.Combine(moduleContext.RootFolder, WrapperFileName);

    private static bool WrapperFileExists(ModuleContext moduleContext)
        => File.Exists(GetWrapperFilePath(moduleContext));

    private static async Task ExtractResourcesAsync(NewModuleParameters parameters, string prefix, string localFolder)
    {
        var type = typeof(GradleModuleBuildSystem);
        var assembly = type.Assembly;

        var resourcePrefix = $"{type.Namespace}.{prefix}.";
        var taskResourceNames = assembly.GetManifestResourceNames()
                                        .Where(x => x.StartsWith(resourcePrefix, StringComparison.OrdinalIgnoreCase));

        foreach (var taskResourceName in taskResourceNames)
        {
            await using var taskResourceStream = assembly.GetManifestResourceStream(taskResourceName);
            if (taskResourceStream == null)
                continue;

            var localFilePath = Path.Combine(parameters.ModuleDirectory,
                                             localFolder,
                                             taskResourceName.Substring(resourcePrefix.Length));

            await using var localFileStream = File.Create(localFilePath);
            await taskResourceStream.CopyToAsync(localFileStream);
        }
    }

    public bool UseForModule(ModuleContext moduleContext)
        => WrapperFileExists(moduleContext);

    public async Task InitializeAsync(NewModuleParameters parameters)
    {
        await GradleCli.InitAsync(parameters.NameInKebabCase);

        await ExtractResourcesAsync(parameters, "Tasks", "");
        await ExtractResourcesAsync(parameters, "Config", "");
    }

    public string[] GetGitIgnorePaths(ModuleType moduleType, ModuleContext moduleContext,
                                      NewModuleParameters parameters)
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
        var providerTemplate =
            Templates.FirstOrDefault(x => x.ModuleType == moduleType && x.Provider == providerParams.Provider);
        if (providerTemplate == null)
            throw new Exception($"Provider '{providerParams.Provider}' is not supported.");

        await providerTemplate.CreateProjectsAsync(srcFolder, parameters, providerParams);

        var projectsToAdd = string.Join(",\n        ",
                                        Directory.EnumerateFiles(srcFolder, "build.gradle", SearchOption.AllDirectories)
                                                 .Select(buildFile => buildFile
                                                                      .Substring(srcFolder.Length)
                                                                      .Trim(Path.DirectorySeparatorChar)
                                                                      .Replace(Path.DirectorySeparatorChar, ':')));

        var settingsFile = Path.Combine(parameters.ModuleDirectory, "settings.gradle");

        if (!File.Exists(settingsFile))
            await File.WriteAllTextAsync(settingsFile, "include ");

        await File.AppendAllTextAsync(settingsFile, projectsToAdd);
    }

    public Task SetMcmaVersionAsync(ModuleContext moduleContext, Version mcmaVersion)
        => GradleWrapperCli.ExecuteTaskAsync("setMcmaVersion", "-p", moduleContext.RootFolder);

    public Task PackageFunctionAsync(ModuleProviderContext moduleProviderContext, FunctionInfo function)
        => FunctionPackager.PackageAsync(moduleProviderContext, function);
}