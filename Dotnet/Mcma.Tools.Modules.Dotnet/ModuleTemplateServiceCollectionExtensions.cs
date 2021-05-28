using Mcma.Tools.Modules.Templates.API;
using Mcma.Tools.Modules.Templates.JobWorker;
using Mcma.Tools.Modules.Templates.Worker;

namespace Mcma.Tools.Modules.Templates
{
    public static class ModuleTemplateServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleTemplates(this IServiceCollection services)
            => services.AddSingleton<INewModuleTemplate, NewApiModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewWorkerModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewJobWorkerModuleTemplate>();
    }
}