namespace Mcma.Tools.Projects;

public interface IMcmaProjectModulesTool
{
    Task AddModuleAsync(string @namespace, string name, string provider, Version version);

    Task AddLocalModuleAsync(string name, string provider);
}