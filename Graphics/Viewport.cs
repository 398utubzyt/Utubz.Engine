using Utubz.Internal.Native.Glad;

namespace Utubz.Graphics
{
    /// <summary>
    /// A rendering target that could be used for rendering to windows, or inside a window.
    /// </summary>
    public sealed class Viewport : Object
    {
        public int x { get; set; }
        public int y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Ratio => (float)Width / Height;
        public float InverseRatio => (float)Height / Width;

        public Transform ToClipSpace(Transform view)
        {
            view.Position.x *= InverseRatio * 0.2f;
            view.Position.y *= 0.2f;
            view.Scale.x *= InverseRatio * 0.2f;
            view.Scale.y *= 0.2f;
            return view;
        }

        public TMatrix ToClipSpaceMatrix(Transform view)
        {
            view.Position.x *= 0.2f;
            view.Position.y *= 0.2f;
            view.Scale.x *= 0.2f;
            view.Scale.y *= 0.2f;
            return view.Matrix;
        }

        public void Resize(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.Width = w;
            this.Height = h;
            glad.GLViewport(x, y, Width, Height);
        }

        public void Clear(Color color)
        {
            glad.GLClearColor(color.r, color.g, color.b, 255);
            glad.GLClearDepth(1.0);

            glad.GLClear(glad.GL_COLOR_BUFFER_BIT | glad.GL_DEPTH_BUFFER_BIT);
        }

        public Viewport(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.Width = w;
            this.Height = h;
        }
    }
}
