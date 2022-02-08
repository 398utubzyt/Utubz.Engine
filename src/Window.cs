using Utubz.Internal.Platforms;
using Utubz.Graphics;
using Utubz.Async;

using System;

namespace Utubz
{
    public abstract class Window : Object, IEquatable<Window>
    {
        #region Private Fields

        private Platform platform;
        private InputContext input;
        private TimeContext time;
        private Viewport view;
        private Scene scene;
        private Scene queuescene;

        private bool loadscene;

        #endregion

        #region Properties

        /// <summary>
        /// The title of the <see cref="Window"/>.
        /// </summary>
        public abstract string Title { get; set; }
        /// <summary>
        /// The x position of the <see cref="Window"/> in screen space.
        /// </summary>
        public abstract int x { get; set; }
        /// <summary>
        /// The y position of the <see cref="Window"/> in screen space.
        /// </summary>
        public abstract int y { get; set; }
        /// <summary>
        /// The width of the <see cref="Window"/> in screen space.
        /// </summary>
        public abstract int Width { get; set; }
        /// <summary>
        /// The height of the <see cref="Window"/> in screen space.
        /// </summary>
        public abstract int Height { get; set; }
        /// <summary>
        /// Enables/disables vertical-sync for the <see cref="Window"/>.
        /// </summary>
        public abstract bool Vsync { get; set; }
        /// <summary>
        /// Gets if the mouse cursor is hovering over the <see cref="Window"/>.
        /// </summary>
        public abstract bool CursorHovering { get; }
        /// <summary>
        /// Determines if the cursor will be hidden when it enters the <see cref="Window"/>.
        /// </summary>
        public abstract bool HideCursor { get; set; }
        /// <summary>
        /// Determines if the cursor will be hidden and confined to the <see cref="Window"/> bounds upon entering.
        /// </summary>
        public abstract bool RestrictCursor { get; set; }

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
        public Platform Platform => platform;
        public InputContext InputContext => input;
        public TimeContext TimeContext => time;

        #endregion

        #region Utility Functions

        /// <summary>
        /// Converts global screen coordinates to window pixel coordinates.
        /// </summary>
        /// <param name="scr">The screen coordinates.</param>
        /// <returns>The relative window coordinates.</returns>
        public abstract Vector2 ScreenToWindow(Vector2 scr);

        #endregion

        #region Game Loop

        internal void Update()
        {
            if (Id != -1)
            {
                if (loadscene)
                    FollowUpSceneLoad(queuescene);

                time.Update();
                Time.winCtx = time;

                input.Update();
                Input.winCtx = input;

                scene.Run();
            }
        }

        internal void Render()
        {
            if (Id != -1)
            {
                ActivateRenderContext();

                view.Resize(0, 0, Width, Height);
                view.Clear(scene.Background);
                scene.Ren();

                SwapRenderBuffers();
            }
        }

        #endregion

        #region Render Context

        protected abstract void ActivateRenderContext();

        protected abstract void DeactivateRenderContext();

        protected abstract void SwapRenderBuffers();

        #endregion

        #region Overrides

        protected sealed override void Clean()
        {
            time.Destroy();
            input.Destroy();

            Quit();
        }

        public static bool operator ==(Window a, Window b) => a.Id == b.Id;
        public static bool operator !=(Window a, Window b) => a.Id != b.Id;

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
            return Id.GetHashCode();
        }

        #endregion

        #region Scene Loading

        private void FollowUpSceneLoad(Scene scene)
        {
            loadscene = false;

            if (Null(scene))
                return;

            if (NotNull(this.scene))
                this.scene.Destroy();

            this.scene = scene;
            this.scene.w = this;
            this.scene.Begin();
        }

        public void LoadScene(Scene scene)
        {
            queuescene = scene;
            loadscene = true;
        }

        public void LoadScene(Type scene)
            => LoadScene((Scene)Activator.CreateInstance(scene));

        public void LoadScene<T>() where T : Scene
            => LoadScene(Activator.CreateInstance<T>());

        #endregion

        #region Construction

        protected abstract void Initialize(string title, int x, int y, int width, int height, bool vsync);
        protected abstract void Quit();

        internal Window Setup(string title, int x, int y, int width, int height, bool vsync, Scene scene, Platform platform)
        {
            this.platform = platform;

            input = new InputContext((int)Key.Max);
            time = new TimeContext();

            view = new Viewport(0, 0, Width, Height);

            Application.Main.windows.Add(this);

            Initialize(title, x, y, width, height, vsync);

            if (NotNull(scene))
                LoadScene(scene);
            else
                LoadScene(Scene.Empty("null", Color.Black));

            return this;
        }

        public static Window Create(string title, int x, int y, int width, int height, bool vsync, Scene scene)
        {
            return Platform.Default.CreateWindow(title, x, y, width, height, vsync, scene);
        }

        public static Window Create(string title, int x, int y, int width, int height, bool vsync, Type scene)
        {
            return Platform.Default.CreateWindow(title, x, y, width, height, vsync, scene == null ? null : (Scene)Activator.CreateInstance(scene));
        }

        public static Window Create<T>(string title, int x, int y, int width, int height, bool vsync) where T : Scene
        {
            return Platform.Default.CreateWindow(title, x, y, width, height, vsync, Activator.CreateInstance<T>());
        }

        protected Window()
        {
        }

        public Window(string title, int width, int height, bool vsync, Scene scene)
        {

        }

        #endregion
    }
}
