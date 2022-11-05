using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Tools.ModuleRepositoryClient;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Repository;

[Command("add")]
public class AddRepository : BaseCmd
{
    public AddRepository(IModuleRepositoryClientManager moduleRepositoryClientManager)
    {
        ModuleRepositoryClientManager = moduleRepositoryClientManager ?? throw new ArgumentNullException(nameof(moduleRepositoryClientManager));
    }

    private IModuleRepositoryClientManager ModuleRepositoryClientManager { get; }
        
    [Argument(0)]
    [Required]
    public string RepositoryName { get; set; }
        
    [Argument(1)]
    [Required]
    public string Url { get; set; }
        
    [Option("-t|--authType <AUTHTYPE>")]
    public string AuthType { get; set; }
        
    [Option("-c|--authContext <AUTHCONTEXT>")]
    public string AuthContext { get; set; }

    [Option("-p|--property <PROPERTY>", CommandOptionType.MultipleValue)]
    public string[] Properties { get; set; }

    protected override Task ExecuteAsync(CommandLineApplication app)
    {
        ModuleRepositoryClientManager.AddRepository(RepositoryName,
                                                    Url,
                                                    AuthType,
                                                    AuthContext,
                                                    Properties.ToNameValuePairs().ToDictionary(x => x.Name, x => x.Value));
        return Task.CompletedTask;
    }
}