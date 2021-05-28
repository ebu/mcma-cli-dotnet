using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.Aws
{
    public class AwsLambdaWorkerModuleTemplate : AwsLambdaApiModuleTemplate, INewProviderWorkerModuleTemplate
    {
        public AwsLambdaWorkerModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters,
                                             NewProviderModuleParameters providerParameters,
                                             string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmaawsworker");
    }
}