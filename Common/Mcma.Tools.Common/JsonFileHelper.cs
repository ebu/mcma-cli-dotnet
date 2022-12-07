using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools;

public static class JsonFileHelper
{
    public static JObject GetJsonObjectFromFile(string path)
    {
        if (!File.Exists(path))
            throw new Exception($"File not found at {path}");

        var text = File.ReadAllText(path);
        try
        {
            return JObject.Parse(text);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to parse json from {path}", ex);
        }
    }
}