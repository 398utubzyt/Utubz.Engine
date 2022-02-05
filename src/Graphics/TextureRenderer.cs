using System;
using System.Runtime.InteropServices;

using Utubz.Internal.Native.Glad;

namespace Utubz.Graphics
{
    /// <summary>
    /// Renders a texture onto a quad.
    /// </summary>
    public sealed unsafe class TextureRenderer : Renderer
    {
        public Texture Texture { get; set; }

        private R2DContext data;
        
        private void UpdateMatrix(Camera cam)
        {
            data.SetMvp(Transform.Transform.LocalToWorld, cam.ViewMatrix, cam.ProjectionMatrix);
        }

        private void SetConstantData()
        {
            data.SetVertices(
                0.5f, 0.5f * Texture.HwRatio, 0f,
                0.5f, -0.5f * Texture.HwRatio, 0f,
                -0.5f, -0.5f * Texture.HwRatio, 0f,
                -0.5f, 0.5f * Texture.HwRatio, 0f
            );
            data.SetColor(1f, 1f, 1f, 1f);
            data.SetIndices(0u, 1u, 2u, 2u, 3u, 0u);
            data.SetTexCoords(1f, 1f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
        }

        protected override void Begin(Camera cam)
        {
            if (Null(Shader))
                Shader = Shader.Default;

            if (Null(Texture))
                //Texture = Texture.Color(64, 64, Color.White);
                Texture = Texture.FromFile($"{Application.ProcessPath}/resources/graphics/test-npc.png");

            data = new R2DContext();

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
