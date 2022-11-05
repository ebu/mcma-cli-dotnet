using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Mcma.Tools.Dotnet
{
    public static class DotnetProjectHelper
    {
        public static void RunForEachProject(Action<(string projectFolder, string csprojFile)> run)
        {
            foreach (var csprojFile in Directory.GetFiles(".", "*.csproj", SearchOption.AllDirectories))
                run((Path.GetDirectoryName(csprojFile), Path.GetFileNameWithoutExtension(csprojFile)));
        }
        
        public static void SetMcmaVersion(string version)
        {
            var parsedVersion = Version.Parse(version);

            RunForEachProject(
                x =>
                    File.WriteAllText(
                        x.csprojFile,
                        Regex.Replace(
                            File.ReadAllText(x.csprojFile),
                            @"(\<PackageReference\s+Include=""Mcma.+""\s+Version=)""\d+\.\d+\.\d+(?:-(?:alpha|beta|rc)\d+)?""(\s+\/\>)",
                            "$1\"" + parsedVersion + "\"$2")));
        }
    }
}