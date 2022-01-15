using System;
using System.Collections.Generic;

using Utubz;

namespace Utubz
{
    public class Entity : Object
    {
        internal static Entity currentAddTo;
        private List<Component> com;

        /// <summary>
        /// The <see cref="Utubz.Scene"/> in which the <see cref="Entity"/> is located in.
        /// </summary>
        public Scene Scene { get; }
        /// <summary>
        /// The <see cref="Entity"/>'s <see cref="Utubz.Scene"/> index which can be used to find it later.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// The name of the <see cref="Entity"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The state of the <see cref="Entity"/>; <see langword="true"/> if it will update automatically, otherwise <see langword="false"/>.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// The <see cref="Utubz.Transform"/> of the <see cref="Entity"/>.
        /// </summary>
        public EntityTransform Transform { get; }

        /// <summary>
        /// Gets the first <see cref="Component"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to look for.</typeparam>
        /// <returns>A <see cref="Component"/> of type <typeparamref name="T"/>.</returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (Component c in com)
            {
                if (c is T)
                    return (T)c;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Component"/> of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Component"/> to look for.</param>
        /// <returns>A <see cref="Component"/> of type <paramref name="type"/>.</returns>
        public Component GetComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(Component)))
            {
                Debug.LogWarn("You cannot get a type that is not a Utubz.Component from an Entity.");
                return null;
            }

            foreach (Component c in com)
            {
                if (type.IsInstanceOfType(c))
                    return c;
            }

            return null;
        }

        /// <summary>
        /// Adds a <see cref="Component"/> with the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to add.</typeparam>
        /// <returns>The newly added <see cref="Component"/> of type <typeparamref name="T"/>.</returns>
        public T AddComponent<T>() where T : Component
        {
            currentAddTo = this;
            T c = Activator.CreateInstance<T>();
            com.Add(c);
            return c;
        }

        /// <summary>
        /// Adds a <see cref="Component"/> with the type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Component"/> to add.</param>
        /// <returns>The newly added <see cref="Component"/> of type <paramref name="type"/>.</returns>
        public Component AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(Component)))
            {
                Debug.LogWarn("You cannot add a type that is not a Utubz.Component to an Entity.");
                return null;
            }

            currentAddTo = this;
            Component c = (Component)Activator.CreateInstance(type);
            com.Add(c);
            return c;
        }

        /// <summary>
        /// Removes the <see cref="Component"/> <paramref name="c"/> from the <see cref="Entity"/>.
        /// </summary>
        /// <param name="c">The <see cref="Component"/> to remove.</param>
        public void RemoveComponent(Component c)
        {
            if (com.Contains(c))
            {
                com.Remove(c);
                c.Destroy();
            }
        }

        internal void Run()
        {
            foreach (Component c in com)
            {
                c.Run();
            }
        }

        protected override void Clean()
        {
            foreach (Component c in com)
            {
                c.Destroy();
            }
        }

        public Entity(Scene scene)
        {
            com = new List<Component>();

            Scene = scene;
            Name = $"Entity #{Id}";
            Enabled = true;
            Transform = new EntityTransform(this);

            Index = scene.Add(this);
        }

        public Entity(Scene scene, string name)
        {
            com = new List<Component>();

            Scene = scene;
            Name = name;
            Enabled = true;
            Transform = new EntityTransform(this);

            Index = scene.Add(this);
        }

        public Entity(Scene scene, string name, params Type[] components)
        {
            com = new List<Component>();

            Scene = scene;
            Name = name;
            Enabled = true;
            Transform = new EntityTransform(this);

            Index = scene.Add(this);

            foreach (Type t in components)
            {
                AddComponent(t);
            }
        }
    }
}
