using System;
using System.Runtime.InteropServices;

using Utubz.Internal.Native.Glad;

namespace Utubz.Graphics
{
    /// <summary>
    /// Renders a texture onto a quad.
    /// </summary>
    public sealed unsafe class SpriteRenderer : Renderer
    {
        public Texture Texture { get; set; }

        private class SRData : Object
        {
            public float* data;
            public uint* indices;
            public uint* vao;
            public uint* vbo;
            public uint* ebo;
            public uint posattr;
            public uint colattr;
            public uint texattr;
            public uint prjunif;
            public uint viwunif;
            public uint modunif;
            public float* model;
            public float* view;
            public float* projection;

            public void SetData(int index, float value)
            {
                data[index] = value;
            }
            public void SetIndices(int index, uint vertex)
            {
                indices[index] = vertex;
            }

            public void GenBuffers()
            {
                glad.GLGenBuffers(1, vbo);
                glad.GLBindBuffer(glad.GL_ARRAY_BUFFER, *vbo);
                glad.GLBufferData(glad.GL_ARRAY_BUFFER, 20 * sizeof(float), (IntPtr)data, glad.GL_DYNAMIC_DRAW);

                glad.GLGenVertexArrays(1, vao);
                glad.GLBindVertexArray(*vao);

                glad.GLGenBuffers(1, ebo);
                glad.GLBindBuffer(glad.GL_ELEMENT_ARRAY_BUFFER, *ebo);
                glad.GLBufferData(glad.GL_ELEMENT_ARRAY_BUFFER, 6 * sizeof(uint), (IntPtr)indices, glad.GL_DYNAMIC_DRAW);
            }

            public void InitAttr(Shader shader)
            {
                posattr = glad.GLGetAttribLocation(shader.ShaderId, DefaultVertexPositionAttribute);
                glad.GLVertexAttribPointer(posattr, 2, glad.GL_FLOAT, glad.GL_FALSE, 2 * sizeof(float), (IntPtr)0);
                glad.GLEnableVertexAttribArray(posattr);

                colattr = glad.GLGetAttribLocation(shader.ShaderId, DefaultVertexColorAttribute);
                glad.GLVertexAttribPointer(colattr, 4, glad.GL_FLOAT, glad.GL_FALSE, 0, (IntPtr)(8 * sizeof(float)));
                glad.GLEnableVertexAttribArray(colattr);

                texattr = glad.GLGetAttribLocation(shader.ShaderId, DefaultVertexTexCoordAttribute);
                glad.GLVertexAttribPointer(texattr, 2, glad.GL_FLOAT, glad.GL_FALSE, 2 * sizeof(float), (IntPtr)(12 * sizeof(float)));
                glad.GLEnableVertexAttribArray(texattr);

                modunif = glad.GLGetUniformLocation(shader.ShaderId, DefaultModelUniform);
                viwunif = glad.GLGetUniformLocation(shader.ShaderId, DefaultViewUniform);
                prjunif = glad.GLGetUniformLocation(shader.ShaderId, DefaultProjectionUniform);
            }

            public void Draw(Texture tex)
            {
                glad.GLActiveTexture(glad.GL_TEXTURE0);
                glad.GLBindTexture(glad.GL_TEXTURE_2D, tex.TextureId);
                glad.GLBindBuffer(glad.GL_ARRAY_BUFFER, *vbo);
                glad.GLBufferData(glad.GL_ARRAY_BUFFER, 20 * sizeof(float), (IntPtr)data, glad.GL_DYNAMIC_DRAW);
                glad.GLDrawElements(glad.GL_TRIANGLES, 6, glad.GL_UNSIGNED_INT, (IntPtr)0);
            }

            public void SetMvp(TMatrix model, TMatrix view, TMatrix projection)
            {
                model.ToArrayPtr(this.model);
                glad.GLUniformMatrix4fv(modunif, 1, 0, this.model);
                view.ToArrayPtr(this.view);
                glad.GLUniformMatrix4fv(viwunif, 1, 0, this.view);
                projection.ToArrayPtr(this.projection);
                glad.GLUniformMatrix4fv(prjunif, 1, 0, this.projection);
            }

