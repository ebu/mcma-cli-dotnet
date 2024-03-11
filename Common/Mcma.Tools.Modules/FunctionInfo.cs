namespace Mcma.Tools.Modules;

public class FunctionInfo
{
    public required string Name { get; set; }

    public required string Type { get; set; }

    public required string Path { get; set; }

    public Dictionary<string, string> Properties { get; set; } = [];
}