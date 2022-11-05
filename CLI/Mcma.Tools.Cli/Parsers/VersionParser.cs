﻿using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace Mcma.Tools.Cli.Parsers;

public class VersionParser : IValueParser
{
    public Type TargetType => typeof(Version);
        
    public object Parse(string argName, string value, CultureInfo culture) => Version.Parse(value);
}