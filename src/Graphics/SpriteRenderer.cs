﻿using Utubz.Graphics;

namespace Utubz.Graphics
{
    /// <summary>
    /// Renders a sprite onto a quad.
    /// </summary>
    public sealed unsafe class SpriteRenderer : Renderer
    {
        public Sprite Sprite { get; set; }
        private R2DContext data;
        
        private void UpdateMatrix(Camera cam)
        {
            data.SetMvp(Transform.Transform.LocalToWorld, cam.ViewMatrix, cam.ProjectionMatrix);
        }

        private void SetConstantData()
        {
            data.SetVertices(
                0.5f, 0.5f * Sprite.Texture.HwRatio, 0f, 
                0.5f, -0.5f * Sprite.Texture.HwRatio, 0f, 
                -0.5f, -0.5f * Sprite.Texture.HwRatio, 0f, 
                -0.5f, 0.5f * Sprite.Texture.HwRatio, 0f
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
                Sprite = new Sprite(Texture.FromFile($"{Application.ProcessPath}/resources/graphics/test-npc.png"), Vector2.Zero, Vector2.One);

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
            data.Draw(Sprite);
        }
    }
}
