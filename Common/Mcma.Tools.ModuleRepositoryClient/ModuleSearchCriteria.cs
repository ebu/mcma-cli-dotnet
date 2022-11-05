namespace Mcma.Tools.ModuleRepositoryClient;

public class ModuleSearchCriteria
{
    public string[] Keywords { get; set; }
        
    public string Namespace { get; set; }
        
    public string Name { get; set; }
        
    public string[] Providers { get; set; }
        
    public int? PageSize { get; set; }
        
    public string PageStartToken { get; set; }

    public bool IncludePreRelease { get; set; } = false;
}