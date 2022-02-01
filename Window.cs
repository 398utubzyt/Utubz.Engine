using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native;
using Utubz.Graphics;
using Utubz.Async;

using System;

namespace Utubz
{
    public sealed class Window : Object, IEquatable<Window>
    {
        private static bool loadedGl;

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

        private InputContext input;
        private TimeContext time;
        private Viewport view;
        private Scene scene;

        private bool inuse;
        private bool ctxm;
        private bool isctx;

        #endregion

        #region Properties

        /// <summary>
        /// The title of the <see cref="Window"/>.
        /// </summary>
        public string Title { get { return title; } set { title = value; glfw3.GlfwSetWindowTitle(win, value); } }
        /// <summary>
        /// The x position of the <see cref="Window"/> in screen space.
        /// </summary>
        public int x { get { return b_x; } set { b_x = value; glfw3.GlfwSetWindowPos(win, value, b_y); } }
        /// <summary>
        /// The y position of the <see cref="Window"/> in screen space.
        /// </summary>
        public int y { get { return b_y; } set { b_y = value; glfw3.GlfwSetWindowPos(win, b_x, value); } }
        /// <summary>
        /// The width of the <see cref="Window"/> in screen space.
        /// </summary>
        public int Width { get { return b_w; } set { b_w = value; glfw3.GlfwSetWindowSize(win, value, b_h); } }
        /// <summary>
        /// The height of the <see cref="Window"/> in screen space.
        /// </summary>
        public int Height { get { return b_h; } set { b_h = value; glfw3.GlfwSetWindowSize(win, b_w, value); } }
        /// <summary>
        /// Enables/disables vertical-sync for the <see cref="Window"/>.
        /// </summary>
        public bool Vsync { get { return vsync; } set { vsync = value; glfw3.GlfwSwapInterval(vsync ? 1 : 0); } }
        /// <summary>
        /// Gets if the mouse cursor is hovering over the <see cref="Window"/>.
        /// </summary>
        public bool CursorHovering { get { return minside; } }
        /// <summary>
        /// Determines if the cursor will be hidden when it enters the <see cref="Window"/>.
        /// </summary>
        public bool HideCursor { get { return mmode != 0; } set { mmode = value ? 1 : 0; glfw3.GlfwSetInputMode(win, 0x00033001, value ? 0x00034002 : 0x00034001); } }
        /// <summary>
        /// Determines if the cursor will be hidden and confined to the <see cref="Window"/> bounds upon entering.
        /// </summary>
        public bool RestrictCursor { get { return mmode == 2; } set { mmode = value ? 1 : 0; glfw3.GlfwSetInputMode(win, 0x00033001, value ? 0x00034003 : 0x00034001); } }

        /// <summary>
        /// Gets if this <see cref="Window"/> is the first <see cref="Window"/> created.
        /// </summary>
        public bool IsMain => Application.IsMainWindow(this);
        /// <summary>
        /// The <see cref="Graphics.Viewport"/> of the <see cref="Window"/>.
        /// </summary>
        public Viewport Viewport => view;
        /// <summary>
        /// The current loaded <see cref="Utubz.Scene"/> of the <see cref="Window"/>.
        /// </summary>
        public Scene Scene => scene;

        #endregion

        #region Utility Functions

        /// <summary>
        /// Converts global screen coordinates to window pixel coordinates.
        /// </summary>
        /// <param name="scr">The screen coordinates.</param>
        /// <returns>The relative window coordinates.</returns>
        public Vector2 ScreenToWindow(Vector2 scr)
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

        protected override void Clean()
        {
            glfw3.GlfwDestroyWindow(win);
            time.Destroy();
            input.Destroy();
        }

        private void OnClose(IntPtr ptr)
        {
            while (inuse) ;

            if (Id == -1 || IsMain)
                Application.Main.Stop();
            Destroy();
        }

        private void OnKey(IntPtr ptr, int key, int scan, int act, int mod)
        {
            input.Change(key, act);
            Input.asyncCtx.Change(key, act);
        }

        private void OnCursor(IntPtr ptr, double x, double y)
        {
            input.Change(x, y);
            Input.asyncCtx.Change(x, y);
        }

        private void OnCursorEnter(IntPtr ptr, int entered)
        {
            minside = entered == 1;
        }

        private void OnScroll(IntPtr ptr, double x, double y)
        {
            input.Scroll(x, y);
        }

