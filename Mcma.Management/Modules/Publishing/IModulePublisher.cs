using System.Threading.Tasks;

namespace Mcma.Management.Modules.Publishing
{
    public interface IModulePublisher
    {
        Task PublishProviderModuleAsync(string repositoryName, string provider, Version version);

        Task PublishAllProviderModulesAsync(string repositoryName, Version version);
    }
}