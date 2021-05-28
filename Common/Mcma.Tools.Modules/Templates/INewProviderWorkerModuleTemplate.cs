using System.Threading.Tasks;
using Mcma.Tools.Modules.Templates.API;

namespace Mcma.Tools.Modules.Templates.Worker
{
    public interface INewProviderWorkerModuleTemplate : INewProviderApiModuleTemplate
    {
        Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}