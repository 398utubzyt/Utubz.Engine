using Utubz.Internal.Native;
using Utubz.Internal.Native.Sdl2;
using Utubz.Internal.Native.Glad;
using Utubz.Graphics;
using Utubz.Async;

using System;

namespace Utubz.Internal.Platforms.Sdl2
{
    public sealed class Sdl2Window : Window
    {
        #region Private Fields

        private IntPtr win;
        private IntPtr ctx;
        private int b_x;
        private int b_y;
        private int b_w;
        private int b_h;
        private string title;
        private bool vsync;
        private bool minside;
        private int mmode;

        #endregion

        #region Properties

        /// <summary>
        /// Not the native pointer to the <see cref="Window"/> (like HWND on Windows), but an internal one used by the <see cref="Internal.Platforms.Platform"/>.
        /// </summary>
        public override IntPtr Pointer { get => win; }
        /// <summary>
        /// The title of the <see cref="Sdl2Window"/>.
        /// </summary>
        public override string Title { get { return title; } set { title = value; sdl2.SDL_GetWindowTitle(win); } }
        /// <summary>
        /// The x position of the <see cref="Sdl2Window"/> in screen space.
        /// </summary>
        public override int x { get { return b_x; } set { b_x = value; sdl2.SDL_SetWindowPosition(win, value, b_y); } }
        /// <summary>
        /// The y position of the <see cref="Sdl2Window"/> in screen space.
        /// </summary>
        public override int y { get { return b_y; } set { b_y = value; sdl2.SDL_SetWindowPosition(win, b_x, value); } }
        /// <summary>
        /// The width of the <see cref="Sdl2Window"/> in screen space.
        /// </summary>
        public override int Width { get { return b_w; } set { b_w = value; sdl2.SDL_SetWindowSize(win, value, b_h); } }
        /// <summary>
        /// The height of the <see cref="Sdl2Window"/> in screen space.
        /// </summary>
        public override int Height { get { return b_h; } set { b_h = value; sdl2.SDL_SetWindowSize(win, b_w, value); } }
        /// <summary>
        /// Enables/disables vertical-sync for the <see cref="Sdl2Window"/>.
        /// </summary>
        public override bool Vsync { get { return vsync; } set { vsync = value; sdl2.SDL_GL_SetSwapInterval(vsync ? 1 : 0); } }
        /// <summary>
        /// Gets if the mouse cursor is hovering over the <see cref="Sdl2Window"/>.
        /// </summary>
        public override bool CursorHovering { get { return minside; } }
        /// <summary>
        /// Determines if the cursor will be hidden when it enters the <see cref="Sdl2Window"/>.
        /// </summary>
        public override bool HideCursor { get { return mmode != 0; } set { mmode = value ? 1 : 0; sdl2.SDL_ShowCursor(value ? 1 : 0); } }
        /// <summary>
        /// Determines if the cursor will be hidden and confined to the <see cref="Sdl2Window"/> bounds upon entering.
        /// </summary>
        public override bool RestrictCursor { get { return mmode == 2; } set { mmode = value ? 1 : 0; sdl2.SDL_ShowCursor(value ? 1 : 0); } }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Converts global screen coordinates to window pixel coordinates.
        /// </summary>
        /// <param name="scr">The screen coordinates.</param>
        /// <returns>The relative window coordinates.</returns>
        public override Vector2 ScreenToWindow(Vector2 scr)
        {
            scr.x -= b_x;
            scr.y -= b_y;

            return scr;
        }

        #endregion

        #region Callbacks

        private void OnClose(IntPtr ptr)
        {
            if (Id == -1 || IsMain)
                Application.Main.Stop();
            Destroy();
        }

        private void OnKey(IntPtr ptr, int key, int scan, int act, int mod)
        {
            InputContext.Change(key, act);
            Input.asyncCtx.Change(key, act);
        }

        private void OnCursor(IntPtr ptr, double x, double y)
        {
            InputContext.Change(x, y);
            Input.asyncCtx.Change(x, y);
        }

        private void OnCursorEnter(IntPtr ptr, int entered)
        {
            minside = entered == 1;
        }

        private void OnScroll(IntPtr ptr, double x, double y)
        {
            InputContext.Scroll(x, y);
        }

        private void OnSize(IntPtr ptr, int width, int height)
        {
            b_w = width;
            b_h = height;
            Viewport.Resize(0, 0, b_w, b_h);
            Application.Main.RequestRefresh();
        }

        private void OnPos(IntPtr ptr, int x, int y)
        {
            b_x = x;
            b_y = y;
            Application.Main.RequestRefresh();
        }

        private void OnRefresh(IntPtr ptr)
        {
            
        }

        #endregion

        #region Render Context

        protected override void ActivateRenderContext()
        {
            sdl2.SDL_GL_MakeCurrent(win, ctx);
        }

        protected override void DeactivateRenderContext()
        {
            sdl2.SDL_GL_MakeCurrent(win, IntPtr.Zero);
        }

        protected override void SwapRenderBuffers()
        {
            sdl2.SDL_GL_SwapWindow(win);
        }

        #endregion

        #region Initialization

        protected override void Initialize(string title, int x, int y, int width, int height, bool vsync)
        {
            CreateSelf(x, y, width, height, title, vsync);
        }

        protected override void Quit()
        {
            sdl2.SDL_GL_DeleteContext(ctx);
            sdl2.SDL_DestroyWindow(win);
        }

        private void CreateSelf(int x, int y, int width, int height, string title, bool vsync)
        {
            win = sdl2.SDL_CreateWindow(title, x, y, width, height, sdl2.SDL_WindowFlags.SDL_WINDOW_OPENGL);

            this.title = title;
            b_x = x;
            b_y = y;
            b_w = width;
            b_h = height;

            ctx = sdl2.SDL_GL_CreateContext(win);

            ActivateRenderContext();
            Platform.LoadGL();
            sdl2.SDL_GL_SetSwapInterval(vsync ? 1 : 0);
        }

        #endregion
    }
}
