using Utubz.Internal.Native;
using Utubz.Internal.Native.Sdl2;
using Utubz.Internal.Native.Glad;
using Utubz.Graphics;

using System;

namespace Utubz.Internal.Platforms.Sdl2
{
    public sealed class Sdl2Platform : Platform
    {
        public override string Name => "SDL2";

        #region Initialization

        public override bool Init()
        {
            return sdl2.SDL_Init(sdl2.SDL_INIT_VIDEO) != 0;
        }

        public override string Error()
        {
            return sdl2.SDL_GetError();
        }

        public override void Quit()
        {
            sdl2.SDL_Quit();
        }

        #endregion

        #region GL

        private static IntPtr PtrGlfwGetProcAddr(string name) => sdl2.SDL_GL_GetProcAddress(name);

        public override unsafe void LoadGL()
        {
            GL.Load(PtrGlfwGetProcAddr);
        }

        #endregion

        #region Events

        public override void Poll()
        {
            while (sdl2.SDL_PollEvent(out sdl2.SDL_Event ev) != 0)
            {
                switch (ev.type)
                {
                    case sdl2.SDL_EventType.SDL_QUIT:
                        Application.Close();
                        break;

                    case sdl2.SDL_EventType.SDL_KEYDOWN:
                        if (Application.Main.windows.TryGetValue((int)ev.key.windowID, out Window win))
                        {
                            
                            win.InputContext.Modify(GetKey((int)ev.key.keysym.scancode), ev.key.state != 0);
                        }
                        break;
                }
            }
        }

        #endregion

        #region Windowing

        public override int GetWindowId(Window window)
        {
            return (int)sdl2.SDL_GetWindowID(window.Pointer);
        }

        protected override Window CreateWindow()
        {
            return new Sdl2Window();
        }

        #endregion

        #region Input

        public override int GetKey(int key)
        {
            
        }

        #endregion
    }
}
