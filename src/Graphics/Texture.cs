using Utubz.Internal.Native.Stb.Image;
using Utubz.Internal.Native.Glad;

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utubz.Graphics
{
    public sealed unsafe class Texture : Object
    {
        private int n;
        private Vector2 s;
        private byte* data;
        private uint t;

        public static unsafe Texture Color(int width, int height, Color color)
        {
            if (width <= 0 || height <= 0)
                return null;

            Texture t = new Texture();

            t.s = new Vector2(width, height);

            t.data = (byte*)Marshal.AllocHGlobal(width * height * 4);

            for (int i = 0; i < width * height * 4; i += 4)
            {
                t.data[i    ] = (byte)color.r8;
                t.data[i + 1] = (byte)color.g8;
                t.data[i + 2] = (byte)color.b8;
                t.data[i + 3] = (byte)color.a8;
            }

            t.Bind();
            Marshal.FreeHGlobal((IntPtr)t.data);

            return t;
        }

        public uint TextureId => t;
        public Vector2 Size => s;
        public int Width => (int)s.x;
        public int Height => (int)s.y;
        public int Depth => n;
        public float HwRatio => Height / Width;

        public static Texture FromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            Texture t = new Texture();

            int _w = 0, _h = 0;
            t.data = stb_image.StbiLoad(path, ref _w, ref _h, ref t.n, 4);
            t.s = new Vector2(_w, _h);
            t.Bind();
            stb_image.StbiImageFree((IntPtr)t.data);

            return t;
        }

        protected override void Clean()
        {
            
        }

        private unsafe void Bind()
        {
            uint ts = 0;
            glad.GLGenTextures(1, &ts);
            t = ts;
            glad.GLBindTexture(glad.GL_TEXTURE_2D, t);

            glad.GLTexParameteri(glad.GL_TEXTURE_2D, glad.GL_TEXTURE_WRAP_S, glad.GL_REPEAT);
            glad.GLTexParameteri(glad.GL_TEXTURE_2D, glad.GL_TEXTURE_WRAP_T, glad.GL_REPEAT);
            glad.GLTexParameteri(glad.GL_TEXTURE_2D, glad.GL_TEXTURE_MIN_FILTER, glad.GL_NEAREST_MIPMAP_NEAREST);
            glad.GLTexParameteri(glad.GL_TEXTURE_2D, glad.GL_TEXTURE_MAG_FILTER, glad.GL_NEAREST);
            glad.GLTexImage2D(glad.GL_TEXTURE_2D, 0, glad.GL_RGBA, (int)s.x, (int)s.y, 0, glad.GL_RGBA, glad.GL_UNSIGNED_BYTE, (IntPtr)data);
            glad.GLGenerateMipmap(glad.GL_TEXTURE_2D);
        }

        private Texture()
        {
            s = Vector2.Zero;
            n = 0;
            data = null;
        }
    }
}
