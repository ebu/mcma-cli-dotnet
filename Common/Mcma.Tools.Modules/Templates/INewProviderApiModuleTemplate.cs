using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates.API
{
    public interface INewProviderApiModuleTemplate : INewProviderModuleTemplate
    {
        Task CreateApiProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}