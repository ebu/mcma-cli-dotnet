namespace Mcma.Tools.Projects;

internal class McmaProjectsTool : IMcmaProjectsTool
{
    public McmaProjectsTool(IMcmaProjectModulesTool modules)
    {
        Modules = modules ?? throw new ArgumentNullException(nameof(modules));
    }

    public IMcmaProjectModulesTool Modules { get; }

    public Task NewAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task DeployAsync()
    {
        throw new NotImplementedException();
    }
}