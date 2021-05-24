using System.Threading.Tasks;

namespace Mcma.Management.Modules.Templates.API
{
    public interface INewProviderApiModuleTemplate : INewProviderModuleTemplate
    {
        Task CreateApiProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}