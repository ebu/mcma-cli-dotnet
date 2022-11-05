﻿using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Packaging;

public interface IFunctionPackager
{
    string Type { get; }
        
    Task PackageAsync(ModuleProviderContext moduleProviderContext, FunctionInfo functionInfo);
}