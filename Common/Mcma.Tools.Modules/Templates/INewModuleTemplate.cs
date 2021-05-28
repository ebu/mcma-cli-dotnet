using System.Threading.Tasks;

namespace Mcma.Management.Modules.Templates
{
    public interface INewModuleTemplate
    {
        string Type { get; }
        
        Task CreateInstanceAsync(NewModuleParameters parameters);
    }
}