using System.Collections.Generic;
using System;
using Utubz.Graphics;

namespace Utubz
{
    public class Scene : Object, IEquatable<Scene>
    {
        public enum SceneMode
        {
            Default,
            Flat
        }

        public static Scene Empty(string name, Color background) => new Scene(name, background);

        private List<Entity> e;
        private List<Renderer> r;
        private List<Camera> c;
        internal Window w;
        internal int bi;

        internal IEnumerable<Renderer> GetRenderers() => r;

        /// <summary>
        /// The name of the <see cref="Scene"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The background clear <see cref="Color"/>.
        /// </summary>
        public Color Background { get; set; }
        /// <summary>
        /// The type of camera to automatically create.
        /// </summary>
        public SceneMode Mode { get; set; }
        public Window Window => w;
        public int Index => bi;

        public Entity GetEntity(int sceneId)
        {
            if (sceneId >= 0 && sceneId < e.Count)
                return e[sceneId];
            return null;
        }

        public Entity GetEntity(string name)
        {
            foreach (Entity e in e)
            {
                if (e.Name.Equals(name))
                    return e;
            }

            return null;
        }

        internal void ReorderEntities()
        {
            e.Sort((a, b) => { return b.order - a.order; });
        }

        public int GetEntityIndex(Entity entity)
        {
            return e.IndexOf(entity);
        }

        internal int Add(Entity entity)
        {
            e.Add(entity);
            return e.Count - 1;
        }

        internal int Add(Renderer renderer)
        {
            r.Add(renderer);
            return r.Count - 1;
        }

        internal int Add(Camera camera)
        {
            c.Add(camera);
            return c.Count - 1;
        }

        internal void Remove(Entity entity)
        {
            e.Remove(entity);
        }

        internal void Remove(Renderer renderer)
        {
            r.Remove(renderer);
        }

        internal void Begin() => Init();

        /// <summary>
        /// Run code in a derived <see cref="Scene"/> object after loading the scene.
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// Run code in a derived <see cref="Scene"/> object during the update loop.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Run code in a derived <see cref="Scene"/> object before it's freed from memory.
        /// </summary>
        protected virtual void Quit() { }

        /// <summary>
        /// Adds an <see cref="Entity"/> with the default constructor.
        /// </summary>
        /// <returns>The instance of the new <see cref="Entity"/>.</returns>
        protected Entity CreateEntity()
        {
            return new Entity(this);
        }
        /// <summary>
        /// Adds an <see cref="Entity"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <returns>The instance of the new <see cref="Entity"/>.</returns>
        protected Entity CreateEntity(string name)
        {
            return new Entity(this, name);
        }
        /// <summary>
        /// Adds an <see cref="Entity"/> with the specified <paramref name="name"/> and provided <paramref name="components"/>.
        /// </summary>
        /// <returns>The instance of the new <see cref="Entity"/>.</returns>
        protected Entity CreateEntity(string name, params Type[] components)
        {
            return new Entity(this, name, components);
        }

        public sealed override bool Equals(object obj)
        {
            if (obj is Scene)
                return Equals((Scene)obj);
            return false;
        }

        public bool Equals(Scene other)
        {
            return other.Id == Id;
        }

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public sealed override string ToString()
        {
            return Name;
        }

        protected sealed override void Clean()
        {
            foreach (Entity e in e)
            {
                e.Destroy();
            }
        }

        internal void Run()
        {
            foreach (Entity e in e)
            {
                e.Run();
            }
        }

        internal void Ren()
        {
            foreach (Camera c in c)
            {
                c.Render();
            }
        }

        public Scene()
        {
            e = new List<Entity>();
            r = new List<Renderer>();
            c = new List<Camera>();
            bi = -1;

            Name = $"Scene ${Id}";
            Background = Color.Black;
        }

        public Scene(string name)
        {
            e = new List<Entity>();
            r = new List<Renderer>();
            c = new List<Camera>();
            bi = -1;

            Name = name;
            Background = Color.Black;
        }

        public Scene(Color background)
        {
            e = new List<Entity>();
            r = new List<Renderer>();
            c = new List<Camera>();
            bi = -1;

            Name = $"Scene ${Id}";
            Background = background;
        }

        public Scene(string name, Color background)
        {
            e = new List<Entity>();
            r = new List<Renderer>();
            c = new List<Camera>();
            bi = -1;

            Name = name;
            Background = background;
        }
    }
}
