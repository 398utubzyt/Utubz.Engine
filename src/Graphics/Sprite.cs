namespace Utubz.Graphics
{
    public sealed class Sprite : Object
    {
        public Texture Texture { get; }
        public Rect Bounds { get; }
        public Vector2 Position => Bounds.Position;
        public Vector2 Size => Bounds.Size;
        public float x => Position.x;
        public float y => Position.y;
        public float Width => Size.x;
        public float Height => Size.y;

        public Sprite(Texture texture, Rect rect)
        {
            Texture = texture;
            Bounds = rect;
        }

        public Sprite(Texture texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Bounds = new Rect(position, size);
        }

        public Sprite(Texture texture, float x, float y, float width, float height)
        {
            Texture = texture;
            Bounds = new Rect(x, y, width, height);
        }
    }
}