        private void OnSize(IntPtr ptr, int width, int height)
        {
            b_w = width;
            b_h = height;
            view.Resize(0, 0, b_w, b_h);
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

        #region Game Loop

        internal void Update()
        {
            time.Update();
            Time.winCtx = time;

            input.Update();
            Input.winCtx = input;

            scene.Run();
        }

        internal void Render()
        {
            while (ctxm || inuse) ;

            if (isctx)
                glfw3.GlfwMakeContextCurrent(null);

            inuse = true;
            isctx = true;

            if (Id != -1)
                glfw3.GlfwMakeContextCurrent(win);

            if (Id != -1)
                view.Resize(0, 0, b_w, b_h);
            if (Id != -1)
                view.Clear(scene.Background);
            if (Id != -1)
                scene.Ren();

            if (Id != -1)
                glfw3.GlfwSwapBuffers(win);

            isctx = false;
            inuse = false;
        }

        #endregion

        #region OpenGL Context

        private void SetGLContext(Window window)
        {
            while (inuse || ctxm) ;
            window.isctx = true;
            ctxm = true;
            glfw3.GlfwMakeContextCurrent(window.win);
            ctxm = false;
        }

        private void RemoveGLContext(Window window)
        {
            while (inuse || ctxm) ;

            window.isctx = false;
            ctxm = true;
            glfw3.GlfwMakeContextCurrent(null);
            ctxm = false;
        }

        #endregion

        #region System.Object Overrides

        public static bool operator ==(Window a, Window b) => a.win.__Instance == b.win.__Instance;
        public static bool operator !=(Window a, Window b) => a.win.__Instance != b.win.__Instance;

        public bool Equals(Window window)
        {
            return window == this;
        }

        public override bool Equals(object obj)
        {
            if (obj is Window)
                return Equals((Window)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return win.__Instance.GetHashCode();
        }

        #endregion

        #region GLFW Stuff

        private static IntPtr PtrGlfwGetProcAddr(string name) => System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(glfw3.GlfwGetProcAddress(name));

        private class GLLoadingException : Exception
        {
            public GLLoadingException() : base("Unable to load GL for some reason.")
            {

            }
        }

        #endregion

        #region Scene Loading

        public void LoadScene(Scene scene)
        {
            if (NotNull(this.scene))
                this.scene.Destroy();

            this.scene = scene;
            this.scene.w = this;
            this.scene.Begin();
        }

        public void LoadScene(Type scene)
            => LoadScene((Scene)Activator.CreateInstance(scene));

        public void LoadScene<T>() where T : Scene
            => LoadScene(Activator.CreateInstance<T>());

        #endregion

        #region Initialization

        public Window(string title, int width, int height, bool vsync)
        {
            Setup(title, width, height, vsync, Scene.Empty("Empty", Color.Black));

            Application.Main.windows.Add(this);
        }

        public Window(string title, int width, int height, bool vsync, Scene scene)
        {
            Setup(title, width, height, vsync, scene);

            Application.Main.windows.Add(this);
        }

        public Window(string title, int width, int height, bool vsync, Type scene)
        {
            Setup(title, width, height, vsync, (Scene)Activator.CreateInstance(scene));

            Application.Main.windows.Add(this);
        }

        private unsafe void Setup(string title, int width, int height, bool vsync, Scene scene)
        {
            CreateSelf(width, height, title, vsync);
            CreateCallbacks();
            LoadScene(scene);
        }

        private void CreateSelf(int width, int height, string title, bool vsync)
        {
            win = glfw3.GlfwCreateWindow(width, height, title, null, null);

            this.title = title;
            glfw3.GlfwGetWindowPos(win, ref b_x, ref b_y);
            b_w = width;
            b_h = height;

            SetGLContext(this);
            LoadGl();
            glfw3.GlfwSwapInterval(vsync ? 1 : 0);
        }

        private unsafe void LoadGl()
        {
            if (loadedGl)
                return;

            loadedGl = true;

            if (glad.GladLoadGLLoader(PtrGlfwGetProcAddr) == 0)
            {
                throw new GLLoadingException();
            }

            glad.GLEnable(glad.GL_BLEND);
            glad.GLEnable(glad.GL_DEPTH_TEST);
            glad.GLBlendFunc(glad.GL_SRC_ALPHA, glad.GL_ONE_MINUS_SRC_ALPHA);

            Debug.LogEngine(
                System.Reflection.Assembly.GetExecutingAssembly().GetName(), 
                (string)UTF8Marshaller.marshaler.MarshalNativeToManaged((IntPtr)glad.GLGetString(glad.GL_VENDOR)),
                (string)UTF8Marshaller.marshaler.MarshalNativeToManaged((IntPtr)glad.GLGetString(glad.GL_RENDERER))
            );

            Shader.Default.Equals(this);
            Shader.Debug.Equals(this);
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

            input = new InputContext((int)Key.Max);
            time = new TimeContext();

            view = new Viewport(0, 0, b_w, b_h);
        }

        #endregion
    }
}
