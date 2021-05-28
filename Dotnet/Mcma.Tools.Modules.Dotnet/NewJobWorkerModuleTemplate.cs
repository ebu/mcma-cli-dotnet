using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates.JobWorker
{
    public class NewJobWorkerModuleTemplate : NewModuleTemplate<INewProviderJobWorkerModuleTemplate>
    {
        public NewJobWorkerModuleTemplate(IDotnetCli dotnetCli, IEnumerable<INewProviderJobWorkerModuleTemplate> providerTemplates)
            : base(dotnetCli, providerTemplates)
        {
        }
        
        public override string Type => "JobWorker";

        protected override async Task CreateProjectsAsync(INewProviderJobWorkerModuleTemplate providerTemplate,
                                                          string srcFolder,
                                                          NewModuleParameters moduleParameters,
                                                          NewProviderModuleParameters providerParameters)
        {
            await providerTemplate.CreateApiProjectAsync(moduleParameters, providerParameters, srcFolder);
            await providerTemplate.CreateWorkerProjectAsync(moduleParameters, providerParameters, srcFolder);
        }
    }
}