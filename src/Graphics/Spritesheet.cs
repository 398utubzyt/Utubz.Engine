namespace Utubz.Graphics
{
    public sealed class Spritesheet : Object
    {
        private Sprite[] cache;

        /// <summary>
        /// Tells the <see cref="Spritesheet"/> how it should be read.
        /// </summary>
        public enum ReadMode
        {
            /// <summary>
            /// Read row by row.
            /// </summary>
            Row,
            /// <summary>
            /// Read column by column.
            /// </summary>
            Column
        }

        /// <summary>
        /// The <see cref="Texture"/> that contains the graphical data.
        /// </summary>
        public Texture Texture;
        /// <summary>
        /// The offset from (0,0) to start reading from.
        /// </summary>
        public Vector2 Offset;
        /// <summary>
        /// The size of a each <see cref="Sprite"/> within the <see cref="Spritesheet"/>.
        /// </summary>
        public Vector2 Size;
        /// <summary>
        /// The space between each <see cref="Sprite"/>.
        /// </summary>
        public Vector2 Padding;
        /// <summary>
        /// The way the <see cref="Spritesheet"/> should read.
        /// </summary>
        public ReadMode Mode;

        /// <summary>
        /// Gets a <see cref="Sprite"/> based on the <see cref="Spritesheet"/>'s properties.
        /// </summary>
        /// <param name="index">The index of the <see cref="Sprite"/>.</param>
        /// <returns>A <see cref="Sprite"/> based on the <see cref="Spritesheet"/>'s current properties.</returns>
        public Sprite Get(int index)
        {
            if (NotNull(cache[index]))
            {
                
                return cache[index];
            } else
            {
                cache[index] = New(index);
                return cache[index];
            }
        }

        private Sprite New(int index)
        {
            return new Sprite(Texture, IndexToPosition(index) / Texture.Size, Size / Texture.Size);
        }

        private Vector2 IndexToPosition(int index)
        {
            return Mode switch
            {
                ReadMode.Column => Offset
                .ShiftVertical(Math.Loop((Size.y + Padding.y) * index, Texture.Height))
                .ShiftHorizontal(Math.Floor((Size.y + Padding.y) * index / Texture.Height) * (Size.x + Padding.x)),

                _ => Offset
                .ShiftHorizontal(Math.Loop((Size.x + Padding.x) * index, Texture.Width))
                .ShiftVertical(Math.Floor((Size.x + Padding.x) * index / Texture.Width) * (Size.y + Padding.y))
            };
        }

        internal void Retry()
        {
            System.Array.Clear(cache);
            System.Array.Resize(ref cache, CacheSize());
        }

        private int CacheSize()
        {
            return (int)(((Texture.Width - Offset.x) / (Size.x + Padding.x)) * ((Texture.Height - Offset.y) / (Size.y + Padding.y)));
        }

        protected override void Clean()
        {
            System.Array.Clear(cache);
        }

        public Spritesheet(Texture texture, Vector2 offset, Vector2 size, Vector2 padding, ReadMode mode = ReadMode.Row)
        {
            Texture = texture;
            Offset = offset;
            Size = size;
            Padding = padding;
            Mode = mode;

            cache = new Sprite[CacheSize()];
        }
    }
}
