using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;

namespace Utubz
{
    public static class Debug
    {
        private static DebugWriter writer;
        private static DebugEntryCollection entries;

        public static bool ShowVerbose { get; set; } =
#if DEBUG
            true
#else
            true
#endif
            ;

        public static void Log(string msg)
        {
            LogRaw(msg, DebugLevel.Debug, false);
        }

        public static void LogInfo(string msg)
        {
            LogRaw(msg, DebugLevel.Info, false);
        }

        public static void LogWarn(string msg)
        {
            LogRaw(msg, DebugLevel.Warning, false);
        }

        public static void LogError(string msg)
        {
            LogRaw(msg, DebugLevel.Error, false);
        }

        internal static void LogCritical(string msg)
        {
            LogRaw(msg, DebugLevel.Critical, true);
            Debugger.Break();
            Application.CloseNow();
        }

        public static void Log(Exception exception)
        {
            LogRaw(exception.Message, DebugLevel.Error, true);
        }

        internal static void LogCritical(Exception exception)
        {
            LogRaw(exception.Message, DebugLevel.Critical, true);
            Debugger.Break();
            Application.CloseNow();
        }

        internal static void LogTrace()
        {
            LogRaw(string.Empty, DebugLevel.Trace, true);
        }

        public static void LogVerbose(string msg)
        {
            if (ShowVerbose)
                LogRaw(msg, DebugLevel.Verbose, false);
        }

        internal static void LogEngine(AssemblyName assembly, string glVendor, string glRenderer)
        {
            DebugEntry e = entries.Add($"Utubz Engine v{assembly.Version}\n{glVendor} {glRenderer}", DebugLevel.Engine, false);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Diagnostics.Debug.WriteLine(e.Text);
            Console.WriteLine(e.Text);
            writer.Write(e.Text);
        }

        private static void LogRaw(string msg, DebugLevel level, bool trace)
        {
            DebugEntry e = entries.Add(msg, level, trace);

            Console.ForegroundColor = level switch
            {
                DebugLevel.Debug => ConsoleColor.Cyan,
                DebugLevel.Info => ConsoleColor.Gray,
                DebugLevel.Warning => ConsoleColor.Yellow,
                DebugLevel.Error => ConsoleColor.Red,
                DebugLevel.Critical => ConsoleColor.DarkRed,
                DebugLevel.Trace => ConsoleColor.Blue,
                DebugLevel.Verbose => ConsoleColor.DarkGray,
                DebugLevel.Engine => ConsoleColor.DarkYellow,
                _ => ConsoleColor.DarkGray
            };
            if (trace)
                System.Diagnostics.Debug.WriteLine(e.ToString());
            Console.WriteLine(e.ToString());
            writer.Write(e);
        }

        internal static void Save()
        {
            writer.Close();
        }

        private class DebugWriter : TextWriter
        {
            private bool disposed = false;
            private byte[] newLine = new byte[] { (byte)'\n' };
            public Stream Stream { get; }
            public override Encoding Encoding { get; }

            public void Write(DebugEntry entry)
            {
                Stream.Write(Encoding.GetBytes(entry.ToString()));
                Stream.Write(newLine);
            }

            public override void Write(string entry)
            {
                Stream.Write(Encoding.GetBytes(entry));
                Stream.Write(newLine);
            }

            public override void Flush()
            {
                Stream.Flush();
            }

            public override void Close()
            {
                if (disposed)
                    return;
                disposed = true;
                Stream.Flush();
                Stream.Dispose();
            }

            public DebugWriter()
            {
                if (File.Exists(Application.LogPath))
                    File.Delete(Application.LogPath);

                Stream = File.OpenWrite(Application.LogPath);
                Stream.Position = 0;

                Encoding = Encoding.UTF8;
            }
        }

        private class DebugEntry
        {
            public string Text { get; set;  }
            public DebugLevel Level { get; set; }
            public StackTrace Trace { get; set; }
            public bool ShowTrace { get; set; }

            public void Reuse(string text, DebugLevel level, bool trace = false)
            {
                Text = text;
                Level = level;
                ShowTrace = trace;
                if (trace)
                    Trace = new StackTrace(4);
            }

            public override string ToString()
            {
                if (ShowTrace)
                    return $"[{System.DateTime.Now:HH:mm:ss}-{Level.ToString().ToUpper()}] {Text}\n{Trace}";
                return $"[{System.DateTime.Now:HH:mm:ss}-{Level.ToString().ToUpper()}] {Text}";
            }

            public DebugEntry(string text, DebugLevel level, bool trace = false)
            {
                Text = text;
                Level = level;

                ShowTrace = trace;
                if (trace)
                    Trace = new StackTrace(4);
            }
        }
        
        private class DebugEntryCollection
        {
            private const int MaxEntries = 4;
            public DebugEntry[] Entries { get; }
            public int Start { get; set; }
            public int Count { get; set; }

            public DebugEntry Add(string text, DebugLevel level, bool trace)
            {
                if (Count != Start)
                    Entries[Start].Reuse(text, level, trace);
                else
                    Entries[Start] = new DebugEntry(text, level, trace);
                DebugEntry rtn = Entries[Start];

                if (Count < MaxEntries)
                    Count++;
                Start = Math.Loop(Start + 1, 0, MaxEntries);

                return rtn;
            }

            public DebugEntryCollection()
            {
                Entries = new DebugEntry[MaxEntries];
                Start = 0;
                Count = 0;
            }
        }

        private enum DebugLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical,
            Trace,
            Verbose,
            Engine
        }

        static Debug()
        {
            writer = new DebugWriter();
            entries = new DebugEntryCollection();
        }
    }
}
