using System;
using System.Runtime.InteropServices;

using Utubz.Internal.Native.Glad;

namespace Utubz.Graphics
{
    public sealed unsafe class RectRenderer : Renderer
    {
        private float* v;
        private int* i;
        private uint vao;
        private uint vbo;
        private uint ebo;
        private uint pa;
        private uint ca;

        public void UpdateTransform(Camera cam)
        {
            Transform t = Scene.Window.Viewport.ToClipSpace(Transform.Transform);
            v[0] = t.Position.x + t.Scale.x * 0.5f;
            v[1] = t.Position.y + t.Scale.y * 0.5f;
            v[5] = t.Position.x + t.Scale.x * 0.5f;
            v[6] = t.Position.y - t.Scale.y * 0.5f;
            v[10] = t.Position.x - t.Scale.x * 0.5f;
            v[11] = t.Position.y - t.Scale.y * 0.5f;
            v[15] = t.Position.x - t.Scale.x * 0.5f;
            v[16] = t.Position.y + t.Scale.y * 0.5f;
        }

        protected override unsafe void Begin(Camera cam)
        {
            if (Null(Shader))
                Shader = Shader.Debug;

            v = (float*)Marshal.AllocHGlobal(sizeof(float) * 20);
            i = (int*)Marshal.AllocHGlobal(sizeof(int) * 6);

            vao = 0;
            vbo = 0;
            ebo = 0;

            UpdateTransform(cam);

            v[2] = 1.0f;
            v[3] = 0.0f;
            v[4] = 0.0f;

            v[7] = 0.0f;
            v[8] = 1.0f;
            v[9] = 0.0f;

            v[12] = 0.0f;
            v[13] = 0.0f;
            v[14] = 1.0f;

            v[17] = 1.0f;
            v[18] = 1.0f;
            v[19] = 1.0f;

            i[0] = 0;
            i[1] = 1;
            i[2] = 2;
            i[3] = 2;
            i[4] = 3;
            i[5] = 0;

            uint _vbo = 0;
            glad.GLGenBuffers(1, &_vbo);
            vbo = _vbo;
            glad.GLBindBuffer(glad.GL_ARRAY_BUFFER, vbo);
            glad.GLBufferData(glad.GL_ARRAY_BUFFER, 20 * sizeof(float), (IntPtr)v, glad.GL_DYNAMIC_DRAW);

            uint _vao = 0;
            glad.GLGenVertexArrays(1, &_vao);
            vao = _vao;
            glad.GLBindVertexArray(vao);

            uint _ebo = 0;
            glad.GLGenBuffers(1, &_ebo);
            ebo = _ebo;
            glad.GLBindBuffer(glad.GL_ELEMENT_ARRAY_BUFFER, ebo);
            glad.GLBufferData(glad.GL_ELEMENT_ARRAY_BUFFER, 6 * sizeof(int), (IntPtr)i, glad.GL_DYNAMIC_DRAW);

            pa = glad.GLGetAttribLocation(Shader.ShaderId, "position");
            glad.GLVertexAttribPointer(pa, 2, glad.GL_FLOAT, glad.GL_FALSE, 5 * sizeof(float), (IntPtr)0);
            glad.GLEnableVertexAttribArray(pa);

            ca = glad.GLGetAttribLocation(Shader.ShaderId, "color");
            glad.GLVertexAttribPointer(ca, 3, glad.GL_FLOAT, glad.GL_FALSE, 5 * sizeof(float), (IntPtr)(2 * sizeof(float)));
            glad.GLEnableVertexAttribArray(ca);
        }

        protected override void End()
        {
            Marshal.FreeHGlobal((IntPtr)v);
            Marshal.FreeHGlobal((IntPtr)i);
        }

        protected override void Render(Camera cam)
        {
            UpdateTransform(cam);
            glad.GLBufferData(glad.GL_ARRAY_BUFFER, 20 * sizeof(float), (IntPtr)v, glad.GL_DYNAMIC_DRAW);
            glad.GLDrawElements(glad.GL_TRIANGLES, 6, glad.GL_UNSIGNED_INT, (IntPtr)0);
        }
    }
}
