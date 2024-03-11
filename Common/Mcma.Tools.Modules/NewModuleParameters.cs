namespace Mcma.Tools.Modules;

public class NewModuleParameters
{
    public NewModuleParameters(string? parentDirectory,
                               string @namespace,
                               string name,
                               IEnumerable<NewProviderModuleParameters>? providers,
                               string? jobType = null,
                               string? displayName = null,
                               string? description = null)
    {
        ParentDirectory = parentDirectory ?? Directory.GetCurrentDirectory();
        (NamespaceInPascalCase, NamespaceInKebabCase) =
            @namespace is not null ? NormalizeCasing(@namespace) : throw new ArgumentNullException(nameof(@namespace));
        (NameInPascalCase, NameInKebabCase) = name is not null ? NormalizeCasing(name) : throw new ArgumentNullException(nameof(name));
        Providers = providers?.ToArray() ?? [];
            
        JobType = jobType;
        DisplayName = displayName ?? NameInPascalCase;
        Description = description ?? DisplayName;
            
        ModuleDirectory = Path.Combine(ParentDirectory, $"{NamespaceInKebabCase}-mcma-module-{NameInKebabCase}");
    }

    public string ParentDirectory { get; }

    public string NamespaceInPascalCase { get; }
        
    public string NamespaceInKebabCase { get; }
        
    public string NameInPascalCase { get; }
        
    public string NameInKebabCase { get; }
        
    public NewProviderModuleParameters[] Providers { get; }
        
    public string? JobType { get; }

    public string? DisplayName { get; }
        
    public string? Description { get; }
        
    public string ModuleDirectory { get; }
        
    private static (string pascalCase, string kebabCase) NormalizeCasing(string name)
    {
        string kebabCase;
        string pascalCase;
        if (name.Contains("-"))
        {
            kebabCase = name.ToLower();
            pascalCase = kebabCase.KebabCaseToPascalCase();
        }
        else if (name.Contains("_"))
        {
            kebabCase = name.ToLower().Replace("_", "-");
            pascalCase = kebabCase.KebabCaseToPascalCase();
        }
        else if (char.IsUpper(name[0]))
        {
            pascalCase = name;
            kebabCase = pascalCase.PascalCaseToKebabCase();
        }
        else
        {
            pascalCase = name.CamelCaseToPascalCase();
            kebabCase = pascalCase.PascalCaseToKebabCase();
        }

        return (pascalCase, kebabCase);
    }

    public string GetProviderDir(Provider provider) => Path.Combine(ModuleDirectory, provider.ToString().ToLower());

    public string GetProviderSrcDir(Provider provider) => Path.Combine(GetProviderDir(provider), "src");
}