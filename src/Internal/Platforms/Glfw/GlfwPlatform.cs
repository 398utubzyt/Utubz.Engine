using Utubz.Internal.Native;
using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Glad;
using Utubz.Graphics;

using System;

namespace Utubz.Internal.Platforms.Glfw
{
    public sealed class GlfwPlatform : Platform
    {
        public override string Name => "GLFW";

        #region GL

        private static IntPtr PtrGlfwGetProcAddr(string name) => System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(glfw3.GlfwGetProcAddress(name));

        public override unsafe void LoadGL()
        {
            GL.Load(PtrGlfwGetProcAddr);
        }

        #endregion

        #region Events

        public override void Poll()
        {
            glfw3.GlfwPollEvents();
        }

        #endregion

        #region Windowing

        protected override Window CreateWindow()
        {
            return new GlfwWindow();
        }

        #endregion
    }
}
