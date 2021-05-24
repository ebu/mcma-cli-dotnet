using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mcma.Management.Modules.FileSystem
{
    internal class FileSystemModuleRepositoryClient : IModuleRepositoryClient
    {
        public FileSystemModuleRepositoryClient(string rootFolder)
        {
            RootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        }
        
        private string RootFolder { get; }

        public async Task PublishAsync(JObject moduleJson, string modulePackageFilePath)
        {
            var path =
                Path.Combine(RootFolder,
                             moduleJson["namespace"]?.Value<string>(),
                             moduleJson["name"]?.Value<string>(),
                             moduleJson["provider"]?.Value<string>(),
                             moduleJson["version"]?.Value<string>());

            Directory.CreateDirectory(path);

            using var readStream = File.OpenRead(modulePackageFilePath);
            using var writeStream = File.Create(Path.Combine(path, Path.GetFileName(modulePackageFilePath)));

            await readStream.CopyToAsync(writeStream);
        }
    }
}