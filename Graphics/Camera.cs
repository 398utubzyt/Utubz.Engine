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
            ml = Transform.Transform.WorldToLocal;
            if (Orthographic)
                mp.ModifyOrtho(Size, Scene.Window.Viewport.InverseRatio, ClipNear, ClipFar);
            else
                mp.ModifyProjection(Fov, Scene.Window.Viewport.InverseRatio, ClipNear, ClipFar, 1f);
        }

        protected override void Init()
        {
            Fov = 70f;
            ClipNear = 0.1f;
            ClipFar = 100f;
            Size = 5f;
            Orthographic = false;

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
