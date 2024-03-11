namespace Mcma.Tools.Modules.Dotnet;

public interface IDotnetProjectCreator
{
    Task CreateProjectAsync(NewModuleParameters moduleParameters,
                            string srcFolder,
                            string template,
                            bool addJobTypeArg = false);
}