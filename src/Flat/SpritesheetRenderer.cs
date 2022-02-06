using Utubz.Graphics;

namespace Utubz.Flat
{
    /// <summary>
    /// Renders a sprite from a spritesheet onto a quad.
    /// </summary>
    public sealed unsafe class SpritesheetRenderer : Renderer
    {
        private Spritesheet sheet;
        private int index;

        public Spritesheet Sheet { get { sheet.Retry(); return sheet; } set { sheet = value; sheet.Retry(); } }
        public int Index { get => index; set { index = value; sheet.Retry(); } }
        public Sprite Sprite => Sheet.Get(Index);

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

            if (Null(sheet))
                //Texture = Texture.Color(64, 64, Color.White);
                sheet = new Spritesheet(Texture.FromFile($"{Application.ProcessPath}/resources/graphics/test-sheet.png"), Vector2.Zero, new Vector2(32f, 64f), Vector2.Zero);

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
