using System.ComponentModel;
using System.Globalization;

namespace Mcma.Tools;

[TypeConverter(typeof(ProviderConverter))]
public class Provider
{
    public Provider(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        
        Name = Match(name)?.Name ?? name;
    }
        
    public string Name { get; }

    public static Provider AWS { get; } = new(nameof(AWS));

    public static Provider Azure { get; } = new(nameof(Azure));

    public static Provider Google { get; } = new(nameof(Google));

    public static Provider Kubernetes { get; } = new(nameof(Kubernetes));

    public static bool operator ==(Provider? provider1, string? provider2)
        => provider2 is null && provider1 is null ||
           provider1 is not null && provider1.Equals(provider2);

    public static bool operator !=(Provider? provider1, string? provider2)
        => !(provider1 == provider2);

    public static bool operator ==(string? provider1, Provider? provider2)
        => provider2 == provider1;

    public static bool operator !=(string? provider1, Provider? provider2)
        => !(provider2 == provider1);

    public static bool operator ==(Provider? provider1, Provider? provider2)
        => (provider1 is null && provider2 is null) ||
           (provider1 is not null && provider2 is not null && provider1.Equals(provider2));

    public static bool operator !=(Provider? provider1, Provider? provider2)
        => !(provider1 == provider2);

    public static implicit operator string(Provider provider) => provider.Name;

    public static implicit operator Provider(string provider) => new(provider);

    public override bool Equals(object? obj)
        => obj is string provider1 && Equals(provider1) || obj is Provider provider2 && Equals(provider2);

    protected bool Equals(string? other) => Name.Equals(other, StringComparison.OrdinalIgnoreCase);
        
    protected bool Equals(Provider other) => Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => Name;

    public static Provider? Match(string provider)
        => provider switch
        {
            var x when x == AWS => AWS,
            var x when x == Azure => Azure,
            var x when x == Google => Google,
            var x when x == Kubernetes => Kubernetes,
            _ => null
        };

    public static bool IsSupported(string provider)
        => Match(provider) is not null;
        
    public class ProviderConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext ?context, Type sourceType)
            => sourceType == typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            => destinationType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            => value is string str ? new Provider(str) : null;

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            => value is Provider provider ? provider.ToString() : null;
    }
}