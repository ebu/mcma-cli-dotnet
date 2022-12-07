using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public class GoogleCloudFunctionPackager : IDotnetFunctionPackager
{
    public string Type => "GoogleCloudFunction";

    private static readonly Regex ProjectRefRegex = new(@"\<ProjectReference\s+Include=""(.+)""\s*\/\>");
        
    private static readonly string[] ExcludeFolders = { "bin", "obj", "dist", "staging", ".publish" };

    private static bool ShouldIncludeFolder(DirectoryInfo dir)
        => !ExcludeFolders.Any(f => f.Equals(dir?.Name, StringComparison.OrdinalIgnoreCase));

    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        foreach (var file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);

        foreach (var sourceSubDir in source.EnumerateDirectories().Where(ShouldIncludeFolder))
            CopyAll(sourceSubDir, target.CreateSubdirectory(sourceSubDir.Name));
    }

    private static void FlattenAndCopyDependencies(string csProjFile, string stagingFolder)
    {
        var csProjFolder = Path.GetDirectoryName(csProjFile);
        var csProjContent = File.ReadAllText(csProjFile);

        foreach (var projectCapture in ProjectRefRegex.Matches(csProjContent).Select(m => m.Groups[1].Captures[0]))
        {
            // regex capture should be the relative path from the current project folder
            var dependencyCsProjPathRelative = projectCapture.Value;

            // the last two parts of the path should be something like "RefProject/RefProject.csproj"
            var dependencyCsProjPathRelativeParts = dependencyCsProjPathRelative.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
            var dependencyCsProjFolder = dependencyCsProjPathRelativeParts.Skip(Math.Max(0, dependencyCsProjPathRelativeParts.Length - 2));

            // resolve the relative path to the absolute path of the dependency project so we can copy it over 
            var dependencyCsProjPathAbsolute = Path.GetFullPath(Path.Combine(csProjFolder, dependencyCsProjPathRelative));

            FlattenAndCopyDependencies(dependencyCsProjPathAbsolute, stagingFolder);

            // flatten the reference in the cs proj
            csProjContent = csProjContent.Replace($"Include=\"{projectCapture.Value}\"", $"Include=\"{dependencyCsProjFolder}\"");
        }

        var srcDir = new DirectoryInfo(csProjFolder);
        var destDirPath = Path.Combine(stagingFolder, srcDir.Name);
        var destDir = new DirectoryInfo(destDirPath);
        CopyAll(srcDir, destDir);

        var stagedProjFile = Path.Combine(destDirPath, Path.GetFileName(csProjFile));
        File.WriteAllText(stagedProjFile, csProjContent);
    }

    public Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo)
    {
        var projectFolder = moduleProviderContext.GetFunctionPath(functionInfo);
        var stagingFolder = Path.Combine(projectFolder, "staging");

        try
        {
            var csProjFile = Directory.EnumerateFiles(projectFolder, "*.csproj", SearchOption.AllDirectories).FirstOrDefault();
            if (csProjFile == null)
                throw new Exception($"No .csproj file found for function '{functionInfo.Name}' in {projectFolder}");

            FlattenAndCopyDependencies(csProjFile, stagingFolder);

            var outputZipFile = moduleProviderContext.GetFunctionOutputZipPath(functionInfo);

            if (File.Exists(outputZipFile))
                File.Delete(outputZipFile);

            ZipFile.CreateFromDirectory(stagingFolder, outputZipFile);

            return Task.CompletedTask;
        }
        finally
        {
            try
            {
                Directory.Delete(stagingFolder, true);
            }
            catch
            {
                // nothing to do at this point...
            }
        }
    }
}