using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates
{
    public interface INewProviderWorkerModuleTemplate : INewProviderApiModuleTemplate
    {
        Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}