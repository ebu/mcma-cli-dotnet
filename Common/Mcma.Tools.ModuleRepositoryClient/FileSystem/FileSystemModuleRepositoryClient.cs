using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Model;

namespace Mcma.Tools.ModuleRepositoryClient.FileSystem;

internal class FileSystemModuleRepositoryClient : IModuleRepositoryClient
{
    public FileSystemModuleRepositoryClient(string rootFolder)
    {
        RootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
    }
        
    private string RootFolder { get; }

    public async Task PublishAsync(Module module, string modulePackageFilePath)
    {
        var path = Path.Combine(RootFolder, module.Namespace, module.Name, module.Provider, module.Version);

        Directory.CreateDirectory(path);

        using var readStream = File.OpenRead(modulePackageFilePath);
        using var writeStream = File.Create(Path.Combine(path, Path.GetFileName(modulePackageFilePath)));

        await readStream.CopyToAsync(writeStream);
    }

    public Task<QueryResults<Module>> SearchAsync(ModuleSearchCriteria searchCriteria)
    {
        return Task.FromResult(new QueryResults<Module>());
    }
}