            public SRData()
            {
                data = (float*)Marshal.AllocHGlobal(sizeof(float) * 20);
                indices = (uint*)Marshal.AllocHGlobal(sizeof(uint) * 6);
                vao = (uint*)Marshal.AllocHGlobal(sizeof(uint));
                vbo = (uint*)Marshal.AllocHGlobal(sizeof(uint));
                ebo = (uint*)Marshal.AllocHGlobal(sizeof(uint));
                model = (float*)Marshal.AllocHGlobal(sizeof(float) * 16);
                view = (float*)Marshal.AllocHGlobal(sizeof(float) * 16);
                projection = (float*)Marshal.AllocHGlobal(sizeof(float) * 16);
                posattr = 0;
                colattr = 0;
                texattr = 0;
                prjunif = 0;
            }

            protected override void Clean()
            {
                glad.GLDeleteBuffers(1, ebo);
                Marshal.FreeHGlobal((IntPtr)ebo);
                glad.GLDeleteBuffers(1, vbo);
                Marshal.FreeHGlobal((IntPtr)vbo);
                glad.GLDeleteVertexArrays(1, vao);
                Marshal.FreeHGlobal((IntPtr)vao);
                Marshal.FreeHGlobal((IntPtr)indices);
                Marshal.FreeHGlobal((IntPtr)data);
                Marshal.FreeHGlobal((IntPtr)projection);
                Marshal.FreeHGlobal((IntPtr)view);
                Marshal.FreeHGlobal((IntPtr)model);
                posattr = 0;
                colattr = 0;
                texattr = 0;
                prjunif = 0;
            }
        }

        private SRData data;

        private void UpdateMatrix(Camera cam)
        {
            data.SetMvp(Transform.Transform.Matrix, cam.ViewMatrix, cam.ProjectionMatrix);
        }

        private void SetConstantData()
        {
            // Vertices
            data.SetData(0, 0.5f);
            data.SetData(1, 0.5f * Texture.HwRatio);
            data.SetData(2, 0.5f);
            data.SetData(3, -0.5f * Texture.HwRatio);
            data.SetData(4, -0.5f);
            data.SetData(5, -0.5f * Texture.HwRatio);
            data.SetData(6, -0.5f);
            data.SetData(7, 0.5f * Texture.HwRatio);

            // Color
            data.SetData(8, 1.0f);
            data.SetData(9, 1.0f);
            data.SetData(10, 1.0f);
            data.SetData(11, 1.0f);

            // Vertex Indices
            data.SetIndices(0, 0u);
            data.SetIndices(1, 1u);
            data.SetIndices(2, 2u);
            data.SetIndices(3, 2u);
            data.SetIndices(4, 3u);
            data.SetIndices(5, 0u);

            // Texture Coordinates
            data.SetData(12, 1.0f);
            data.SetData(13, 1.0f);
            data.SetData(14, 1.0f);
            data.SetData(15, 0.0f);
            data.SetData(16, 0.0f);
            data.SetData(17, 0.0f);
            data.SetData(18, 0.0f);
            data.SetData(19, 1.0f);
        }

        protected override void Begin(Camera cam)
        {
            if (Null(Shader))
                Shader = Shader.Default;

            if (Null(Texture))
                //Texture = Texture.Color(64, 64, Color.White);
                Texture = Texture.FromFile($"{Application.ProcessPath}/resources/graphics/test-npc.png");

            data = new SRData();

            SetConstantData();

            data.GenBuffers();
            data.InitAttr(Shader);
            UpdateMatrix(cam);
        }

        protected override void End()
        {
            data.Destroy();
        }

        protected override void Render(Camera cam)
        {
            UpdateMatrix(cam);
            data.Draw(Texture);
        }
    }
}
