using System.Threading.Tasks;

namespace Mcma.Tools.Dotnet
{    
    public interface IDotnetCli
    {
        Task BuildAsync(string target);
        
        Task RestoreAsync(string target);

        Task PublishAsync(string projectPath, string config = "Release", string arch = "linux-x64", string outputFolder = null);
        
        Task NewAsync(string templateName, string folder, params (string name, string value)[] options);

        Task InstallTemplateAsync(string templateName);

        Task AddProjectToSolutionAsync(string slnFile, string projectFile, string subFolder);

        Task InstallToolAsync(string toolName, string toolManifestFile);

        Task RestoreToolsAsync(string toolManifestFile);

        Task<(string stdOut, string stdErr)> RunCustomAsync(string cmdName, params string[] args);
    }
}