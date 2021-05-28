using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Publishing;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules
{
    internal class McmaModulesTool : IMcmaModulesTool
    {
        public McmaModulesTool(IModulePackager packager, IModulePublisher publisher, IEnumerable<INewModuleTemplate> templates)
        {
            Packager = packager ?? throw new ArgumentNullException(nameof(packager));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            Templates = templates?.ToArray() ?? new INewModuleTemplate[0];
        }

        private IModulePackager Packager { get; }

        private IModulePublisher Publisher { get; }
        
        private INewModuleTemplate[] Templates { get; }
        
        public async Task NewAsync(string template, NewModuleParameters newModuleParameters)
        {
            var moduleTemplate = Templates.FirstOrDefault(x => x.Type.Equals(template, StringComparison.OrdinalIgnoreCase));
            if (moduleTemplate == null)
                throw new Exception($"Invalid template '{template}'.");

            await moduleTemplate.CreateInstanceAsync(newModuleParameters);
        }

        public async Task PackageAsync(Version version, string provider = null)
        {
            if (provider != null)
                await Packager.PackageProviderModuleAsync(provider, version);
            else
                await Packager.PackageAllProviderModulesAsync(version);
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