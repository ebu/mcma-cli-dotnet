using System.Threading.Tasks;
using Mcma.Management.DataModel;

namespace Mcma.Management
{
    public interface IFileSystem
    {
        Task<string> CreateProjectFolderAsync(string projectRoot, Project project);

        Task<string> CreateServiceFolderAsync(string projectRoot, Project project, string serviceName);
    }
}