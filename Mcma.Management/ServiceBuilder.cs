using System;

namespace Mcma.Management
{
    public class ServiceBuilder
    {
        public ServiceBuilder(string name, string provider, string language, bool hasWorker)
        {
            Name = string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Provider = ProviderRegistry.IsRegistered(provider) ? provider : throw new ArgumentException($"Unknown provider '{provider}'", provider);
            Language = LanguageRegistry.IsRegistered(language) ? language : throw new ArgumentException($"Unknown language '{language}'", language);
            HasWorker = hasWorker;
        }
        
        public string Name { get; }
        
        public string Provider { get; }
        
        public string Language { get; }
        
        public bool HasWorker { get; }
    }
}