using Utubz.Internal.Platforms.Glfw;

namespace Utubz.Internal.Platforms
{
    public abstract class Platform
    {
        public static Platform Default { get; } = new GlfwPlatform();
        public abstract string Name { get; }

        #region GL

        public abstract void LoadGL();

        #endregion

        #region Events

        public abstract void Poll();

        #endregion

        #region Windowing

        public Window CreateWindow(string title, int x, int y, int width, int height, bool vsync, Scene scene)
            => CreateWindow().Setup(title, x, y, width, height, vsync, scene, this);
        protected abstract Window CreateWindow();

        #endregion
    }
}
