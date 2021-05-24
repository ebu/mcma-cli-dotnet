using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Management.Modules.Publishing;
using Mcma.Management.Modules.Templates;

namespace Mcma.Management.Modules
{
    internal class McmaModuleManagementTool : IMcmaModuleManagementTool
    {
        public McmaModuleManagementTool(IModulePublisher publisher, IEnumerable<INewModuleTemplate> templates)
        {
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            Templates = templates?.ToArray() ?? new INewModuleTemplate[0];
        }
        
        private IModulePublisher Publisher { get; }
        
        private INewModuleTemplate[] Templates { get; }
        
        public async Task NewAsync(string template, NewModuleParameters newModuleParameters)
        {
            var moduleTemplate = Templates.FirstOrDefault(x => x.Type.Equals(template, StringComparison.OrdinalIgnoreCase));
            if (moduleTemplate == null)
                throw new Exception($"Invalid template '{template}'.");

            await moduleTemplate.CreateInstanceAsync(newModuleParameters);
        }

        public async Task PublishAsync(string repositoryName, Version version, string provider = null)
        {
            if (provider != null)
                await Publisher.PublishProviderModuleAsync(repositoryName, provider, version);
            else
                await Publisher.PublishAllProviderModulesAsync(repositoryName, version);
        }
    }
}