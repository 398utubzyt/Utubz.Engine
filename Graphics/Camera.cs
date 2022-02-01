namespace Utubz.Graphics
{
    public sealed class Camera : Component
    {
        private TMatrix mp;
        private TMatrix ml;

        public bool Orthographic { get; set; }
        public float Fov { get; set; }
        public float ClipNear { get; set; }
        public float ClipFar { get; set; }
        public float Size { get; set; }

        internal TMatrix ProjectionMatrix { get { return mp; } }
        internal TMatrix ViewMatrix { get { return ml; } }
        internal TMatrix CombinedMatrix { get => mp * ml; }

        internal void PrepareForRender()
        {
            ml = TMatrix.Rotation(-Transform.Rotation) * TMatrix.Translation(-Transform.Position);
            if (Orthographic)
                mp.ModifyOrthographic(Size, Scene.Window.Viewport.Ratio, ClipNear, ClipFar);
            else
                mp.ModifyPerspective(Fov, Scene.Window.Viewport.Ratio, ClipNear, ClipFar, 1f);
        }

        protected override void Init()
        {
            Fov = 70f;
            ClipNear = 0.1f;
            ClipFar = 100f;
            Size = 5f;
            Orthographic = Scene.Mode == Scene.SceneMode.Flat;
            
            ml = TMatrix.Identity;
            mp = TMatrix.OrthographicIdentity;
            Scene.Add(this);
        }

        public void Render()
        {
            PrepareForRender();
            foreach (Renderer r in Scene.GetRenderers())
            {
                r.Ren(this);
            }
        }
    }
}
