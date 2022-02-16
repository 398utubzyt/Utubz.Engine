using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Stb.Image;
using Utubz.Internal.Native.Bass;

using System;

namespace Utubz.Internal.Native
{
    internal static class NativeUtil
    {
        private class InitializationFailureException : Exception
        {
            public InitializationFailureException(string lib, string msg) : base($"The library '{lib}' was unable to initialize (and therefore the application is unable to run) because '{msg}'.")
            {

            }
        }

        private static void Throw(string lib, string msg)
        {
            throw new InitializationFailureException(lib, msg);
        }

        public static unsafe void InitNativeLibraries()
        {
            stb_image.StbiSetFlipVerticallyOnLoad(1);

            if (bass.BASS_Init(-1, 44100, 0x4000, IntPtr.Zero, IntPtr.Zero) == 0)
                Throw("bass", "Initialization error");
        }

        public static void QuitNativeLibraries()
        {
            bass.BASS_Stop();
            bass.BASS_Free();
            glfw3.GlfwTerminate();
        }
    }
}
