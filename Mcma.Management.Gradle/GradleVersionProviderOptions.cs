namespace Mcma.Management.Gradle
{
    public class GradleVersionProviderOptions
    {
        public string DownloadUrl { get; set; } = "https://services.gradle.org/distributions/";

        public string DownloadLinksXPath { get; set; } = "//ul[@class='items']/li/a[contains(@href, 'bin.zip')]";

        public string DownloadLinkVersionRegex { get; set; } = "gradle-(\\d+\\.\\d+(?:\\.\\d+)?-bin.zip";
    }
}