using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    /// <summary>
    /// 3-element structure that can be used to represent positions in 3D space or any other pair of numeric values.
    /// </summary>
    /// <remarks>
    /// Thank you, Godot, for your better implementation of Vectors. ...and also being open-source.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3Int : IEquatable<Vector3Int>
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
        /// The vector's Z component. Also accessible by using the index position <c>[2]</c>.
        /// </summary>
        public int z;

        /// <summary>
        /// Access vector components using their index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when the given the <paramref name="index"/> is not 0, 1 or 2.
        /// </exception>
        /// <value>
        /// <c>[0]</c> is equivalent to <see cref="x"/>,
        /// <c>[1]</c> is equivalent to <see cref="y"/>,
        /// <c>[2]</c> is equivalent to <see cref="z"/>.
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
                    case 2:
                        return z;
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
                    case 2:
                        z = value;
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
        public Vector3Int Abs()
        {
            return new Vector3Int(Math.Abs(x), Math.Abs(y), Math.Abs(z));
        }

        /// <summary>
        /// Returns the dot product of this vector and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public int Dot(Vector3Int b)
        {
            return (x * b.x) + (y * b.y) + (z * b.z);
        }

        /// <summary>
        /// Returns the inverse of this vector. This is the same as <c>new Vector3Int(1 / v.x, 1 / v.y, 1 / v.z)</c>.
        /// </summary>
        /// <returns>The inverse of this vector.</returns>
        public Vector3Int Inverse()
        {
            return new Vector3Int(1 / x, 1 / y, 1 / z);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the vector is normalized, and <see langword="false"/> otherwise.
        /// </summary>
        /// <returns>A <see langword="bool"/> indicating whether or not the vector is normalized.</returns>
        public bool IsNormalized()
        {
            return Math.Abs(LengthSquared() - 1.0f) < Math.Epsilon;
        }

        /// <summary>
        /// Returns the squared length (squared magnitude) of this vector.
        /// This method runs faster than <see cref="Length"/>, so prefer it if
        /// you need to compare vectors or need the squared length for some formula.
        /// </summary>
        /// <returns>The squared length of this vector.</returns>
        public int LengthSquared()
        {
            int x2 = x * x;
            int y2 = y * y;
            int z2 = z * z;

            return x2 + y2 + z2;
        }

        /// <summary>
        /// Returns the axis of the vector's largest value.
        /// If all components are equal, this method returns X.
        /// </summary>
        /// <returns>The value of the largest axis.</returns>
        public int MaxAxis()
        {
            return x < y ? (y < z ? z : y) : (x < z ? z : x);
        }

        /// <summary>
        /// Returns the axis of the vector's largest value.
        /// If all components are equal, this method returns X.
        /// </summary>
        /// <returns>The index of the largest axis.</returns>
        public int MaxAxisIndex()
        {
            return x < y ? (y < z ? 2 : 1) : (x < z ? 2 : 1);
        }

        /// <summary>
        /// Returns the axis of the vector's smallest value.
        /// If all components are equal, this method returns Z.
        /// </summary>
        /// <returns>The value of the smallest axis.</returns>
        public int MinAxis()
        {
            return x < y ? (x < z ? x : z) : (y < z ? y : z);
        }

        /// <summary>
        /// Returns the axis of the vector's smallest value.
        /// If all components are equal, this method returns Z.
        /// </summary>
        /// <returns>The index of the smallest axis.</returns>
        public int MinAxisIndex()
        {
            return x < y ? (x < z ? 0 : 2) : (y < z ? 1 : 2);
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="Math.PosMod(int, int)"/> of this vector's components
        /// and <paramref name="mod"/>.
        /// </summary>
        /// <param name="mod">A value representing the divisor of the operation.</param>
        /// <returns>
        /// A vector with each component <see cref="Math.PosMod(int, int)"/> by <paramref name="mod"/>.
        /// </returns>
        public Vector3Int PosMod(int mod)
        {
            Vector3Int v;
            v.x = Math.PosMod(x, mod);
            v.y = Math.PosMod(y, mod);
            v.z = Math.PosMod(z, mod);
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
        public Vector3Int PosMod(Vector3Int modv)
        {
            Vector3Int v;
            v.x = Math.PosMod(x, modv.x);
            v.y = Math.PosMod(y, modv.y);
            v.z = Math.PosMod(z, modv.z);
            return v;
        }

        /// <summary>
        /// Returns this vector projected onto another vector <paramref name="onNormal"/>.
        /// </summary>
        /// <param name="onNormal">The vector to project onto.</param>
        /// <returns>The projected vector.</returns>
        public Vector3Int Project(Vector3Int onNormal)
        {
            return onNormal * (Dot(onNormal) / onNormal.LengthSquared());
        }

        /// <summary>
        /// Returns a vector with each component set to one or negative one, depending
        /// on the signs of this vector's components, or zero if the component is zero,
        /// by calling <see cref="Math.Sign(int)"/> on each component.
        /// </summary>
        /// <returns>A vector with all components as either <c>1</c>, <c>-1</c>, or <c>0</c>.</returns>
        public Vector3Int Sign()
        {
            Vector3Int v;
            v.x = Math.Sign(x);
            v.y = Math.Sign(y);
            v.z = Math.Sign(z);
            return v;
        }

        /// <summary>
        /// Returns this vector slid along a plane defined by the given <paramref name="normal"/>.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to slide on.</param>
        /// <returns>The slid vector.</returns>
        public Vector3Int Slide(Vector3Int normal)
        {
            return this - (normal * Dot(normal));
        }

        // Constants
        private static readonly Vector3Int _zero = new Vector3Int(0, 0, 0);
        private static readonly Vector3Int _one = new Vector3Int(1, 1, 1);
        private static readonly Vector3Int _inf = new Vector3Int(Math.InfinityInt, Math.InfinityInt, Math.InfinityInt);

        private static readonly Vector3Int _up = new Vector3Int(0, 1, 0);
        private static readonly Vector3Int _down = new Vector3Int(0, -1, 0);
        private static readonly Vector3Int _right = new Vector3Int(1, 0, 0);
        private static readonly Vector3Int _left = new Vector3Int(-1, 0, 0);
        private static readonly Vector3Int _forward = new Vector3Int(0, 0, -1);
        private static readonly Vector3Int _back = new Vector3Int(0, 0, 1);

        /// <summary>
        /// Zero vector, a vector with all components set to <c>0</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(0, 0, 0)</c>.</value>
        public static Vector3Int Zero { get { return _zero; } }
        /// <summary>
        /// One vector, a vector with all components set to <c>1</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(1, 1, 1)</c>.</value>
        public static Vector3Int One { get { return _one; } }
        /// <summary>
        /// Infinity vector, a vector with all components set to <see cref="Math.Infinity"/>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(Math.Inf, Math.Inf, Math.Inf)</c>.</value>
        public static Vector3Int Infinity { get { return _inf; } }

        /// <summary>
        /// Up unit vector.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(0, 1, 0)</c>.</value>
        public static Vector3Int Up { get { return _up; } }
        /// <summary>
        /// Down unit vector.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(0, -1, 0)</c>.</value>
        public static Vector3Int Down { get { return _down; } }
        /// <summary>
        /// Right unit vector. Represents the local direction of right,
        /// and the global direction of east.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(1, 0, 0)</c>.</value>
        public static Vector3Int Right { get { return _right; } }
        /// <summary>
        /// Left unit vector. Represents the local direction of left,
        /// and the global direction of west.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(-1, 0, 0)</c>.</value>
        public static Vector3Int Left { get { return _left; } }
        /// <summary>
        /// Forward unit vector. Represents the local direction of forward,
        /// and the global direction of north.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(0, 0, -1)</c>.</value>
        public static Vector3Int Forward { get { return _forward; } }
        /// <summary>
        /// Back unit vector. Represents the local direction of back,
        /// and the global direction of south.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3Int(0, 0, 1)</c>.</value>
        public static Vector3Int Back { get { return _back; } }

        /// <summary>
        /// Constructs a new <see cref="Vector3Int"/> with the given components.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        /// <param name="z">The vector's Z component.</param>
        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructs a new <see cref="Vector3Int"/> from an existing <see cref="Vector3Int"/>.
        /// </summary>
        /// <param name="v">The existing <see cref="Vector3Int"/>.</param>
        public Vector3Int(Vector3Int v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static Vector3Int operator +(Vector3Int left, Vector3Int right)
        {
            left.x += right.x;
            left.y += right.y;
            left.z += right.z;
            return left;
        }

        public static Vector3Int operator -(Vector3Int left, Vector3Int right)
        {
            left.x -= right.x;
            left.y -= right.y;
            left.z -= right.z;
            return left;
        }

        public static Vector3Int operator -(Vector3Int vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            vec.z = -vec.z;
            return vec;
        }

        public static Vector3Int operator *(Vector3Int vec, int scale)
        {
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        public static Vector3Int operator *(int scale, Vector3Int vec)
        {
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        public static Vector3Int operator *(Vector3Int left, Vector3Int right)
        {
            left.x *= right.x;
            left.y *= right.y;
            left.z *= right.z;
            return left;
        }

        public static Vector3Int operator /(Vector3Int vec, int divisor)
        {
            vec.x /= divisor;
            vec.y /= divisor;
            vec.z /= divisor;
            return vec;
        }

        public static Vector3Int operator /(Vector3Int vec, Vector3Int divisorv)
        {
            vec.x /= divisorv.x;
            vec.y /= divisorv.y;
            vec.z /= divisorv.z;
            return vec;
        }

        public static Vector3Int operator %(Vector3Int vec, int divisor)
        {
            vec.x %= divisor;
            vec.y %= divisor;
            vec.z %= divisor;
            return vec;
        }

        public static Vector3Int operator %(Vector3Int vec, Vector3Int divisorv)
        {
            vec.x %= divisorv.x;
            vec.y %= divisorv.y;
            vec.z %= divisorv.z;
            return vec;
        }

        public static bool operator ==(Vector3Int left, Vector3Int right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3Int left, Vector3Int right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Vector3Int left, Vector3Int right)
        {
            if (left.x == right.x)
            {
                if (left.y == right.y)
                {
                    return left.z < right.z;
                }
                return left.y < right.y;
            }
            return left.x < right.x;
        }

        public static bool operator >(Vector3Int left, Vector3Int right)
        {
            if (left.x == right.x)
            {
                if (left.y == right.y)
                {
                    return left.z > right.z;
                }
                return left.y > right.y;
            }
            return left.x > right.x;
        }

        public static bool operator <=(Vector3Int left, Vector3Int right)
        {
            if (left.x == right.x)
            {
                if (left.y == right.y)
                {
                    return left.z <= right.z;
                }
                return left.y < right.y;
            }
            return left.x < right.x;
        }

        public static bool operator >=(Vector3Int left, Vector3Int right)
        {
            if (left.x == right.x)
            {
                if (left.y == right.y)
                {
                    return left.z >= right.z;
                }
                return left.y > right.y;
            }
            return left.x > right.x;
        }

        public static implicit operator Vector3Int(Vector2Int vec)
        {
            return new Vector3Int(vec.x, vec.y, 0);
        }

        public static implicit operator Vector3Int(Vector3 vec)
        {
            return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
        }

        public static implicit operator Vector3Int(Vector2 vec)
        {
            return new Vector3Int((int)vec.x, (int)vec.y, 0);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the vector and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3Int)
            {
                return Equals((Vector3Int)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are equal
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are equal.</returns>
        public bool Equals(Vector3Int other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Math.Approx(int, int)"/> on each component.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are approximately equal.</returns>
        public bool Approx(Vector3Int other)
        {
            return Math.Approx(x, other.x) && Math.Approx(y, other.y) && Math.Approx(z, other.z);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Vector3Int"/>.
        /// </summary>
        /// <returns>A hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return y.GetHashCode() ^ x.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector3Int"/> to a string.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        /// <summary>
        /// Converts this <see cref="Vector3Int"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)})";
        }
    }
}
