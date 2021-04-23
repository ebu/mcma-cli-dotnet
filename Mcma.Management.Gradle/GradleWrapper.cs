using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Mcma.Management.Gradle
{
    internal class GradleWrapper : IGradleWrapper
    {
        public GradleWrapper(IGradleVersionProvider versionProvider, IGradleDownloadProgressReporter downloadProgressReporter, IOptions<GradleWrapperOptions> optionsWrapper)
        {
            VersionProvider = versionProvider ?? throw new ArgumentNullException(nameof(versionProvider));
            DownloadProgressReporter = downloadProgressReporter;

            var options = optionsWrapper.Value ?? new GradleWrapperOptions();
            
            Version = options.Version;
            VersionDir = Path.Combine(options.ProjectRoot, options.GradleDir, options.Version);
            GradleBinary = Path.Combine(VersionDir, "bin", "gradle" + (OperatingSystem.IsWindows() ? ".bat" : ""));
            
            EnsureDownloadedTask = new Lazy<Task>(EnsureDownloadedAsync);
        }
        
        private IGradleVersionProvider VersionProvider { get; }

        private IGradleDownloadProgressReporter DownloadProgressReporter { get; }

        private string Version { get; set; }
        
        private string VersionDir { get; }
        
        private string GradleBinary { get; }
        
        private Lazy<Task> EnsureDownloadedTask { get; }

        private async Task EnsureDownloadedAsync()
        {
            if (string.IsNullOrWhiteSpace(Version))
                Version = await VersionProvider.ResolveLatestVersionAsync();

            if (!Directory.Exists(VersionDir))
                Directory.CreateDirectory(VersionDir);

            if (!File.Exists(GradleBinary))
                await VersionProvider.GetVersionAsync(Version, VersionDir, DownloadProgressReporter);
        }

        public async Task ExecuteTaskAsync(string task, params string[] args)
        {
            try
            {
                await EnsureDownloadedTask.Value;

                var process = Process.Start(GradleBinary, new[] {task}.Concat(args));
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                // TODO: error handling
            }
        }
    }
}