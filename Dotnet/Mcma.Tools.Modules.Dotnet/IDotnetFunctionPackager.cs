using Mcma.Tools.Modules.Packaging;

namespace Mcma.Tools.Modules.Dotnet;

public interface IDotnetFunctionPackager : IFunctionPackager
{
    string Type { get; }
}