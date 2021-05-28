using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.Aws
{
    public class AwsLambdaApiModuleTemplate : AwsLambdaFunctionModuleTemplate, INewProviderApiModuleTemplate
    {
        public AwsLambdaApiModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateApiProjectAsync(NewModuleParameters moduleParameters,
                                          NewProviderModuleParameters providerParameters,
                                          string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmaawsapi");
    }
}