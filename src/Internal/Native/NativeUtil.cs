using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Stb.Image;

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
            if (glfw3.GlfwInit() == 0)
            {
                string msg = "";
                int code = glfw3.GlfwGetError(ref msg);
                Throw("glfw", $"{msg} ({code})");
            }

            stb_image.StbiSetFlipVerticallyOnLoad(1);
        }

        public static void QuitNativeLibraries()
        {
            glfw3.GlfwTerminate();
        }
    }
}
