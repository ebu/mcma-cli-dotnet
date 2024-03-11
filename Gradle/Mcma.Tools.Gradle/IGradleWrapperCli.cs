namespace Mcma.Tools.Gradle;

public interface IGradleWrapperCli
{
    Task<(string stdOut, string stdErr)> ExecuteTaskAsync(string taskName, params string[] args);
}