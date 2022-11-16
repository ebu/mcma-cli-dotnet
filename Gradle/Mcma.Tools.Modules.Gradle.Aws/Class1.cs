using System;
using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Gradle.Aws
{
    public abstract class AwsLambdaFunctionGradleModuleTemplate : IGradleNewProviderModuleTemplate
    {
        public abstract ModuleType ModuleType { get; }

        public Provider Provider => Provider.AWS;

        public Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        {
            throw new NotImplementedException();
        }
    }
}