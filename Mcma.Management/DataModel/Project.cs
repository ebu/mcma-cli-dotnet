namespace Mcma.Management.DataModel
{
    public class Project
    {
        public string Name { get; set; }
        
        public string[] ModuleDirs { get; set; }
        
        public VariableDefinition[] Variables { get; set; }
        
        public ProviderConfiguration[] Providers { get; set; }
    }
}