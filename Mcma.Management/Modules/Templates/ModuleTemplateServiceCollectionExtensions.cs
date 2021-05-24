using Mcma.Management.Modules.Templates.API;
using Mcma.Management.Modules.Templates.JobWorker;
using Mcma.Management.Modules.Templates.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Modules.Templates
{
    public static class ModuleTemplateServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleTemplates(this IServiceCollection services)
            => services.AddSingleton<INewModuleTemplate, NewApiModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewWorkerModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewJobWorkerModuleTemplate>();
    }
}