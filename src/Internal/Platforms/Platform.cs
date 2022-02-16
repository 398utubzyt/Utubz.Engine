using Utubz.Internal.Platforms.Glfw;
using Utubz.Internal.Platforms.Sdl2;

namespace Utubz.Internal.Platforms
{
    public abstract class Platform
    {
        public static Platform Default { get; } = new Sdl2Platform();
        public abstract string Name { get; }

        #region Initialization

        public abstract bool Init();

        public abstract string Error();

        public abstract void Quit();

        #endregion

        #region GL

        public abstract void LoadGL();

        #endregion

        #region Events

        public abstract void Poll();

        #endregion

        #region Windowing

        public abstract int GetWindowId(Window window);

        public Window CreateWindow(string title, int x, int y, int width, int height, bool vsync, Scene scene)
            => CreateWindow().Setup(title, x, y, width, height, vsync, scene, this);
        protected abstract Window CreateWindow();

        #endregion

        #region Input

        public abstract int GetKey(int key);

        #endregion
    }
}
