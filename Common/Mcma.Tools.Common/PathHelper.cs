namespace Mcma.Tools;

public static class PathHelper
{
    public static string ExpandPath(string path)
    {
        if (!path.StartsWith("~/"))
            return path;
        
        var homeEnvVars = OperatingSystem.IsWindows() ? ["HOMEDRIVE", "HOMEPATH"] : new[] { "HOME" };

        var homeDir = Environment.ExpandEnvironmentVariables(string.Join("", homeEnvVars.Select(v => $"%{v}%")));

        return Path.Combine(homeDir, path[2..]);
    }
}