using Utubz.Internal;

using System.IO;
using System;

namespace Utubz
{
    public delegate bool ApplicationClosingHandle();
    public delegate void ApplicationClosedHandle();

    public static class Application
    {
        internal static ProcessLoop Main;
        private static string baseDir = Path.GetDirectoryName(Environment.ProcessPath);

        public static string ProcessPath => baseDir;
        public static string LogPath => $"{baseDir}/ulog.txt";

        /// <summary>
        /// Returns the combined path relative to <see cref="ProcessPath"/>.
        /// </summary>
        /// <param name="path">The path to combine.</param>
        /// <returns>The combined path.</returns>
        public static string RelativePath(string path)
        {
            return Path.Combine(baseDir, path);
        }

        public static bool IsMainWindow(Window window)
        {
            return MainWindow == window;
        }

        public static Window MainWindow { get { Main.windows.TryPeek(out Window r); return r; } }

        public static void Close()
        {
            Main?.Stop();
        }

        internal static void CloseNow()
        {
            Main?.StopImmediate();
        }
    }
}
