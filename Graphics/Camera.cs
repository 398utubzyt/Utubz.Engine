namespace Utubz.Graphics
{
    public sealed class Camera : Component
    {
        private Vector3 data;
        private TMatrix mp;
        private TMatrix ml;
        private bool ortho;

        public bool Orthographic { get => ortho; set => ortho = value; }
        public float Fov { get => data.x; set { data.x = value; } }
        public float ClipNear { get => data.y; set { data.y = value; } }
        public float ClipFar { get => data.z; set { data.z = value; } }
        public float Size { get => Transform.Scale.Average(); set => Transform.Scale = Vector3.One * value; }

        internal TMatrix ProjectionMatrix { get { return mp; } }
        internal TMatrix ViewMatrix { get { return ml; } }
        public TMatrix LookAtMatrix(Transform transform) { ml.ModifyLookAt(Transform.Position, transform.Position, Vector3.Up); return ml; }

        internal void PrepareForRender()
        {
            ml.ModifyView(Transform.Transform);
            mp.ModifyProjection(data.x, Scene.Window.Viewport.Ratio, data.y, data.z, 0.2f);
        }

        protected override void Init()
        {
            data = new Vector3(70f, 0.3f, 100f);
            ml = TMatrix.Identity;
            mp = new TMatrix(data.x, Scene.Window.Viewport.Ratio, data.y, data.z, 0.2f);
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
