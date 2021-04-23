using System;
using System.Collections.Generic;
using System.Linq;

namespace Mcma.Management
{
    public static class LanguageRegistry
    {
        private static List<string> Languages { get; } = new();

        private static object Lock { get; } = new();

        public static void Add(string name)
        {
            lock (Lock)
                Languages.Add(name);
        }

        public static bool IsRegistered(string name)
        {
            lock (Lock)
                return Languages.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}