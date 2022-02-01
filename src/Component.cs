using System.Collections.Generic;
using System;

namespace Utubz
{
    public abstract class Component : Object
    {
        private bool _s;

        /// <summary>
        /// The <see cref="Utubz.Entity"/> the <see cref="Component"/> is attached to.
        /// </summary>
        public Entity Entity { get; }
        /// <summary>
        /// The <see cref="Utubz.Scene"/> the <see cref="Utubz.Entity"/> of the <see cref="Component"/> is located in.
        /// </summary>
        public Scene Scene => Entity.Scene;
        /// <summary>
        /// The state of the <see cref="Component"/>; <see langword="true"/> if it will update automatically, otherwise <see langword="false"/>.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The <see cref="Utubz.Transform"/> of the <see cref="Utubz.Entity"/> the <see cref="Component"/> is attached to.
        /// </summary>
        public EntityTransform Transform => Entity.Transform;

        /// <summary>
        /// Called during construction of the <see cref="Component"/>.
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// Called right before the first <see cref="Update"/>.
        /// </summary>
        protected virtual void Start() { }
        /// <summary>
        /// Runs every "<see cref="Component"/> frame". For running code during on the rendering thread, use <see cref="Graphic.Renderer"/>. 
        /// </summary>
        protected virtual void Update() { }
        /// <summary>
        /// Called when the <see cref="Component"/> is about to be destroyed.
        /// </summary>
        protected virtual void Quit() { }

        internal void Run()
        {
            if (!Enabled)
                return;

            if (!_s)
            {
                _s = true;
                Start();
            }

            Update();
        }

        /// <summary>
        /// Gets the first <see cref="Component"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to look for.</typeparam>
        /// <returns>A <see cref="Component"/> of type <typeparamref name="T"/>.</returns>
        public T GetComponent<T>() where T : Component
            => Entity.GetComponent<T>();

        /// <summary>
        /// Gets the first <see cref="Component"/> of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Component"/> to look for.</param>
        /// <returns>A <see cref="Component"/> of type <paramref name="type"/>.</returns>
        public Component GetComponent(Type type)
            => Entity.GetComponent(type);

        /// <summary>
        /// Adds a <see cref="Component"/> with the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to add.</typeparam>
        /// <returns>The newly added <see cref="Component"/> of type <typeparamref name="T"/>.</returns>
        public void AddComponent<T>() where T : Component
            => Entity.AddComponent<T>();

        /// <summary>
        /// Adds a <see cref="Component"/> with the type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Component"/> to add.</param>
        /// <returns>The newly added <see cref="Component"/> of type <paramref name="type"/>.</returns>
        public void AddComponent(Type type)
            => Entity.AddComponent(type);

        /// <summary>
        /// Removes the <see cref="Component"/> <paramref name="c"/> from the <see cref="Entity"/>.
        /// </summary>
        /// <param name="c">The <see cref="Component"/> to remove.</param>
        public void RemoveComponent(Component c)
            => Entity.RemoveComponent(c);

        protected sealed override void Clean()
        {
            Quit();
            Entity.RemoveComponent(this);
        }

        public Component(Entity entity)
        {
            Entity = entity;
            Enabled = true;

            Init();
        }

        /// <summary>
        /// This only exists to avoid CS1729. Please avoid using this.
        /// </summary>
        protected Component()
        {
            Entity = Entity.currentAddTo;
            Enabled = true;

            Init();
        }
    }
}
