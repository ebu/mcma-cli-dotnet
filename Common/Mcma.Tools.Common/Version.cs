using System.ComponentModel;
using System.Globalization;

namespace Mcma.Tools;

[TypeConverter(typeof(VersionConverter))]
public class Version
{
    public Version(int major, int minor, int patch, PreReleaseStage? preReleaseStage = null, int? preReleaseNumber = null)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        PreReleaseStage = preReleaseStage;
        PreReleaseNumber = preReleaseNumber;
    }

    public int Major { get; }

    public int Minor { get; }

    public int Patch { get; }

    public PreReleaseStage? PreReleaseStage { get; }

    public int? PreReleaseNumber { get; }

    private static readonly char[] Separator = ['.'];

    public static implicit operator string(Version version) => version.ToString();

    public static Version Parse(string version)
    {
        ArgumentNullException.ThrowIfNull(version);

        var releaseParts = version.Split(["-"], StringSplitOptions.RemoveEmptyEntries);
        var preReleaseLabel = releaseParts.ElementAtOrDefault(1);
        version = releaseParts[0];

        var versionParts = version.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        if (versionParts.Length != 3)
            throw new ArgumentException($"Invalid semantic version '{version}'. Must contain 3 parts delimited by periods.", nameof(version));

        if (!int.TryParse(versionParts[0], out var major))
            throw new ArgumentException($"Invalid semantic version '{version}'. Major value '{versionParts[0]}' must be an integer value.", nameof(version));
        if (!int.TryParse(versionParts[1], out var minor))
            throw new ArgumentException($"Invalid semantic version '{version}'. Minor value '{versionParts[1]}' must be an integer value.", nameof(version));
        if (!int.TryParse(versionParts[2], out var patch))
            throw new ArgumentException($"Invalid semantic version '{version}'. Patch value '{versionParts[2]}' must be an integer value.", nameof(version));

        if (preReleaseLabel == null)
            return new Version(major, minor, patch);

        PreReleaseStage? preReleaseStage = null;

        string? preReleaseNumberText = null;
        if (preReleaseLabel.StartsWith(Tools.PreReleaseStage.Alpha.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            preReleaseStage = Tools.PreReleaseStage.Alpha;
            preReleaseNumberText = preReleaseLabel.Substring(Tools.PreReleaseStage.Alpha.ToString().Length);
        }
        else if (preReleaseLabel.StartsWith(Tools.PreReleaseStage.Beta.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            preReleaseStage = Tools.PreReleaseStage.Beta;
            preReleaseNumberText = preReleaseLabel.Substring(Tools.PreReleaseStage.Beta.ToString().Length);
        }
        else if (preReleaseLabel.StartsWith(Tools.PreReleaseStage.RC.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            preReleaseStage = Tools.PreReleaseStage.RC;
            preReleaseNumberText = preReleaseLabel.Substring(Tools.PreReleaseStage.RC.ToString().Length);
        }

        if (!preReleaseStage.HasValue)
            throw new ArgumentException($"Invalid semantic version '{version}'. Pre-release label '{preReleaseLabel}' does not start with a known pre-release stage ('alpha', 'beta', or 'rc').", nameof(version));

        if (!int.TryParse(preReleaseNumberText, out var preReleaseNumber))
            throw new ArgumentException($"Invalid semantic version '{version}'. Pre-release number '{preReleaseNumberText}' must be an integer value.", nameof(version));

        return new Version(major, minor, patch, preReleaseStage, preReleaseNumber);
    }

    public static Version Initial() => new(0, 0, 1, Tools.PreReleaseStage.Alpha, 1);

    public Version Next()
        => PreReleaseStage.HasValue
               ? new Version(Major, Minor, Patch, PreReleaseStage, PreReleaseNumber + 1)
               : new Version(Major, Minor, Patch + 1);

    public override string ToString()
        =>
            $"{Major}.{Minor}.{Patch}{(PreReleaseStage.HasValue ? $"-{PreReleaseStage.Value.ToString().ToLower()}{PreReleaseNumber}" : string.Empty)}";

    public class VersionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            => destinationType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            => value is string str ? Parse(str) : null;

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            => value is Version version ? version.ToString() : null;
    }
}