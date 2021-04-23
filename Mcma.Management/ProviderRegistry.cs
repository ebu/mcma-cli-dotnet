using System;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Mcma.Management
{
    public static class ProviderRegistry
    {
        private static ConcurrentDictionary<string, JObject> ProvidersAndDefaults { get; } = new(StringComparer.OrdinalIgnoreCase);
        
        public static void Add(string name, JObject defaultConfiguration = null)
            => ProvidersAndDefaults.AddOrUpdate(name, defaultConfiguration, (_, x) => x);

        public static bool IsRegistered(string name)
            => ProvidersAndDefaults.ContainsKey(name);
        
        public static JObject GetDefaultConfiguration(string name)
            => ProvidersAndDefaults.TryGetValue(name, out var value) ? value : throw new Exception($"Provider with name '{name}' not registered.");
    }
}