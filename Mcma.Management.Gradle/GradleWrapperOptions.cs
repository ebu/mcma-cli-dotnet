using System.IO;

namespace Mcma.Management.Gradle
{
    public class GradleWrapperOptions
    {
        public string Version { get; set; }
        
        public string ProjectRoot { get; set; } = Directory.GetCurrentDirectory();

        public string GradleDir { get; set; } = ".mcma/gradle";
    }
}