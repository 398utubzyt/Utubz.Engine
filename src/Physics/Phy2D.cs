using System.Collections.Generic;

namespace Utubz.Physics
{
    public static class Phy2D
    {
        public const int LAYER_MAX = 16;
        private static List<Collider2D>[] colliders;

        internal static void Init()
        {
            colliders = new List<Collider2D>[LAYER_MAX];
            for (int i = 0; i < LAYER_MAX; i++)
            {
                colliders[i] = new List<Collider2D>();
            }
        }

        internal static void Quit()
        {
            for (int i = 0; i < LAYER_MAX; i++)
            {
                colliders[i].Clear();
            }
        }

        internal static void Register(Collider2D c, int layer)
        {
            if (colliders[layer].Contains(c))
                return;

            colliders[layer].Add(c);
        }

        internal static void Unregister(Collider2D c)
        {
            for (int i = 0; i < LAYER_MAX; i++)
            {
                if (colliders[i].Contains(c))
                    colliders[i].Remove(c);
            }
        }

        public static bool RectCheck(Rect rect, int layer = 0)
            => RectCheck(rect.Position, rect.Size, layer);

        public static bool RectCheck(Vector2 position, Vector2 size, int layer = 0)
        {
            bool hit = false;
            foreach (Collider2D c in colliders[layer])
            {
                hit |= 
                    (c.Transform.Position.x + c.Offset.x) + (c.Size.x * c.Transform.Scale.x) < position.x + size.x &&
                    (c.Transform.Position.x + c.Offset.x) - (c.Size.x * c.Transform.Scale.x) > position.x - size.x &&
                    (c.Transform.Position.y + c.Offset.y) + (c.Size.y * c.Transform.Scale.y) < position.y + size.y &&
                    (c.Transform.Position.y + c.Offset.y) - (c.Size.y * c.Transform.Scale.y) > position.y - size.y;
            }
            return hit;
        }
    }
}
