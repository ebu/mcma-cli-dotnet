using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace Mcma.Tools.Cli.Parsers;

public class ProviderParser : IValueParser
{
    public Type TargetType => typeof(Provider);

    public object Parse(string argName, string value, CultureInfo culture) => new Provider(value);
}