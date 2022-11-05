using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public abstract class GoogleCloudFunctionModuleTemplate : IDotnetNewProviderModuleTemplate
{
    protected GoogleCloudFunctionModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
    {
        DotnetProjectCreator = dotnetProjectCreator ?? throw new ArgumentNullException(nameof(dotnetProjectCreator));
    }
        
    protected IDotnetProjectCreator DotnetProjectCreator { get; }

    public Provider Provider => Provider.AWS;

    public abstract ModuleType ModuleType { get; }

    public virtual string GetModuleTf(IDictionary<string, string> args)
    {
        return $@"terraform {{
  required_providers {{
    google = {{
      source  = ""hashicorp/google""
      version = ""~> 3.69.0""
    }} 
  }}
}}

provider ""google"" {{
  {string.Join(Environment.NewLine + "  ", args.Select(kvp => $"{kvp.Key} = \"{kvp.Value}\""))}
}}";
    }

    public virtual string GetVariablesTf(IDictionary<string, string> args) => string.Empty;

    public virtual string GetOutputsTf(IDictionary<string, string> args) => string.Empty;

    public abstract Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters);
}