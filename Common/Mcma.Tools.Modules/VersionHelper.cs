using System;
using System.IO;

namespace Mcma.Tools.Modules
{
    public class VersionHelper
    {
        public static Version FromFile()
        {
            if (!File.Exists("version"))
                return null;
        
            try
            {
                return Version.Parse(File.ReadAllText("version"));
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid version found in local 'version' file", ex);
            }
        }
    }
}