using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    /// <summary>
    /// 2D axis-aligned bounding box. RectInt2 consists of a position, a size, and
    /// several utility functions. It is typically used for fast overlap tests.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct RectInt : IEquatable<RectInt>
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
        /// The area of this <see cref="RectInt"/>.
        /// </summary>
        /// <value>Equivalent to <see cref="GetArea()"/>.</value>
        public float Area
        {
            get { return GetArea(); }
        }

        /// <summary>
        /// Returns a <see cref="RectInt"/> with equivalent position and size, modified so that
        /// the top-left corner is the origin and width and height are positive.
        /// </summary>
        /// <returns>The modified <see cref="RectInt"/>.</returns>
        public RectInt Abs()
        {
            Vector2 end = Max;
            Vector2 topLeft = new Vector2(Math.Min(Position.x, end.x), Math.Min(Position.y, end.y));
            return new RectInt(topLeft, Size.Abs());
        }

        /// <summary>
        /// Returns the intersection of this <see cref="RectInt"/> and <paramref name="b"/>.
        /// If the RectIntangles do not intersect, an empty <see cref="RectInt"/> is returned.
        /// </summary>
        /// <param name="b">The other <see cref="RectInt"/>.</param>
        /// <returns>
        /// The intersection of this <see cref="RectInt"/> and <paramref name="b"/>,
        /// or an empty <see cref="RectInt"/> if they do not intersect.
        /// </returns>
        public RectInt Intersection(RectInt b)
        {
            RectInt newRectInt = b;

            if (!Intersects(newRectInt))
            {
                return new RectInt();
            }

            newRectInt.Position.x = Math.Max(b.Position.x, Position.x);
            newRectInt.Position.y = Math.Max(b.Position.y, Position.y);

            Vector2 bEnd = b.Position + b.Size;
            Vector2 end = Position + Size;

            newRectInt.Size.x = Math.Min(bEnd.x, end.x) - newRectInt.Position.x;
            newRectInt.Size.y = Math.Min(bEnd.y, end.y) - newRectInt.Position.y;

            return newRectInt;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this <see cref="RectInt"/> completely encloses another one.
        /// </summary>
        /// <param name="b">The other <see cref="RectInt"/> that may be enclosed.</param>
        /// <returns>
        /// A <see langword="bool"/> for whether or not this <see cref="RectInt"/> encloses <paramref name="b"/>.
        /// </returns>
        public bool Encloses(RectInt b)
        {
            return b.Position.x >= Position.x && b.Position.y >= Position.y &&
               b.Position.x + b.Size.x < Position.x + Size.x &&
               b.Position.y + b.Size.y < Position.y + Size.y;
        }

        /// <summary>
        /// Returns this <see cref="RectInt"/> expanded to include a given point.
        /// </summary>
        /// <param name="to">The point to include.</param>
        /// <returns>The expanded <see cref="RectInt"/>.</returns>
        public RectInt Expand(Vector2Int to)
        {
            RectInt expanded = this;

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
        /// Returns the area of the <see cref="RectInt"/>.
        /// </summary>
        /// <returns>The area.</returns>
        public float GetArea()
        {
            return Size.x * Size.y;
        }

        /// <summary>
        /// Returns a copy of the <see cref="RectInt"/> grown by the specified amount
        /// on all sides.
        /// </summary>
        /// <seealso cref="GrowIndividual(int, int, int, int)"/>
        /// <seealso cref="GrowSide(Side, int)"/>
        /// <param name="by">The amount to grow by.</param>
        /// <returns>The grown <see cref="RectInt"/>.</returns>
        public RectInt Grow(int by)
        {
            RectInt g = this;

            g.Position.x -= by;
            g.Position.y -= by;
            g.Size.x += by * 2;
            g.Size.y += by * 2;

            return g;
        }

        /// <summary>
        /// Returns a copy of the <see cref="RectInt"/> grown by the specified amount
        /// on each side individually.
        /// </summary>
        /// <seealso cref="Grow(int)"/>
        /// <seealso cref="GrowSide(Side, int)"/>
        /// <param name="left">The amount to grow by on the left side.</param>
        /// <param name="top">The amount to grow by on the top side.</param>
        /// <param name="right">The amount to grow by on the right side.</param>
        /// <param name="bottom">The amount to grow by on the bottom side.</param>
        /// <returns>The grown <see cref="RectInt"/>.</returns>
        public RectInt GrowIndividual(int left, int top, int right, int bottom)
        {
            RectInt g = this;

            g.Position.x -= left;
            g.Position.y -= top;
            g.Size.x += left + right;
            g.Size.y += top + bottom;

            return g;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="RectInt"/> is flat or empty,
        /// or <see langword="false"/> otherwise.
        /// </summary>
        /// <returns>
        /// A <see langword="bool"/> for whether or not the <see cref="RectInt"/> has area.
        /// </returns>
        public bool HasNoArea()
        {
            return Size.x <= 0 || Size.y <= 0;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="RectInt"/> contains a point,
        /// or <see langword="false"/> otherwise.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>
        /// A <see langword="bool"/> for whether or not the <see cref="RectInt"/> contains <paramref name="point"/>.
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
        /// Returns <see langword="true"/> if the <see cref="RectInt"/> overlaps with <paramref name="b"/>
        /// (i.e. they have at least one point in common).
        ///
        /// If <paramref name="includeBorders"/> is <see langword="true"/>,
        /// they will also be considered overlapping if their borders touch,
        /// even without intersection.
        /// </summary>
        /// <param name="b">The other <see cref="RectInt"/> to check for intersections with.</param>
        /// <param name="includeBorders">Whether or not to consider borders.</param>
        /// <returns>A <see langword="bool"/> for whether or not they are intersecting.</returns>
        public bool Intersects(RectInt b, bool includeBorders = false)
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
        /// Returns a larger <see cref="RectInt"/> that contains this <see cref="RectInt"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other <see cref="RectInt"/>.</param>
        /// <returns>The merged <see cref="RectInt"/>.</returns>
        public RectInt Merge(RectInt b)
        {
            RectInt newRectInt;

            newRectInt.Position.x = Math.Min(b.Position.x, Position.x);
            newRectInt.Position.y = Math.Min(b.Position.y, Position.y);

            newRectInt.Size.x = Math.Max(b.Position.x + b.Size.x, Position.x + Size.x);
            newRectInt.Size.y = Math.Max(b.Position.y + b.Size.y, Position.y + Size.y);

            newRectInt.Size -= newRectInt.Position; // Make relative again

            return newRectInt;
        }

        /// <summary>
        /// Constructs a <see cref="RectInt"/> from a position and size.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public RectInt(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Constructs a <see cref="RectInt"/> from a position, width, and height.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectInt(Vector2 position, int width, int height)
        {
            Position = position;
            Size = new Vector2(width, height);
        }

        /// <summary>
        /// Constructs a <see cref="RectInt"/> from x, y, and size.
        /// </summary>
        /// <param name="x">The position's X coordinate.</param>
        /// <param name="y">The position's Y coordinate.</param>
        /// <param name="size">The size.</param>
        public RectInt(int x, int y, Vector2 size)
        {
            Position = new Vector2(x, y);
            Size = size;
        }

        /// <summary>
        /// Constructs a <see cref="RectInt"/> from x, y, width, and height.
        /// </summary>
        /// <param name="x">The position's X coordinate.</param>
        /// <param name="y">The position's Y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectInt(int x, int y, int width, int height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        public static bool operator ==(RectInt left, RectInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectInt left, RectInt right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this RectInt and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the RectInt and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RectInt)
            {
                return Equals((RectInt)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this RectInt and <paramref name="other"/> are equal.
        /// </summary>
        /// <param name="other">The other RectInt to compare.</param>
        /// <returns>Whether or not the RectInts are equal.</returns>
        public bool Equals(RectInt other)
        {
            return Position.Equals(other.Position) && Size.Equals(other.Size);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this RectInt and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Vector2.IsEqualApprox(Vector2)"/> on each component.
        /// </summary>
        /// <param name="other">The other RectInt to compare.</param>
        /// <returns>Whether or not the RectInts are approximately equal.</returns>
        public bool IsEqualApprox(RectInt other)
        {
            return Position.Approx(other.Position) && Size.Approx(other.Size);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="RectInt"/>.
        /// </summary>
        /// <returns>A hash code for this RectInt.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Size.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="RectInt"/> to a string.
        /// </summary>
        /// <returns>A string representation of this RectInt.</returns>
        public override string ToString()
        {
            return $"{Position}, {Size}";
        }

        /// <summary>
        /// Converts this <see cref="RectInt"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this RectInt.</returns>
        public string ToString(string format)
        {
            return $"{Position.ToString(format)}, {Size.ToString(format)}";
        }
    }
}
