using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Management.Modules.Packaging;
using Mcma.Tools.ModuleRepositoryClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

        public async Task PublishProviderModuleAsync(string repositoryName, ModuleContext moduleContext)
        {   
            await ModulePackager.PackageProviderModuleAsync(moduleContext);

            var client = ClientManager.GetClient(repositoryName);

            await client.PublishAsync(JObject.FromObject(moduleContext.Module,
                                                         JsonSerializer.CreateDefault(new JsonSerializerSettings
                                                         {
                                                             ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                         })),
                                      moduleContext.OutputZipFile);
        }

        public Task PublishProviderModuleAsync(string repositoryName, string provider, Version version)
            => PublishProviderModuleAsync(repositoryName, ModuleContext.ForProviderInCurrentDirectory(provider, version));

        public async Task PublishAllProviderModulesAsync(string repositoryName, Version version)
        {
            foreach (var moduleContext in ModuleContext.ForAllInCurrentDirectory(version))
                await PublishProviderModuleAsync(repositoryName, moduleContext);
        
            File.WriteAllText("version", version.ToString());
        }
    }
}