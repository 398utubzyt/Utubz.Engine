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
            public GL.ArrayBuffer dbuf;
            public GL.ElementArrayBuffer ibuf;
            public GL.VertexArray aarr;

            public GL.VertexAttribute posattr;
            public GL.VertexAttribute colattr;
            public GL.VertexAttribute texattr;
            //public uint prjunif;
            //public uint viwunif;
            //public uint modunif;
            //public uint mvpunif;
            //public float* model;
            //public float* view;
            //public float* projection;
            //public float* mvp;
            public GL.ShaderUniform mvpunif;

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
                dbuf = new GL.ArrayBuffer();
                dbuf.Set(data, 24);

                aarr = new GL.VertexArray();
                aarr.Bind();

                ibuf = new GL.ElementArrayBuffer();
                ibuf.Set(indices, 6);
            }

            public void InitAttr(Shader shader)
            {
                posattr = GL.VertexAttribute.Find(shader, DefaultVertexPositionAttribute);
                posattr.Set(dbuf, 3, 3, 0);
                posattr.Enable();

                colattr = GL.VertexAttribute.Find(shader, DefaultVertexColorAttribute);
                colattr.Set(dbuf, 4, 0, 12);
                colattr.Enable();

                texattr = GL.VertexAttribute.Find(shader, DefaultVertexTexCoordAttribute);
                texattr.Set(dbuf, 2, 2, 16);
                texattr.Enable();

                mvpunif = GL.ShaderUniform.Find(shader, DefaultMvpUniform);
            }

            public void Draw(Texture tex)
            {
                GL.UseTexture(tex, 0);
                GL.DrawTriangles(dbuf, ibuf, aarr);
            }

            public void SetMvp(TMatrix model, TMatrix view, TMatrix projection)
            {
                mvpunif.Set(projection * view * model);
            }

            public SRData()
            {
                data = (float*)Marshal.AllocHGlobal(sizeof(float) * 24);
                indices = (uint*)Marshal.AllocHGlobal(sizeof(uint) * 6);
            }

            protected override void Clean()
            {
                ibuf.Destroy();
                aarr.Destroy();
                dbuf.Destroy();
                Marshal.FreeHGlobal((IntPtr)indices);
                Marshal.FreeHGlobal((IntPtr)data);
                posattr.Destroy();
                colattr.Destroy();
                texattr.Destroy();
                mvpunif.Destroy();
            }
        }

        private SRData data;
        
        private void UpdateMatrix(Camera cam)
        {
            data.SetMvp(Transform.Transform.LocalToWorld, cam.ViewMatrix, cam.ProjectionMatrix);
        }

        private void SetConstantData()
        {
            // Vertices
            data.SetData(0, 0.5f);
            data.SetData(1, 0.5f * Texture.HwRatio);
            data.SetData(2, 0f);
            data.SetData(3, 0.5f);
            data.SetData(4, -0.5f * Texture.HwRatio);
            data.SetData(5, 0f);
            data.SetData(6, -0.5f);
            data.SetData(7, -0.5f * Texture.HwRatio);
            data.SetData(8, 0f);
            data.SetData(9, -0.5f);
            data.SetData(10, 0.5f * Texture.HwRatio);
            data.SetData(11, 0f);

            // Color
            data.SetData(12, 1.0f);
            data.SetData(13, 1.0f);
            data.SetData(14, 1.0f);
            data.SetData(15, 1.0f);

            // Vertex Indices
            data.SetIndices(0, 0u);
            data.SetIndices(1, 1u);
            data.SetIndices(2, 2u);
            data.SetIndices(3, 2u);
            data.SetIndices(4, 3u);
            data.SetIndices(5, 0u);

            // Texture Coordinates
            data.SetData(16, 1.0f);
            data.SetData(17, 1.0f);
            data.SetData(18, 1.0f);
            data.SetData(19, 0.0f);
            data.SetData(20, 0.0f);
            data.SetData(21, 0.0f);
            data.SetData(22, 0.0f);
            data.SetData(23, 1.0f);
        }

        protected override void Begin(Camera cam)
        {
            if (Null(Shader))
                Shader = Shader.Debug;

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
