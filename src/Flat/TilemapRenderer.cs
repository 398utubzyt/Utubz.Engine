/*
using Utubz.Graphics;

namespace Utubz.Flat
{
    /// <summary>
    /// Renders a sprite from a spritesheet onto a quad.
    /// </summary>
    public sealed unsafe class TilemapRenderer : Renderer
    {
        public Tilemap Tilemap { get; set; }

        private class R2DTilemapContext : Object
        {
            public float[] data;
            public uint[] indices;
            public GL.ArrayBuffer dbuf;
            public GL.ElementArrayBuffer ibuf;
            public GL.VertexArray aarr;
            public GL.VertexAttribute posattr;
            public GL.VertexAttribute colattr;
            public GL.VertexAttribute texattr;
            public GL.ShaderUniform mvpunif;

            public void SetVertices(float v00x, float v00y, float v00z, float v01x, float v01y, float v01z, float v11x, float v11y, float v11z, float v10x, float v10y, float v10z)
            {
                // I feel like there's a better way to do this...
                data[0] = v00x;
                data[1] = v00y;
                data[2] = v00z;
                data[3] = v01x;
                data[4] = v01y;
                data[5] = v01z;
                data[6] = v11x;
                data[7] = v11y;
                data[8] = v11z;
                data[9] = v10x;
                data[10] = v10y;
                data[11] = v10z;

                if (NotNull(dbuf))
                    dbuf.Set(data);

                if (NotNull(posattr))
                    posattr.Set(dbuf, 3, 3, 0);
            }
            public void SetColor(float r, float g, float b, float a)
            {
                data[12] = r;
                data[13] = g;
                data[14] = b;
                data[15] = a;
                data[16] = r;
                data[17] = g;
                data[18] = b;
                data[19] = a;
                data[20] = r;
                data[21] = g;
                data[22] = b;
                data[23] = a;
                data[24] = r;
                data[25] = g;
                data[26] = b;
                data[27] = a;

                if (NotNull(dbuf))
                    dbuf.Set(data);

                if (NotNull(colattr))
                    colattr.Set(dbuf, 4, 4, 12);
            }
            public void SetTexCoords(float v00x, float v00y, float v01x, float v01y, float v11x, float v11y, float v10x, float v10y)
            {
                data[28] = v00x;
                data[29] = v00y;
                data[30] = v01x;
                data[31] = v01y;
                data[32] = v11x;
                data[33] = v11y;
                data[34] = v10x;
                data[35] = v10y;

                if (NotNull(dbuf))
                    dbuf.Set(data);

                if (NotNull(texattr))
                    texattr.Set(dbuf, 2, 2, 28);
            }

            public void SetTexCoords(Rect v)
            {
                data[28] = v.Size.x;
                data[29] = v.Size.y;
                data[30] = v.Size.x;
                data[31] = v.Position.y;
                data[32] = v.Position.x;
                data[33] = v.Position.y;
                data[34] = v.Position.x;
                data[35] = v.Size.y;

                if (NotNull(dbuf))
                    dbuf.Set(data);

                if (NotNull(texattr))
                    texattr.Set(dbuf, 2, 2, 28);
            }

            public void SetIndices(uint i00, uint i01, uint i02, uint i10, uint i11, uint i12)
            {
                indices[0] = i00;
                indices[1] = i01;
                indices[2] = i02;
                indices[3] = i10;
                indices[4] = i11;
                indices[5] = i12;

                if (NotNull(ibuf))
                    ibuf.Set(indices);
            }

            public void GenBuffers()
            {
                dbuf = new GL.ArrayBuffer();
                dbuf.Set(data);

                aarr = new GL.VertexArray();
                aarr.Bind();

                ibuf = new GL.ElementArrayBuffer();
                ibuf.Set(indices);
            }

            public void InitAttr(Shader shader)
            {
                posattr = GL.VertexAttribute.Find(shader, DefaultVertexPositionAttribute);
                posattr.Set(dbuf, 3, 3, 0);

                colattr = GL.VertexAttribute.Find(shader, DefaultVertexColorAttribute);
                colattr.Set(dbuf, 4, 4, 12);

                texattr = GL.VertexAttribute.Find(shader, DefaultVertexTexCoordAttribute);
                texattr.Set(dbuf, 2, 2, 28);

                mvpunif = GL.ShaderUniform.Find(shader, DefaultMvpUniform);

                dbuf.Set(data);
                aarr.Bind();
                ibuf.Set(indices);
            }

            public void Draw(Texture tex)
            {
                GL.UseTexture(tex, 0);
                GL.DrawTriangles(dbuf, ibuf, aarr);
            }

            public void Draw(Sprite spr)
            {
                SetTexCoords(spr.Bounds);
                GL.UseTexture(spr.Texture, 0);
                GL.DrawTriangles(dbuf, ibuf, aarr);
            }

            public void SetMvp(TMatrix model, TMatrix view, TMatrix projection)
            {
                mvpunif.Set(projection * view * model);
            }

            public R2DTilemapContext()
            {
                data = new float[36];
                indices = new uint[6];
            }

            protected override void Clean()
            {
                ibuf.Destroy();
                aarr.Destroy();
                dbuf.Destroy();
                posattr.Destroy();
                colattr.Destroy();
                texattr.Destroy();
                mvpunif.Destroy();
            }
        }

        private R2DTilemapContext data;
        
        private void UpdateMatrix(Camera cam)
        {
            data.SetMvp(Transform.Transform.LocalToWorld, cam.ViewMatrix, cam.ProjectionMatrix);
        }

        private void SetConstantData()
        {
            data.SetVertices(
                0.5f, 0.5f, 0f, 
                0.5f, -0.5f, 0f, 
                -0.5f, -0.5f, 0f, 
                -0.5f, 0.5f, 0f
            );
            data.SetColor(1f, 1f, 1f, 1f);
            data.SetIndices(0u, 1u, 2u, 2u, 3u, 0u);
            data.SetTexCoords(Sprite.Bounds);
        }

        protected override void Begin(Camera cam)
        {
            if (Null(Shader))
                Shader = Shader.Default;

            if (Null(Sprite))
                //Texture = Texture.Color(64, 64, Color.White);
                Sheet = new Spritesheet(Texture.FromFile($"{Application.ProcessPath}/resources/graphics/test-npc.png"), Vector2.Zero, Vector2.One, Vector2.Zero);

            data = new R2DTilemapContext();

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
            data.Draw(Sprite);
        }
    }
}
*/