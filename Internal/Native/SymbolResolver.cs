using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Utubz.Internal.Native
{
    /// <summary>
    /// From CppSharp by Mono: <see href="https://github.com/mono/CppSharp/tree/main/src/Runtime"/>
    /// </summary>
    internal static class SymbolResolver
    {
        private static readonly string[] formats;
        private static readonly Func<string, IntPtr> loadImage;
        private static readonly Func<IntPtr, string, IntPtr> resolveSymbol;
        private const int RTLD_LAZY = 1;

        static SymbolResolver()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    SymbolResolver.loadImage = new Func<string, IntPtr>(SymbolResolver.dlopen);
                    SymbolResolver.resolveSymbol = new Func<IntPtr, string, IntPtr>(SymbolResolver.dlsym);
                    SymbolResolver.formats = new string[6]
                    {
            "{0}",
            "{0}.so",
            "{0}.dylib",
            "lib{0}.so",
            "lib{0}.dylib",
            "{0}.bundle"
                    };
                    break;
                default:
                    SymbolResolver.loadImage = new Func<string, IntPtr>(SymbolResolver.LoadLibrary);
                    SymbolResolver.resolveSymbol = new Func<IntPtr, string, IntPtr>(SymbolResolver.GetProcAddress);
                    SymbolResolver.formats = new string[2]
                    {
            "{0}",
            "{0}.dll"
                    };
                    break;
            }
        }

        public static IntPtr LoadImage(ref string name)
        {
            string environmentVariable = Environment.GetEnvironmentVariable("PATH");
            string[] strArray;
            if (environmentVariable != null)
                strArray = environmentVariable.Split(Path.PathSeparator);
            else
                strArray = new string[0];
            List<string> stringList = new List<string>((IEnumerable<string>)strArray);
            stringList.Insert(0, Directory.GetCurrentDirectory());
            stringList.Insert(0, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            foreach (string format in SymbolResolver.formats)
            {
                string path2 = string.Format(format, (object)name);
                string path1 = (string)null;
                foreach (string path1_1 in stringList)
                {
                    string path3 = Path.Combine(path1_1, path2);
                    if (File.Exists(path3))
                    {
                        path1 = path3;
                        break;
                    }
                }
                if (File.Exists(path1))
                {
                    IntPtr num = SymbolResolver.loadImage(path1);
                    if (!(num == IntPtr.Zero))
                    {
                        name = path1;
                        return num;
                    }
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr ResolveSymbol(string name, string symbol) => SymbolResolver.ResolveSymbol(SymbolResolver.LoadImage(ref name), symbol);

        public static IntPtr ResolveSymbol(IntPtr image, string symbol) => image != IntPtr.Zero ? SymbolResolver.resolveSymbol(image, symbol) : IntPtr.Zero;

        private static IntPtr dlopen(string path) => SymbolResolver.dlopen(path, 1);

        [DllImport("dl", CharSet = CharSet.Ansi)]
        private static extern IntPtr dlopen(string path, int flags);

        [DllImport("dl", CharSet = CharSet.Ansi)]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
