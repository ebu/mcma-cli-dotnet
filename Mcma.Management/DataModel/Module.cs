namespace Mcma.Management.DataModel
{
    public class Module
    {
        public string Id { get; set; }
        
        public string Namespace { get; set; }
        
        public string Name { get; set; }
        
        public string Provider { get; set; }
        
        public string Version { get; set; }
        
        public string Description { get; set; }
        
        public string Link { get; set; }
        
        public VariableDefinition[] InputVariables { get; set; }
        
        public VariableDefinition[] OutputVariables { get; set; }
        
        public DeploymentAction[] DeploymentActions { get; set; }
    }
}