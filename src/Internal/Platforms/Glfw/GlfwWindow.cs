using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native;
using Utubz.Graphics;
using Utubz.Async;

using System;

namespace Utubz.Internal.Platforms.Glfw
{
    public sealed class GlfwWindow : Window
    {
        #region Private Fields

        private GLFWwindow win;
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
        /// The title of the <see cref="GlfwWindow"/>.
        /// </summary>
        public override string Title { get { return title; } set { title = value; glfw3.GlfwSetWindowTitle(win, value); } }
        /// <summary>
        /// The x position of the <see cref="GlfwWindow"/> in screen space.
        /// </summary>
        public override int x { get { return b_x; } set { b_x = value; glfw3.GlfwSetWindowPos(win, value, b_y); } }
        /// <summary>
        /// The y position of the <see cref="GlfwWindow"/> in screen space.
        /// </summary>
        public override int y { get { return b_y; } set { b_y = value; glfw3.GlfwSetWindowPos(win, b_x, value); } }
        /// <summary>
        /// The width of the <see cref="GlfwWindow"/> in screen space.
        /// </summary>
        public override int Width { get { return b_w; } set { b_w = value; glfw3.GlfwSetWindowSize(win, value, b_h); } }
        /// <summary>
        /// The height of the <see cref="GlfwWindow"/> in screen space.
        /// </summary>
        public override int Height { get { return b_h; } set { b_h = value; glfw3.GlfwSetWindowSize(win, b_w, value); } }
        /// <summary>
        /// Enables/disables vertical-sync for the <see cref="GlfwWindow"/>.
        /// </summary>
        public override bool Vsync { get { return vsync; } set { vsync = value; glfw3.GlfwSwapInterval(vsync ? 1 : 0); } }
        /// <summary>
        /// Gets if the mouse cursor is hovering over the <see cref="GlfwWindow"/>.
        /// </summary>
        public override bool CursorHovering { get { return minside; } }
        /// <summary>
        /// Determines if the cursor will be hidden when it enters the <see cref="GlfwWindow"/>.
        /// </summary>
        public override bool HideCursor { get { return mmode != 0; } set { mmode = value ? 1 : 0; glfw3.GlfwSetInputMode(win, 0x00033001, value ? 0x00034002 : 0x00034001); } }
        /// <summary>
        /// Determines if the cursor will be hidden and confined to the <see cref="GlfwWindow"/> bounds upon entering.
        /// </summary>
        public override bool RestrictCursor { get { return mmode == 2; } set { mmode = value ? 1 : 0; glfw3.GlfwSetInputMode(win, 0x00033001, value ? 0x00034003 : 0x00034001); } }

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

        #region Callback Delagates (to avoid GC)

        private GLFWwindowclosefun dClose;
        private GLFWkeyfun dKey;
        private GLFWcursorposfun dCursor;
        private GLFWcursorenterfun dCursorEnter;
        private GLFWscrollfun dScroll;
        private GLFWwindowsizefun dSize;
        private GLFWwindowposfun dPos;
        private GLFWwindowrefreshfun dRefresh;

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
            glfw3.GlfwMakeContextCurrent(win);
        }

        protected override void DeactivateRenderContext()
        {
            glfw3.GlfwMakeContextCurrent(null);
        }

        protected override void SwapRenderBuffers()
        {
            glfw3.GlfwSwapBuffers(win);
        }

        #endregion

        #region Initialization

        protected override void Initialize(string title, int x, int y, int width, int height, bool vsync)
        {
            CreateSelf(width, height, title, vsync);
            CreateCallbacks();
        }

        protected override void Quit()
        {
            glfw3.GlfwDestroyWindow(win);
        }

        private void CreateSelf(int width, int height, string title, bool vsync)
        {
            win = glfw3.GlfwCreateWindow(width, height, title, null, null);

            this.title = title;
            glfw3.GlfwGetWindowPos(win, ref b_x, ref b_y);
            b_w = width;
            b_h = height;

            ActivateRenderContext();
            Platform.LoadGL();
            glfw3.GlfwSwapInterval(vsync ? 1 : 0);
        }

        private void CreateCallbacks()
        {
            dClose = OnClose;
            dKey = OnKey;
            dCursor = OnCursor;
            dCursorEnter = OnCursorEnter;
            dScroll = OnScroll;
            dSize = OnSize;
            dPos = OnPos;
            dRefresh = OnRefresh;

            glfw3.GlfwSetWindowCloseCallback(win, dClose);
            glfw3.GlfwSetKeyCallback(win, dKey);
            glfw3.GlfwSetCursorPosCallback(win, dCursor);
            glfw3.GlfwSetCursorEnterCallback(win, dCursorEnter);
            glfw3.GlfwSetScrollCallback(win, dScroll);
            glfw3.GlfwSetWindowSizeCallback(win, dSize);
            glfw3.GlfwSetWindowPosCallback(win, dPos);
            glfw3.GlfwSetWindowRefreshCallback(win, dRefresh);
            glfw3.GlfwFocusWindow(win);
        }

        #endregion
    }
}
