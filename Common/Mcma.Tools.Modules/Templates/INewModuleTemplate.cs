using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Templates
{
    public interface INewModuleTemplate
    {
        string Type { get; }
        
        Task CreateInstanceAsync(NewModuleParameters parameters);
    }
}