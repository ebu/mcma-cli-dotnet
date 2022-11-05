using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;

namespace Mcma.Tools.Modules.Dotnet.Aws;

public abstract class AwsLambdaFunctionDotnetModuleTemplate : IDotnetNewProviderModuleTemplate
{
    protected AwsLambdaFunctionDotnetModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
    {
        DotnetProjectCreator = dotnetProjectCreator ?? throw new ArgumentNullException(nameof(dotnetProjectCreator));
    }
        
    protected IDotnetProjectCreator DotnetProjectCreator { get; }

    public abstract ModuleType ModuleType { get; }

    public Provider Provider => Provider.AWS;

    public virtual string GetModuleTf(IDictionary<string, string> args)
    {
        if (!args.ContainsKey("region"))
            throw new Exception("AWS provider must specify a region");
            
        return $@"terraform {{
  required_providers {{
    aws = {{
      source  = ""hashicorp/aws""
      version = ""~> 3.0""
    }} 
  }}
}}

provider ""aws"" {{
  {string.Join(Environment.NewLine + "  ", args.Select(kvp => $"{kvp.Key} = \"{kvp.Value}\""))}
}}";
    }

    public virtual string GetVariablesTf(IDictionary<string, string> args) => string.Empty;

    public virtual string GetOutputsTf(IDictionary<string, string> args) => string.Empty;

    public abstract Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters);
}