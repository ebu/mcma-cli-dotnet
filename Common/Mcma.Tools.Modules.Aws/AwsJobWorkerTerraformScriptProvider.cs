namespace Mcma.Tools.Modules.Aws;

public class AwsJobWorkerTerraformScriptProvider : IModuleTerraformScriptProvider
{
    public ModuleType ModuleType => ModuleType.JobWorker; 
    
    public Provider Provider => Provider.AWS;
    
    public string GetModuleTf(IDictionary<string, string> args)
    {
        throw new NotImplementedException();
    }

    public string GetVariablesTf(IDictionary<string, string> args)
    {
        throw new NotImplementedException();
    }

    public string GetOutputsTf(IDictionary<string, string> args)
    {
        throw new NotImplementedException();
    }
}