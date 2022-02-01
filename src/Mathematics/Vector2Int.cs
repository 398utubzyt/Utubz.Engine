using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    /// <summary>
    /// 2-element structure that can be used to represent positions in 2D space or any other pair of numeric values.
    /// </summary>
    /// <remarks>
    /// thanks Godot for the generous donation.
    /// (i wonder if anyone will read this)
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        /// <summary>
        /// The vector's X component. Also accessible by using the index position <c>[0]</c>.
        /// </summary>
        public int x;

        /// <summary>
        /// The vector's Y component. Also accessible by using the index position <c>[1]</c>.
        /// </summary>
        public int y;

        /// <summary>
        /// Access vector components using their index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when the given the <paramref name="index"/> is not 0 or 1.
        /// </exception>
        /// <value>
        /// <c>[0]</c> is equivalent to <see cref="x"/>,
        /// <c>[1]</c> is equivalent to <see cref="y"/>.
        /// </value>
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        return;
                    case 1:
                        y = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns a new vector with all components in absolute values (i.e. positive).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Abs(int)"/> called on each component.</returns>
        public Vector2Int Abs()
        {
            return new Vector2Int(Math.Abs(x), Math.Abs(y));
        }

        /// <summary>
        /// Returns a new vector with all components clamped between the
        /// components of <paramref name="min"/> and <paramref name="max"/> using
        /// <see cref="Math.Clamp(int, int, int)"/>.
        /// </summary>
        /// <param name="min">The vector with minimum allowed values.</param>
        /// <param name="max">The vector with maximum allowed values.</param>
        /// <returns>The vector with all components clamped.</returns>
        public Vector2Int Clamp(Vector2Int min, Vector2Int max)
        {
            return new Vector2Int
            (
                Math.Clamp(x, min.x, max.x),
                Math.Clamp(y, min.y, max.y)
            );
        }
        /// <summary>
        /// Returns the dot product of this vector and <paramref name="with"/>.
        /// </summary>
        /// <param name="with">The other vector to use.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public float Dot(Vector2Int with)
        {
            return (x * with.x) + (y * with.y);
        }

        /// <summary>
        /// Returns the inverse of this vector. This is the same as <c>new Vector2Int(1 / v.x, 1 / v.y)</c>.
        /// </summary>
        /// <returns>The inverse of this vector.</returns>
        public Vector2Int Inverse()
        {
            return new Vector2Int(1 / x, 1 / y);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the vector is normalized, and <see langword="false"/> otherwise.
        /// </summary>
        /// <returns>A <see langword="bool"/> indicating whether or not the vector is normalized.</returns>
        public bool IsNormalized()
        {
            return Math.Abs(LengthSquared - 1.0f) < Math.Epsilon;
        }

        /// <summary>
        /// Returns the length (magnitude) of this vector.
        /// </summary>
        /// <seealso cref="LengthSquared"/>
        /// <returns>The length of this vector.</returns>
        public float Length
        {
            get
            {
                return Math.Sqrt((x * x) + (y * y));
            }
        }

        /// <summary>
        /// Returns the squared length (squared magnitude) of this vector.
        /// This method runs faster than <see cref="Length"/>, so prefer it if
        /// you need to compare vectors or need the squared length for some formula.
        /// </summary>
        /// <returns>The squared length of this vector.</returns>
        public float LengthSquared
        {
            get
            {
                return (x * x) + (y * y);
            }
        }

        /// <summary>
        /// Returns the max axis value.
        /// </summary>
        /// <returns>The value of the largest axis.</returns>
        public int MaxAxis()
        {
            return Math.Max(x, y);
        }

        /// <summary>
        /// Returns the max axis value.
        /// </summary>
        /// <returns>The index of the largest axis.</returns>
        public int MaxAxisIndex()
        {
            return y < x ? 0 : 1;
        }

        /// <summary>
        /// Returns the min axis value.
        /// </summary>
        /// <returns>The value of the smallest axis.</returns>
        public int MinAxis()
        {
            return Math.Min(x, y);
        }

        /// <summary>
        /// Returns the min axis value.
        /// </summary>
        /// <returns>The index of the smallest axis.</returns>
        public int MinAxisIndex()
        {
            return x < y ? 0 : 1;
        }

        /// <summary>
        /// Returns the max axis value.
        /// </summary>
        /// <returns>The value of the largest axis.</returns>
        public int LongestAxis()
        {
            return Math.Max(Math.Abs(x), Math.Abs(y));
        }

        /// <summary>
        /// Returns the max axis value.
        /// </summary>
        /// <returns>The index of the largest axis.</returns>
        public int LongestAxisIndex()
        {
            return Math.Abs(y) < Math.Abs(x) ? 0 : 1;
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="Math.PosMod(int, int)"/> of this vector's components
        /// and <paramref name="mod"/>.
        /// </summary>
        /// <param name="mod">A value representing the divisor of the operation.</param>
        /// <returns>
        /// A vector with each component <see cref="Math.PosMod(int, int)"/> by <paramref name="mod"/>.
        /// </returns>
        public Vector2Int PosMod(int mod)
        {
            Vector2Int v;
            v.x = Math.PosMod(x, mod);
            v.y = Math.PosMod(y, mod);
            return v;
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="Math.PosMod(int, int)"/> of this vector's components
        /// and <paramref name="modv"/>'s components.
        /// </summary>
        /// <param name="modv">A vector representing the divisors of the operation.</param>
        /// <returns>
        /// A vector with each component <see cref="Math.PosMod(int, int)"/> by <paramref name="modv"/>'s components.
        /// </returns>
        public Vector2Int PosMod(Vector2Int modv)
        {
            Vector2Int v;
            v.x = Math.PosMod(x, modv.x);
            v.y = Math.PosMod(y, modv.y);
            return v;
        }

        /// <summary>
        /// Returns a vector with each component set to one or negative one, depending
        /// on the signs of this vector's components, or zero if the component is zero,
        /// by calling <see cref="Math.Sign(int)"/> on each component.
        /// </summary>
        /// <returns>A vector with all components as either <c>1</c>, <c>-1</c>, or <c>0</c>.</returns>
        public Vector2Int Sign()
        {
            Vector2Int v;
            v.x = Math.Sign(x);
            v.y = Math.Sign(y);
            return v;
        }

        /// <summary>
        /// Returns a perpendicular vector rotated 90 degrees counter-clockwise
        /// compared to the original, with the same length.
        /// </summary>
        /// <returns>The perpendicular vector.</returns>
        public Vector2Int Orthogonal()
        {
            return new Vector2Int(y, -x);
        }

        // Constants
        private static readonly Vector2Int _zero = new Vector2Int(0, 0);
        private static readonly Vector2Int _one = new Vector2Int(1, 1);
        private static readonly Vector2Int _infinity = new Vector2Int(Math.InfinityInt, Math.InfinityInt);

        private static readonly Vector2Int _up = new Vector2Int(0, -1);
        private static readonly Vector2Int _down = new Vector2Int(0, 1);
        private static readonly Vector2Int _right = new Vector2Int(1, 0);
        private static readonly Vector2Int _left = new Vector2Int(-1, 0);

        /// <summary>
        /// Zero vector, a vector with all components set to <c>0</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(0, 0)</c>.</value>
        public static Vector2Int Zero { get { return _zero; } }
        /// <summary>
        /// One vector, a vector with all components set to <c>1</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(1, 1)</c>.</value>
        public static Vector2Int One { get { return _one; } }
        /// <summary>
        /// Infinity vector, a vector with all components set to <see cref="Math.Inf"/>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(Math.Inf, Math.Inf)</c>.</value>
        public static Vector2Int Infinity { get { return _infinity; } }

        /// <summary>
        /// Up unit vector. Y is down in 2D, so this vector points -Y.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(0, -1)</c>.</value>
        public static Vector2Int Up { get { return _up; } }
        /// <summary>
        /// Down unit vector. Y is down in 2D, so this vector points +Y.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(0, 1)</c>.</value>
        public static Vector2Int Down { get { return _down; } }
        /// <summary>
        /// Right unit vector. Represents the direction of right.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(1, 0)</c>.</value>
        public static Vector2Int Right { get { return _right; } }
        /// <summary>
        /// Left unit vector. Represents the direction of left.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2Int(-1, 0)</c>.</value>
        public static Vector2Int Left { get { return _left; } }

        /// <summary>
        /// Constructs a new <see cref="Vector2Int"/> with the given components.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2Int"/> from an existing <see cref="Vector2Int"/>.
        /// </summary>
        /// <param name="v">The existing <see cref="Vector2Int"/>.</param>
        public Vector2Int(Vector2Int v)
        {
            x = v.x;
            y = v.y;
        }

        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }

        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            left.x -= right.x;
            left.y -= right.y;
            return left;
        }

        public static Vector2Int operator -(Vector2Int vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            return vec;
        }

        public static Vector2Int operator *(Vector2Int vec, int scale)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2Int operator *(int scale, Vector2Int vec)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2Int operator *(Vector2Int left, Vector2Int right)
        {
            left.x *= right.x;
            left.y *= right.y;
            return left;
        }

        public static Vector2Int operator /(Vector2Int vec, int divisor)
        {
            vec.x /= divisor;
            vec.y /= divisor;
            return vec;
        }

        public static Vector2Int operator /(Vector2Int vec, Vector2Int divisorv)
        {
            vec.x /= divisorv.x;
            vec.y /= divisorv.y;
            return vec;
        }

        public static Vector2Int operator %(Vector2Int vec, int divisor)
        {
            vec.x %= divisor;
            vec.y %= divisor;
            return vec;
        }

        public static Vector2Int operator %(Vector2Int vec, Vector2Int divisorv)
        {
            vec.x %= divisorv.x;
            vec.y %= divisorv.y;
            return vec;
        }

        public static bool operator ==(Vector2Int left, Vector2Int right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2Int left, Vector2Int right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Vector2Int left, Vector2Int right)
        {
            if (left.x == right.x)
            {
                return left.y < right.y;
            }
            return left.x < right.x;
        }

        public static bool operator >(Vector2Int left, Vector2Int right)
        {
            if (left.x == right.x)
            {
                return left.y > right.y;
            }
            return left.x > right.x;
        }

        public static bool operator <=(Vector2Int left, Vector2Int right)
        {
            if (left.x == right.x)
            {
                return left.y <= right.y;
            }
            return left.x <= right.x;
        }

        public static bool operator >=(Vector2Int left, Vector2Int right)
        {
            if (left.x == right.x)
            {
                return left.y >= right.y;
            }
            return left.x >= right.x;
        }

        public static implicit operator Vector2Int(Vector3Int vec)
        {
            return new Vector2Int(vec.x, vec.y);
        }

        public static implicit operator Vector2Int(Vector2 vec)
        {
            return new Vector2Int((int)vec.x, (int)vec.y);
        }

        public static implicit operator Vector2Int(Vector3 vec)
        {
            return new Vector2Int((int)vec.x, (int)vec.y);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the vector and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2Int)
            {
                return Equals((Vector2Int)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are equal.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are equal.</returns>
        public bool Equals(Vector2Int other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Math.IsEqualApprox(int, int)"/> on each component.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are approximately equal.</returns>
        public bool Approx(Vector2Int other)
        {
            return Math.Approx(x, other.x) && Math.Approx(y, other.y);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Vector2Int"/>.
        /// </summary>
        /// <returns>A hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return y.GetHashCode() ^ x.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector2Int"/> to a string.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return $"({x}, {y})";
        }

        /// <summary>
        /// Converts this <see cref="Vector2Int"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)})";
        }
    }
}
