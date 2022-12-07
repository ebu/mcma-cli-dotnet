using System.Threading.Tasks;
using Mcma.Tools.Modules;

namespace Mcma.Management.Docker;

public interface IDockerImageFunctionHelper
{
    Task BuildAndPushFunctionImageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo);
}