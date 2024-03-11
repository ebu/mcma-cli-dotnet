using System.ComponentModel.DataAnnotations;
using Mcma.Tools.ModuleRepositoryClient;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Repository;

[Command("set-auth")]
public class SetRepositoryAuth : BaseCmd
{
    public SetRepositoryAuth(IModuleRepositoryClientManager moduleRepositoryClientManager)
    {
        ModuleRepositoryClientManager = moduleRepositoryClientManager ?? throw new ArgumentNullException(nameof(moduleRepositoryClientManager));
    }

    private IModuleRepositoryClientManager ModuleRepositoryClientManager { get; }
        
    [Argument(0)]
    [Required]
    public required string RepositoryName { get; set; }
        
    [Argument(1)]
    [Required]
    public required string AuthType { get; set; }
        
    [Option("-c|--authContext <AUTHCONTEXT>")]
    public string? AuthContext { get; set; }
        
    protected override Task ExecuteAsync(CommandLineApplication app)
    {
        ModuleRepositoryClientManager.SetRepositoryAuth(RepositoryName, AuthType, AuthContext);
        return Task.CompletedTask;
    }
}