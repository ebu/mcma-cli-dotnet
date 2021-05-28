using Mcma.Tools.Modules.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet
{
    public static class ModuleTemplateServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleTemplates(this IServiceCollection services)
            => services.AddSingleton<INewModuleTemplate, NewApiModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewWorkerModuleTemplate>()
                       .AddSingleton<INewModuleTemplate, NewJobWorkerModuleTemplate>();
    }
}