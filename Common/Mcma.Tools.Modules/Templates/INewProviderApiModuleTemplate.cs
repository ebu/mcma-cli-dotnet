using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates
{
    public interface INewProviderApiModuleTemplate : INewProviderModuleTemplate
    {
        Task CreateApiProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}