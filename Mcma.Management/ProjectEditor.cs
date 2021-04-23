using System;
using System.Collections.Generic;
using System.Linq;
using Mcma.Management.DataModel;

namespace Mcma.Management
{
    public interface IProjectBuildSystem
    {
        string[] SupportedLanguages { get; }
        
        string DefaultLanguage { get; }
    }
    
    public class ProjectEditor
    {
        public const string DefaultModuleDir = "modules/**";
        
        public ProjectEditor(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            
            Name = name;
        }
        
        public ProjectEditor(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            Name = project.Name;
            ModuleDirs = project.ModuleDirs?.ToList() ?? new List<string>();
            Variables = project.Variables?.ToList() ?? new List<VariableDefinition>();
            Providers = project.Providers?.ToList() ?? new List<ProviderConfiguration>();
        }
        
        public string Name { get; }

        private List<string> ModuleDirs { get; } = new() {DefaultModuleDir};

        private List<VariableDefinition> Variables { get; } = new();

        private List<ProviderConfiguration> Providers { get; } = new();

        public ProjectEditor AddModuleDir(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            if (ModuleDirs.Any(md => md.Equals(path, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Module dir '{path}' already exists in this project.");
            
            ModuleDirs.Add(path);
            return this;
        }

        public ProjectEditor RemoveModuleDir(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            var toRemove = ModuleDirs.FirstOrDefault(md => md.Equals(path, StringComparison.OrdinalIgnoreCase));
            if (toRemove == default)
                throw new Exception($"Module dir '{path}' does not exist in this project.");
            
            ModuleDirs.Remove(toRemove);
            return this;
        }

        public ProjectEditor AddVariable<T>(string name, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            if (Variables.Any(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Variable '{name}' already exists in this project.");
            
            Variables.Add(new VariableDefinition {Name = name, Type = typeof(T).Name, DefaultValue = defaultValue});
            return this;
        }

        public ProjectEditor RemoveVariable(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            var toRemove = Variables.FirstOrDefault(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (toRemove == default)
                throw new Exception($"Variable '{name}' does not exist in this project.");
            
            Variables.Remove(toRemove);
            return this;
        }

        public ProjectEditor AddProvider(string type, string name)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            
            if (Providers.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Provider '{name}' already exists in this project.");

            if (!ProviderRegistry.IsRegistered(type))
                throw new Exception($"Unknown provider type '{type}'");

            Providers.Add(new ProviderConfiguration {Name = name, Type = type, DefaultConfig = ProviderRegistry.GetDefaultConfiguration(type)});
            return this;
        }

        public ProjectEditor RemoveProvider(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            var toRemove = Providers.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (toRemove == default)
                throw new Exception($"Provider '{name}' does not exist in this project.");

            Providers.Remove(toRemove);
            return this;
        }

        public Project Build()
            => new()
            {
                Name = Name,
                ModuleDirs = ModuleDirs.ToArray(),
                Variables = Variables.ToArray(),
                Providers = Providers.ToArray()
            };
    }
}