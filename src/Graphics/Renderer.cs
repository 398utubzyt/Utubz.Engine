using Utubz.Internal.Native.Glad;

namespace Utubz.Graphics
{
    public abstract class Renderer : Component
    {
        private Shader shd;
        private bool _s;

        public int Depth;

        internal const string DefaultVertexPositionAttribute = "vPosition";
        internal const string DefaultVertexColorAttribute = "vColor";
        internal const string DefaultVertexTexCoordAttribute = "vTexCoord";
        internal const string DefaultFragmentColorAttribute = "uColor";
        internal const string DefaultFragmentTexCoordAttribute = "uTexCoord";
        internal const string DefaultOutputColorAttribute = "fColor";
        internal const string DefaultModelUniform = "tModel";
        internal const string DefaultViewUniform = "tView";
        internal const string DefaultProjectionUniform = "tProjection";
        internal const string DefaultMvpUniform = "mvp";

        public Shader Shader 
        { 
            get => shd;
            set => shd = value;
        }
        protected Viewport Viewport => Entity.Scene.Window.Viewport;

        protected sealed override unsafe void Init()
        {
            if (Null(Shader))
                Shader = Shader.Default;

            Scene.Add(this);
        }


        protected sealed override void Start() { }
        protected sealed override void Update() { }

        protected sealed override void Quit()
        {
            End();
            Scene.Remove(this);
            Shader.Destroy();
        }

        protected virtual void Begin(Camera camera) { }
        protected virtual void End() { }

        protected void Use(Shader shader)
        {
            glad.GLUseProgram(shader.ShaderId);
        }

        protected virtual void Render(Camera camera) { }
        internal void Ren(Camera camera)
        {
            if (!_s)
            {
                _s = true;
                Begin(camera);
            }

            Use(shd);
            Render(camera);
        }
    }
}
