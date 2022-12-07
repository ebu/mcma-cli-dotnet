using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools.Dotnet;

internal class DotnetCli : CliBase, IDotnetCli
{
    public DotnetCli(ICliExecutor cliExecutor)
        : base(cliExecutor)
    {
    }

    protected override string Executable => "dotnet";

    public Task BuildAsync(string target)
        => RunCmdWithOutputAsync("build", target);

    public Task RestoreAsync(string target)
        => RunCmdWithOutputAsync("restore", target);

    public Task PublishAsync(string projectPath, string config = "Release", string arch = "linux-x64", string outputFolder = null)
        => RunCmdWithOutputAsync("publish",
                                 new[] { projectPath, "-c", config, "-r", arch }
                                     .Concat(outputFolder != null ? new[] { "-o", outputFolder } : Array.Empty<string>())
                                     .ToArray());

    public Task NewAsync(string templateName, string folder, params (string name, string value)[] options)
        => RunCmdWithOutputAsync("new",
                                 new[] { templateName, "-o", folder }
                                     .Concat(options.SelectMany(x => new[] { "--" + x.name, x.value }))
                                     .ToArray());

    public Task InstallTemplateAsync(string templateName)
        => RunCmdWithoutOutputAsync("new", "-i", templateName);

    public Task AddProjectToSolutionAsync(string slnFile, string projectFile, string subFolder)
        => RunCmdWithOutputAsync("sln", slnFile, "add", projectFile, "-s", subFolder);

    public Task InstallToolAsync(string toolName, string toolManifestFile)
        => RunCmdWithOutputAsync("tool", "install", toolName, "--tool-manifest", toolManifestFile);

    public Task RestoreToolsAsync(string toolManifestFile)
        => RunCmdWithOutputAsync("tool", "restore", "--tool-manifest", toolManifestFile);

    public Task<(string stdOut, string stdErr)> RunCustomAsync(string cmdName, params string[] args)
        => RunCmdWithOutputAsync(cmdName, args);
}