using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet
{
    public interface IDotnetNewProviderModuleTemplate
    {
        ModuleType ModuleType { get; }
        
        Provider Provider { get; }

        Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters);
    }
}