using System.Threading.Tasks;
using Mcma.Management.DataModel;

namespace Mcma.Management
{
    public interface IModuleRetriever
    {
        Task<Module> AddModuleToProjectAsync(string repository, string moduleId, string version = null);
    }
}