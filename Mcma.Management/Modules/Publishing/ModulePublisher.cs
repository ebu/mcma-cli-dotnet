using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Management.Modules.Packaging;

namespace Mcma.Management.Modules.Publishing
{ 
    public class ModulePublisher : IModulePublisher
    {
        public ModulePublisher(IModulePackager modulePackager, IModuleRepositoryClientManager clientManager)
        {
            ModulePackager = modulePackager ?? throw new ArgumentNullException(nameof(modulePackager));
            ClientManager = clientManager ?? throw new ArgumentNullException(nameof(clientManager));
        }

        private IModulePackager ModulePackager { get; }

        private IModuleRepositoryClientManager ClientManager { get; }

        private async Task PublishProviderModuleAsync(string repositoryName, ModuleContext moduleContext)
        {   
            await ModulePackager.PackageModuleAsync(moduleContext);

            var client = ClientManager.GetClient(repositoryName);
            
            await client.PublishAsync(moduleContext.GetModuleJson(), moduleContext.OutputZipFile);
        }

        public async Task PublishProviderModuleAsync(string repositoryName, string provider, Version version)
        {
            var moduleFilePath = Path.Combine(Directory.GetCurrentDirectory(), provider, "module.json");
            if (!File.Exists(moduleFilePath))
                throw new Exception($"module.json not found at {moduleFilePath}");
            
            var moduleContext = new ModuleContext(Path.GetDirectoryName(moduleFilePath), version);

            await PublishProviderModuleAsync(repositoryName, moduleContext);
        }

        public async Task PublishAllProviderModulesAsync(string repositoryName, Version version)
        {
            foreach (var moduleFilePath in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "module.json", SearchOption.AllDirectories))
            {
                if (moduleFilePath.IndexOf(".publish", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;
            
                var moduleContext = new ModuleContext(Path.GetDirectoryName(moduleFilePath), version);
                
                await PublishProviderModuleAsync(repositoryName, moduleContext);
            }
        
            File.WriteAllText("version", version.ToString());
        }
    }
}