namespace Mcma.Tools;

public class CliExecutableNotFoundException : Exception
{
    public CliExecutableNotFoundException(string executable)
        : base(
            $"'{executable}' was not found on any of the paths in the PATH variable. " +
            "Please ensure that it's properly installed in a folder registered in PATH or specify the full path.")
    {
    }
}