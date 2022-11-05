using System;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Gradle.Aws
{
    public class AwsLambdaFunctionGradleModuleTemplate : IGradleNewProviderModuleTemplate
    {
        public ModuleType ModuleType { get; }

        public Provider Provider { get; }

        public Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        {
            throw new NotImplementedException();
        }
    }
}