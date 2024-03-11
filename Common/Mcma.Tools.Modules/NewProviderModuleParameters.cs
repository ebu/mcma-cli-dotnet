namespace Mcma.Tools.Modules;

public class NewProviderModuleParameters
{
    public NewProviderModuleParameters(string provider, params (string, string)[]? args)
    {
        Provider = new Provider(provider);
        Args = args?.ToDictionary(x => x.Item1, x => x.Item2) ?? [];
    }
        
    public Provider Provider { get; }
        
    public Dictionary<string, string> Args { get; }
}