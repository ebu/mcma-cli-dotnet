using System.Threading.Tasks;

namespace Mcma.Tools.Git
{
    public interface IGitCli
    {
        Task InitAsync(string dir);

        Task AddAsync(string path);
    }
}