using Utubz.Graphics;

namespace Utubz.Flat
{
    public sealed class Spritesheet : Object
    {
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
        /// Generates a <see cref="Sprite"/> based on the <see cref="Spritesheet"/>'s properties.
        /// </summary>
        /// <param name="index">The index of the <see cref="Sprite"/>.</param>
        /// <returns>A new <see cref="Sprite"/> based on the <see cref="Spritesheet"/>'s current properties.</returns>
        public Sprite Get(int index)
        {
            return new Sprite(
                Texture, Mode switch
                {
                    ReadMode.Column => Offset
                    .ShiftVertical(Math.Loop((Size.y + Padding.y) * index, Texture.Height))
                    .ShiftHorizontal(Math.Floor((Size.y + Padding.y) * index / Texture.Height) * (Size.x + Padding.x)),

                    _ => Offset
                    .ShiftHorizontal(Math.Loop((Size.x + Padding.x) * index, Texture.Width))
                    .ShiftVertical(Math.Floor((Size.x + Padding.x) * index / Texture.Width) * (Size.y + Padding.y))
                }, Size
            );
        }

        public Spritesheet(Texture texture, Vector2 offset, Vector2 size, Vector2 padding, ReadMode mode = ReadMode.Row)
        {
            Texture = texture;
            Offset = offset;
            Size = size;
            Padding = padding;
            Mode = mode;
        }
    }
}
