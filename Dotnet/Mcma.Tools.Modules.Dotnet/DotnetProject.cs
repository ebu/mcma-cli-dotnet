using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Mcma.Tools.Modules.Dotnet
{
    public class DotnetProject
    {
        private DotnetProject(XmlDocument xmlDoc, string path)
        {
            XmlDoc = xmlDoc ?? throw new ArgumentNullException(nameof(xmlDoc));
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        private XmlDocument XmlDoc { get; }
        
        public string Path { get; }

        private IEnumerable<XmlElement> GetPropertyGroupElements()
            => XmlDoc.DocumentElement?.GetElementsByTagName("PropertyGroup").OfType<XmlElement>();

        private IEnumerable<XmlElement> GetItemGroupElements()
            => XmlDoc.DocumentElement?.GetElementsByTagName("ItemGroup").OfType<XmlElement>();

        private XmlElement GetVersionElement()
            => GetPropertyGroupElements().Select(x => x.GetElementsByTagName("Version").OfType<XmlElement>().FirstOrDefault())
                                         .FirstOrDefault(x => x != null);

        private IEnumerable<XmlElement> GetPackageReferenceElements()
            => GetItemGroupElements().SelectMany(i => i.GetElementsByTagName("PackageReference").OfType<XmlElement>());

        private IEnumerable<XmlElement> GetMcmaPackageReferenceElements()
            => GetPackageReferenceElements().Where(x => x.GetAttribute("Include").StartsWith("Mcma.", StringComparison.OrdinalIgnoreCase));

        public static DotnetProject Load(string csProjFile)
        {
            var csProjXml = new XmlDocument();
            csProjXml.Load(csProjFile);
            
            if (csProjXml.DocumentElement == null)
                throw new Exception("Invalid .csproj content found at " + csProjFile);

            return new DotnetProject(csProjXml, csProjFile);
        }

        public void SetProjectVersion(Version version)
        {
            var versionElement = GetVersionElement();
            if (versionElement == null)
            {
                var propertyGroupElements = GetPropertyGroupElements().ToArray();
                var firstPropertyGroup = propertyGroupElements.FirstOrDefault();
                var propertyGroupWithTargetFramework =
                    propertyGroupElements.FirstOrDefault(pg => pg.GetElementsByTagName("TargetFramework").OfType<XmlElement>().Any());
                
                var propertyGroupToAddTo = propertyGroupWithTargetFramework ?? firstPropertyGroup;
                if (propertyGroupToAddTo == null)
                {
                    propertyGroupToAddTo = XmlDoc.CreateElement("PropertyGroup");
                    XmlDoc.DocumentElement?.AppendChild(propertyGroupToAddTo);
                }

                versionElement = XmlDoc.CreateElement("Version");
                propertyGroupToAddTo.AppendChild(versionElement);
            }
            
            versionElement.InnerText = version.ToString();

            XmlDoc.Save(Path);
        }

        public void SetVersionOnMcmaPackageReferences(Version version)
        {
            foreach (var mcmaRef in GetMcmaPackageReferenceElements())
                mcmaRef.SetAttribute("Version", version.ToString());

            XmlDoc.Save(Path);
        }

        public static IEnumerable<DotnetProject> GetAllForModuleContext(ModuleContext moduleContext)
            => GetAllInDirectory(moduleContext.RootFolder);

        public static IEnumerable<DotnetProject> GetAllForModuleProviderContext(ModuleProviderContext moduleProviderContext)
            => GetAllInDirectory(moduleProviderContext.ProviderFolder);

        public static IEnumerable<DotnetProject> GetAllInCurrentDirectory()
            => GetAllInDirectory(Directory.GetCurrentDirectory());

        public static IEnumerable<DotnetProject> GetAllInDirectory(string dir)
            => Directory.EnumerateFiles(dir, "*.csproj", SearchOption.AllDirectories).Select(Load);
    }
}