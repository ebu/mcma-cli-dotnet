using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Tools.ModuleRepositoryClient;
using Mcma.Tools.Modules.Packaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mcma.Tools.Modules.Publishing
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