using Mcma.Model;

namespace Mcma.Tools.ModuleRepositoryClient;

public interface IModuleRepositoryClient
{
    Task PublishAsync(Module moduleJson, string modulePackageFilePath);

    Task<QueryResults<Module>> SearchAsync(ModuleSearchCriteria searchCriteria);
}