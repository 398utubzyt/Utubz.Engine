using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    /// <summary>
    /// 2D axis-aligned bounding box. Rect2 consists of a position, a size, and
    /// several utility functions. It is typically used for fast overlap tests.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect : IEquatable<Rect>
    {
        public Vector2 Position;
        public Vector2 Size;

        /// <summary>
        /// Ending corner. This is calculated as <see cref="Position"/> plus <see cref="Size"/>.
        /// Setting this value will change the size.
        /// </summary>
        /// <value>
        /// Getting is equivalent to <paramref name="value"/> = <see cref="Position"/> + <see cref="Size"/>,
        /// setting is equivalent to <see cref="Size"/> = <paramref name="value"/> - <see cref="Position"/>
        /// </value>
        public Vector2 Max
        {
            get { return Position + Size; }
            set { Size = value - Position; }
        }

        /// <summary>
        /// The area of this <see cref="Rect"/>.
        /// </summary>
        /// <value>Equivalent to <see cref="GetArea()"/>.</value>
        public float Area
        {
            get { return GetArea(); }
        }

        /// <summary>
        /// Returns a <see cref="Rect"/> with equivalent position and size, modified so that
        /// the top-left corner is the origin and width and height are positive.
        /// </summary>
        /// <returns>The modified <see cref="Rect"/>.</returns>
        public Rect Abs()
        {
            Vector2 end = Max;
            Vector2 topLeft = new Vector2(Math.Min(Position.x, end.x), Math.Min(Position.y, end.y));
            return new Rect(topLeft, Size.Abs());
        }

        /// <summary>
        /// Returns the intersection of this <see cref="Rect"/> and <paramref name="b"/>.
        /// If the rectangles do not intersect, an empty <see cref="Rect"/> is returned.
        /// </summary>
        /// <param name="b">The other <see cref="Rect"/>.</param>
        /// <returns>
        /// The intersection of this <see cref="Rect"/> and <paramref name="b"/>,
        /// or an empty <see cref="Rect"/> if they do not intersect.
        /// </returns>
        public Rect Intersection(Rect b)
        {
            Rect newRect = b;

            if (!Intersects(newRect))
            {
                return new Rect();
            }

            newRect.Position.x = Math.Max(b.Position.x, Position.x);
            newRect.Position.y = Math.Max(b.Position.y, Position.y);

            Vector2 bEnd = b.Position + b.Size;
            Vector2 end = Position + Size;

            newRect.Size.x = Math.Min(bEnd.x, end.x) - newRect.Position.x;
            newRect.Size.y = Math.Min(bEnd.y, end.y) - newRect.Position.y;

            return newRect;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this <see cref="Rect"/> completely encloses another one.
        /// </summary>
        /// <param name="b">The other <see cref="Rect"/> that may be enclosed.</param>
        /// <returns>
        /// A <see langword="bool"/> for whether or not this <see cref="Rect"/> encloses <paramref name="b"/>.
        /// </returns>
        public bool Encloses(Rect b)
        {
            return b.Position.x >= Position.x && b.Position.y >= Position.y &&
               b.Position.x + b.Size.x < Position.x + Size.x &&
               b.Position.y + b.Size.y < Position.y + Size.y;
        }

        /// <summary>
        /// Returns this <see cref="Rect"/> expanded to include a given point.
        /// </summary>
        /// <param name="to">The point to include.</param>
        /// <returns>The expanded <see cref="Rect"/>.</returns>
        public Rect Expand(Vector2 to)
        {
            Rect expanded = this;

            Vector2 begin = expanded.Position;
            Vector2 end = expanded.Position + expanded.Size;

            if (to.x < begin.x)
            {
                begin.x = to.x;
            }
            if (to.y < begin.y)
            {
                begin.y = to.y;
            }

            if (to.x > end.x)
            {
                end.x = to.x;
            }
            if (to.y > end.y)
            {
                end.y = to.y;
            }

            expanded.Position = begin;
            expanded.Size = end - begin;

            return expanded;
        }

        /// <summary>
        /// Returns the area of the <see cref="Rect"/>.
        /// </summary>
        /// <returns>The area.</returns>
        public float GetArea()
        {
            return Size.x * Size.y;
        }

        /// <summary>
        /// Returns a copy of the <see cref="Rect"/> grown by the specified amount
        /// on all sides.
        /// </summary>
        /// <seealso cref="GrowIndividual(float, float, float, float)"/>
        /// <seealso cref="GrowSide(Side, float)"/>
        /// <param name="by">The amount to grow by.</param>
        /// <returns>The grown <see cref="Rect"/>.</returns>
        public Rect Grow(float by)
        {
            Rect g = this;

            g.Position.x -= by;
            g.Position.y -= by;
            g.Size.x += by * 2;
            g.Size.y += by * 2;

            return g;
        }

        /// <summary>
        /// Returns a copy of the <see cref="Rect"/> grown by the specified amount
        /// on each side individually.
        /// </summary>
        /// <seealso cref="Grow(float)"/>
        /// <seealso cref="GrowSide(Side, float)"/>
        /// <param name="left">The amount to grow by on the left side.</param>
        /// <param name="top">The amount to grow by on the top side.</param>
        /// <param name="right">The amount to grow by on the right side.</param>
        /// <param name="bottom">The amount to grow by on the bottom side.</param>
        /// <returns>The grown <see cref="Rect"/>.</returns>
        public Rect GrowIndividual(float left, float top, float right, float bottom)
        {
            Rect g = this;

            g.Position.x -= left;
            g.Position.y -= top;
            g.Size.x += left + right;
            g.Size.y += top + bottom;

            return g;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="Rect"/> is flat or empty,
        /// or <see langword="false"/> otherwise.
        /// </summary>
        /// <returns>
        /// A <see langword="bool"/> for whether or not the <see cref="Rect"/> has area.
        /// </returns>
        public bool HasNoArea()
        {
            return Size.x <= 0 || Size.y <= 0;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="Rect"/> contains a point,
        /// or <see langword="false"/> otherwise.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>
        /// A <see langword="bool"/> for whether or not the <see cref="Rect"/> contains <paramref name="point"/>.
        /// </returns>
        public bool HasPoint(Vector2 point)
        {
            if (point.x < Position.x)
                return false;
            if (point.y < Position.y)
                return false;

            if (point.x >= Position.x + Size.x)
                return false;
            if (point.y >= Position.y + Size.y)
                return false;

            return true;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="Rect"/> overlaps with <paramref name="b"/>
        /// (i.e. they have at least one point in common).
        ///
        /// If <paramref name="includeBorders"/> is <see langword="true"/>,
        /// they will also be considered overlapping if their borders touch,
        /// even without intersection.
        /// </summary>
        /// <param name="b">The other <see cref="Rect"/> to check for intersections with.</param>
        /// <param name="includeBorders">Whether or not to consider borders.</param>
        /// <returns>A <see langword="bool"/> for whether or not they are intersecting.</returns>
        public bool Intersects(Rect b, bool includeBorders = false)
        {
            if (includeBorders)
            {
                if (Position.x > b.Position.x + b.Size.x)
                {
                    return false;
                }
                if (Position.x + Size.x < b.Position.x)
                {
                    return false;
                }
                if (Position.y > b.Position.y + b.Size.y)
                {
                    return false;
                }
                if (Position.y + Size.y < b.Position.y)
                {
                    return false;
                }
            } else
            {
                if (Position.x >= b.Position.x + b.Size.x)
                {
                    return false;
                }
                if (Position.x + Size.x <= b.Position.x)
                {
                    return false;
                }
                if (Position.y >= b.Position.y + b.Size.y)
                {
                    return false;
                }
                if (Position.y + Size.y <= b.Position.y)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a larger <see cref="Rect"/> that contains this <see cref="Rect"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other <see cref="Rect"/>.</param>
        /// <returns>The merged <see cref="Rect"/>.</returns>
        public Rect Merge(Rect b)
        {
            Rect newRect;

            newRect.Position.x = Math.Min(b.Position.x, Position.x);
            newRect.Position.y = Math.Min(b.Position.y, Position.y);

            newRect.Size.x = Math.Max(b.Position.x + b.Size.x, Position.x + Size.x);
            newRect.Size.y = Math.Max(b.Position.y + b.Size.y, Position.y + Size.y);

            newRect.Size -= newRect.Position; // Make relative again

            return newRect;
        }

        /// <summary>
        /// Constructs a <see cref="Rect"/> from a position and size.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public Rect(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Constructs a <see cref="Rect"/> from a position, width, and height.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rect(Vector2 position, float width, float height)
        {
            Position = position;
            Size = new Vector2(width, height);
        }

        /// <summary>
        /// Constructs a <see cref="Rect"/> from x, y, and size.
        /// </summary>
        /// <param name="x">The position's X coordinate.</param>
        /// <param name="y">The position's Y coordinate.</param>
        /// <param name="size">The size.</param>
        public Rect(float x, float y, Vector2 size)
        {
            Position = new Vector2(x, y);
            Size = size;
        }

        /// <summary>
        /// Constructs a <see cref="Rect"/> from x, y, width, and height.
        /// </summary>
        /// <param name="x">The position's X coordinate.</param>
        /// <param name="y">The position's Y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rect(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this rect and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the rect and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Rect)
            {
                return Equals((Rect)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this rect and <paramref name="other"/> are equal.
        /// </summary>
        /// <param name="other">The other rect to compare.</param>
        /// <returns>Whether or not the rects are equal.</returns>
        public bool Equals(Rect other)
        {
            return Position.Equals(other.Position) && Size.Equals(other.Size);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this rect and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Vector2.IsEqualApprox(Vector2)"/> on each component.
        /// </summary>
        /// <param name="other">The other rect to compare.</param>
        /// <returns>Whether or not the rects are approximately equal.</returns>
        public bool IsEqualApprox(Rect other)
        {
            return Position.Approx(other.Position) && Size.Approx(other.Size);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Rect"/>.
        /// </summary>
        /// <returns>A hash code for this rect.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Size.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Rect"/> to a string.
        /// </summary>
        /// <returns>A string representation of this rect.</returns>
        public override string ToString()
        {
            return $"{Position}, {Size}";
        }

        /// <summary>
        /// Converts this <see cref="Rect"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this rect.</returns>
        public string ToString(string format)
        {
            return $"{Position.ToString(format)}, {Size.ToString(format)}";
        }
    }
}
