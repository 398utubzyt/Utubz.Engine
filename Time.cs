using System;

using Utubz.Async;
using Utubz.Internal.Native.Glfw;

namespace Utubz
{
    /// <summary>
    /// Get data from the <see cref="Component"/>'s associated <see cref="Utubz.Window"/> <see cref="TimeContext"/>, or the asynchronous <see cref="TimeContext"/> that can be used on other threads.
    /// </summary>
    public static class Time
    {
        internal static TimeContext winCtx;
        internal static TimeContext asyncCtx;

        /// <summary>
        /// A single-threaded <see cref="TimeContext"/> that corresponds with the <see cref="Utubz.Window"/> the <see cref="Component"/> is calling from.
        /// </summary>
        public static TimeContext Window => winCtx;
        /// <summary>
        /// An asynchronous <see cref="TimeContext"/> for situations where <see cref="Time.Delta"/> is required on other threads.
        /// </summary>
        public static TimeContext Asynchronous => asyncCtx;

        /// <summary>
        /// The time in seconds since the application started.
        /// </summary>
        public static double Since64 => winCtx.Time64;
        /// <summary>
        /// The time in seconds since the <see cref="Utubz.Window"/>'s <see cref="TimeContext"/> delta was updated.
        /// </summary>
        public static double Delta64 => winCtx.Delta64;
        /// <summary>
        /// The time in seconds since the application started.
        /// </summary>
        public static float Since => winCtx.Time;
        /// <summary>
        /// The time in seconds since the <see cref="Utubz.Window"/>'s <see cref="TimeContext"/> delta was updated.
        /// </summary>
        public static float Delta => winCtx.Delta;

        /// <summary>
        /// The time in seconds since the application started.
        /// </summary>
        public static double Since64Async => asyncCtx.Time64;
        /// <summary>
        /// The time in seconds since the asynchronous delta was updated.
        /// </summary>
        public static double Delta64Async => asyncCtx.Delta64;
        /// <summary>
        /// The time in seconds since the application started.
        /// </summary>
        public static float SinceAsync => asyncCtx.Time;
        /// <summary>
        /// The time in seconds since the asynchronous delta was updated.
        /// </summary>
        public static float DeltaAsync => asyncCtx.Delta;

        /// <summary>
        /// Resets the <see cref="Utubz.Window"/>'s <see cref="TimeContext"/> delta to 0.
        /// </summary>
        public static void Update()
        {
            winCtx.Update();
        }

        /// <summary>
        /// Resets the asynchronous <see cref="TimeContext"/> delta to 0.
        /// </summary>
        public static void UpdateAsync()
        {
            asyncCtx.Update();
        }
    }
}
