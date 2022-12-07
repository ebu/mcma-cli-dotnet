using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.Azure;

public abstract class AzureFunctionAppModuleTemplate : IDotnetNewProviderModuleTemplate
{
    protected AzureFunctionAppModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
    {
        DotnetProjectCreator = dotnetProjectCreator ?? throw new ArgumentNullException(nameof(dotnetProjectCreator));
    }
        
    protected IDotnetProjectCreator DotnetProjectCreator { get; }

    public abstract ModuleType ModuleType { get; }

    public Provider Provider => Provider.AWS;

    public virtual string GetModuleTf(IDictionary<string, string> args)
    {
        return $@"terraform {{
  required_providers {{
    azurerm = {{
      source  = ""hashicorp/azurerm""
      version = ""~> 2.60.0""
    }} 
  }}
}}

provider ""azurerm"" {{
  features {{}}
  {string.Join(Environment.NewLine + "  ", args.Select(kvp => $"{kvp.Key} = \"{kvp.Value}\""))}
}}";
    }

    public virtual string GetVariablesTf(IDictionary<string, string> args) => string.Empty;

    public virtual string GetOutputsTf(IDictionary<string, string> args) => string.Empty;

    public abstract Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters);
}