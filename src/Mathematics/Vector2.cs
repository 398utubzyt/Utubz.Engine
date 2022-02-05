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
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// The vector's X component. Also accessible by using the index position <c>[0]</c>.
        /// </summary>
        public float x;

        /// <summary>
        /// The vector's Y component. Also accessible by using the index position <c>[1]</c>.
        /// </summary>
        public float y;

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
        public float this[int index]
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

        private void Normalize()
        {
            float lengthsq = LengthSquared;

            if (lengthsq == 0)
            {
                x = y = 0f;
            } else
            {
                float length = Math.Sqrt(lengthsq);
                x /= length;
                y /= length;
            }
        }

        /// <summary>
        /// Returns a new vector with all components in absolute values (i.e. positive).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Abs(float)"/> called on each component.</returns>
        public Vector2 Abs()
        {
            return new Vector2(Math.Abs(x), Math.Abs(y));
        }

        /// <summary>
        /// Returns this vector's angle with respect to the X axis, or (1, 0) vector, in degrees.
        ///
        /// Equivalent to the result of <see cref="Math.Atan2(float, float)"/> when
        /// called with the vector's <see cref="y"/> and <see cref="x"/> as parameters: <c>Math.Atan2(v.y, v.x)</c>.
        /// </summary>
        /// <returns>The angle of this vector, in degrees.</returns>
        public float Angle()
        {
            return Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns the angle to the given vector, in degrees.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <returns>The angle between the two vectors, in degrees.</returns>
        public float AngleTo(Vector2 to)
        {
            return Math.Atan2(Cross(to), Dot(to));
        }

        /// <summary>
        /// Returns the angle between the line connecting the two points and the X axis, in degrees.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <returns>The angle between the two vectors, in degrees.</returns>
        public float AngleToPoint(Vector2 to)
        {
            return Math.Atan2(y - to.y, x - to.x);
        }

        /// <summary>
        /// Returns the aspect ratio of this vector, the ratio of <see cref="x"/> to <see cref="y"/>.
        /// </summary>
        /// <returns>The <see cref="x"/> component divided by the <see cref="y"/> component.</returns>
        public float Aspect()
        {
            return x / y;
        }

        /// <summary>
        /// Returns the vector shifted on the x-axis by <paramref name="shift"/>.
        /// </summary>
        /// <param name="shift">The amount to shift by.</param>
        /// <returns>The shifted vector.</returns>
        public Vector2 ShiftHorizontal(float shift)
        {
            return new Vector2(x + shift, y);
        }

        /// <summary>
        /// Returns the vector shifted on the y-axis by <paramref name="shift"/>.
        /// </summary>
        /// <param name="shift">The amount to shift by.</param>
        /// <returns>The shifted vector.</returns>
        public Vector2 ShiftVertical(float shift)
        {
            return new Vector2(x, y + shift);
        }

        /// <summary>
        /// Returns the vector "bounced off" from a plane defined by the given normal.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to bounce off. Must be normalized.</param>
        /// <returns>The bounced vector.</returns>
        public Vector2 Bounce(Vector2 normal)
        {
            return -Reflect(normal);
        }

        /// <summary>
        /// Returns a new vector with all components rounded up (towards positive infinity).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Ceil"/> called on each component.</returns>
        public Vector2 Ceil()
        {
            return new Vector2(Math.Ceil(x), Math.Ceil(y));
        }

        /// <summary>
        /// Returns a new vector with all components clamped between the
        /// components of <paramref name="min"/> and <paramref name="max"/> using
        /// <see cref="Math.Clamp(float, float, float)"/>.
        /// </summary>
        /// <param name="min">The vector with minimum allowed values.</param>
        /// <param name="max">The vector with maximum allowed values.</param>
        /// <returns>The vector with all components clamped.</returns>
        public Vector2 Clamp(Vector2 min, Vector2 max)
        {
            return new Vector2
            (
                Math.Clamp(x, min.x, max.x),
                Math.Clamp(y, min.y, max.y)
            );
        }

        /// <summary>
        /// Returns the cross product of this vector and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>The cross product value.</returns>
        public float Cross(Vector2 b)
        {
            return (x * b.y) - (y * b.x);
        }

        /// <summary>
        /// Performs a cubic interpolation between vectors <paramref name="preA"/>, this vector,
        /// <paramref name="b"/>, and <paramref name="postB"/>, by the given amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="b">The destination vector.</param>
        /// <param name="preA">A vector before this vector.</param>
        /// <param name="postB">A vector after <paramref name="b"/>.</param>
        /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The interpolated vector.</returns>
        public Vector2 CubicInterpolate(Vector2 b, Vector2 preA, Vector2 postB, float weight)
        {
            Vector2 p0 = preA;
            Vector2 p1 = this;
            Vector2 p2 = b;
            Vector2 p3 = postB;

            float t = weight;
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f * (
                (p1 * 2.0f) +
                ((-p0 + p2) * t) +
                (((2.0f * p0) - (5.0f * p1) + (4 * p2) - p3) * t2) +
                ((-p0 + (3.0f * p1) - (3.0f * p2) + p3) * t3)
            );
        }

        /// <summary>
        /// Returns the normalized vector pointing from this vector to <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector to point towards.</param>
        /// <returns>The direction from this vector to <paramref name="b"/>.</returns>
        public Vector2 DirectionTo(Vector2 b)
        {
            return new Vector2(b.x - x, b.y - y).Normalized();
        }

        /// <summary>
        /// Returns the squared distance between this vector and <paramref name="to"/>.
        /// This method runs faster than <see cref="DistanceTo"/>, so prefer it if
        /// you need to compare vectors or need the squared distance for some formula.
        /// </summary>
        /// <param name="to">The other vector to use.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public float DistanceSquaredTo(Vector2 to)
        {
            return (x - to.x) * (x - to.x) + (y - to.y) * (y - to.y);
        }

        /// <summary>
        /// Returns the distance between this vector and <paramref name="to"/>.
        /// </summary>
        /// <param name="to">The other vector to use.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float DistanceTo(Vector2 to)
        {
            return Math.Sqrt((x - to.x) * (x - to.x) + (y - to.y) * (y - to.y));
        }

        /// <summary>
        /// Returns the dot product of this vector and <paramref name="with"/>.
        /// </summary>
        /// <param name="with">The other vector to use.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public float Dot(Vector2 with)
        {
            return (x * with.x) + (y * with.y);
        }

        /// <summary>
        /// Returns a new vector with all components rounded down (towards negative infinity).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Floor"/> called on each component.</returns>
        public Vector2 Floor()
        {
            return new Vector2(Math.Floor(x), Math.Floor(y));
        }

        /// <summary>
        /// Returns the inverse of this vector. This is the same as <c>new Vector2(1 / v.x, 1 / v.y)</c>.
        /// </summary>
        /// <returns>The inverse of this vector.</returns>
        public Vector2 Inverse()
        {
            return new Vector2(1 / x, 1 / y);
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
        /// Returns the result of the linear interpolation between
        /// this vector and <paramref name="to"/> by amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination vector for interpolation.</param>
        /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting vector of the interpolation.</returns>
        public Vector2 Lerp(Vector2 to, float weight)
        {
            return new Vector2
            (
                Math.Lerp(x, to.x, weight),
                Math.Lerp(y, to.y, weight)
            );
        }

        /// <summary>
        /// Returns the result of the linear interpolation between
        /// this vector and <paramref name="to"/> by the vector amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination vector for interpolation.</param>
        /// <param name="weight">
        /// A vector with components on the range of 0.0 to 1.0, representing the amount of interpolation.
        /// </param>
        /// <returns>The resulting vector of the interpolation.</returns>
        public Vector2 Lerp(Vector2 to, Vector2 weight)
        {
            return new Vector2
            (
                Math.Lerp(x, to.x, weight.x),
                Math.Lerp(y, to.y, weight.y)
            );
        }

        /// <summary>
        /// Returns the vector with a maximum length by limiting its length to <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The length to limit to.</param>
        /// <returns>The vector with its length limited.</returns>
        public Vector2 LimitLength(float length = 1.0f)
        {
            Vector2 v = this;
            float l = Length;

            if (l > 0 && length < l)
            {
                v /= l;
                v *= length;
            }

            return v;
        }

        /// <summary>
        /// Returns the max axis value.
        /// </summary>
        /// <returns>The value of the largest axis.</returns>
        public float MaxAxis()
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
        public float MinAxis()
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
        public float LongestAxis()
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
        /// Moves this vector toward <paramref name="to"/> by the fixed <paramref name="delta"/> amount.
        /// </summary>
        /// <param name="to">The vector to move towards.</param>
        /// <param name="delta">The amount to move towards by.</param>
        /// <returns>The resulting vector.</returns>
        public Vector2 Move(Vector2 to, float delta)
        {
            Vector2 v = this;
            Vector2 vd = to - v;
            float len = vd.Length;
            if (len <= delta || len < Math.Epsilon)
                return to;

            return v + (vd / len * delta);
        }

        /// <summary>
        /// Returns the vector scaled to unit length. Equivalent to <c>v / v.Length()</c>.
        /// </summary>
        /// <returns>A normalized version of the vector.</returns>
        public Vector2 Normalized()
        {
            Vector2 v = this;
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="Math.PosMod(float, float)"/> of this vector's components
        /// and <paramref name="mod"/>.
        /// </summary>
        /// <param name="mod">A value representing the divisor of the operation.</param>
        /// <returns>
        /// A vector with each component <see cref="Math.PosMod(float, float)"/> by <paramref name="mod"/>.
        /// </returns>
        public Vector2 PosMod(float mod)
        {
            Vector2 v;
            v.x = Math.PosMod(x, mod);
            v.y = Math.PosMod(y, mod);
            return v;
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="Math.PosMod(float, float)"/> of this vector's components
        /// and <paramref name="modv"/>'s components.
        /// </summary>
        /// <param name="modv">A vector representing the divisors of the operation.</param>
        /// <returns>
        /// A vector with each component <see cref="Math.PosMod(float, float)"/> by <paramref name="modv"/>'s components.
        /// </returns>
        public Vector2 PosMod(Vector2 modv)
        {
            Vector2 v;
            v.x = Math.PosMod(x, modv.x);
            v.y = Math.PosMod(y, modv.y);
            return v;
        }

        /// <summary>
        /// Returns this vector projected onto another vector <paramref name="onNormal"/>.
        /// </summary>
        /// <param name="onNormal">The vector to project onto.</param>
        /// <returns>The projected vector.</returns>
        public Vector2 Project(Vector2 onNormal)
        {
            return onNormal * (Dot(onNormal) / onNormal.LengthSquared);
        }

        /// <summary>
        /// Returns this vector reflected from a plane defined by the given <paramref name="normal"/>.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to reflect from. Must be normalized.</param>
        /// <returns>The reflected vector.</returns>
        public Vector2 Reflect(Vector2 normal)
        {
#if DEBUG
            if (!normal.IsNormalized())
            {
                throw new ArgumentException("Argument is not normalized", nameof(normal));
            }
#endif
            return (2 * Dot(normal) * normal) - this;
        }

        /// <summary>
        /// Rotates this vector by <paramref name="phi"/> degrees.
        /// </summary>
        /// <param name="phi">The angle to rotate by, in degrees.</param>
        /// <returns>The rotated vector.</returns>
        public Vector2 Rotated(float phi)
        {
            float sine = Math.Sin(phi);
            float cosi = Math.Cos(phi);
            return new Vector2(
                x * cosi - y * sine,
                x * sine + y * cosi);
        }

        /// <summary>
        /// Returns this vector with all components rounded to the nearest integer,
        /// with halfway cases rounded towards the nearest multiple of two.
        /// </summary>
        /// <returns>The rounded vector.</returns>
        public Vector2 Round()
        {
            return new Vector2(Math.Round(x), Math.Round(y));
        }

        /// <summary>
        /// Returns a vector with each component set to one or negative one, depending
        /// on the signs of this vector's components, or zero if the component is zero,
        /// by calling <see cref="Math.Sign(float)"/> on each component.
        /// </summary>
        /// <returns>A vector with all components as either <c>1</c>, <c>-1</c>, or <c>0</c>.</returns>
        public Vector2 Sign()
        {
            Vector2 v;
            v.x = Math.Sign(x);
            v.y = Math.Sign(y);
            return v;
        }

        /// <summary>
        /// Returns the result of the spherical linear interpolation between
        /// this vector and <paramref name="to"/> by amount <paramref name="weight"/>.
        ///
        /// Note: Both vectors must be normalized.
        /// </summary>
        /// <param name="to">The destination vector for interpolation. Must be normalized.</param>
        /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting vector of the interpolation.</returns>
        public Vector2 Slerp(Vector2 to, float weight)
        {
#if DEBUG
            if (!IsNormalized())
            {
                throw new InvalidOperationException("Vector2.Slerp: From vector is not normalized.");
            }
            if (!to.IsNormalized())
            {
                throw new InvalidOperationException($"Vector2.Slerp: `{nameof(to)}` is not normalized.");
            }
#endif
            return Rotated(AngleTo(to) * weight);
        }

        /// <summary>
        /// Returns this vector slid along a plane defined by the given <paramref name="normal"/>.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to slide on.</param>
        /// <returns>The slid vector.</returns>
        public Vector2 Slide(Vector2 normal)
        {
            return this - (normal * Dot(normal));
        }

        /// <summary>
        /// Returns this vector with each component snapped to the nearest multiple of <paramref name="step"/>.
        /// This can also be used to round to an arbitrary number of decimals.
        /// </summary>
        /// <param name="step">A vector value representing the step size to snap to.</param>
        /// <returns>The snapped vector.</returns>
        public Vector2 Snapped(Vector2 step)
        {
            return new Vector2(Math.Snap(x, step.x), Math.Snap(y, step.y));
        }

        /// <summary>
        /// Returns a perpendicular vector rotated 90 degrees counter-clockwise
        /// compared to the original, with the same length.
        /// </summary>
        /// <returns>The perpendicular vector.</returns>
        public Vector2 Orthogonal()
        {
            return new Vector2(y, -x);
        }

        /// <summary>
        /// Maps this vector to a <see cref="Vector3"/> on the XZ axis (vs XY).
        /// </summary>
        /// <returns>A <see cref="Vector3"/> version of this vector on the XZ axis.</returns>
        public Vector3 ToXZ()
        {
            Vector3 v = Vector3.Zero;
            v.x = x;
            v.y = 0f;
            v.z = y;
            return v;
        }

        // Constants
        private static readonly Vector2 _zero = new Vector2(0, 0);
        private static readonly Vector2 _one = new Vector2(1, 1);
        private static readonly Vector2 _infinity = new Vector2(Math.Infinity, Math.Infinity);

        private static readonly Vector2 _up = new Vector2(0, -1);
        private static readonly Vector2 _down = new Vector2(0, 1);
        private static readonly Vector2 _right = new Vector2(1, 0);
        private static readonly Vector2 _left = new Vector2(-1, 0);

        /// <summary>
        /// Zero vector, a vector with all components set to <c>0</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(0, 0)</c>.</value>
        public static Vector2 Zero { get { return _zero; } }
        /// <summary>
        /// One vector, a vector with all components set to <c>1</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(1, 1)</c>.</value>
        public static Vector2 One { get { return _one; } }
        /// <summary>
        /// Infinity vector, a vector with all components set to <see cref="Math.Inf"/>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(Math.Inf, Math.Inf)</c>.</value>
        public static Vector2 Infinity { get { return _infinity; } }

        /// <summary>
        /// Up unit vector. Y is down in 2D, so this vector points -Y.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(0, -1)</c>.</value>
        public static Vector2 Up { get { return _up; } }
        /// <summary>
        /// Down unit vector. Y is down in 2D, so this vector points +Y.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(0, 1)</c>.</value>
        public static Vector2 Down { get { return _down; } }
        /// <summary>
        /// Right unit vector. Represents the direction of right.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(1, 0)</c>.</value>
        public static Vector2 Right { get { return _right; } }
        /// <summary>
        /// Left unit vector. Represents the direction of left.
        /// </summary>
        /// <value>Equivalent to <c>new Vector2(-1, 0)</c>.</value>
        public static Vector2 Left { get { return _left; } }

        /// <summary>
        /// Constructs a new <see cref="Vector2"/> with the given components.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2"/> from an existing <see cref="Vector2"/>.
        /// </summary>
        /// <param name="v">The existing <see cref="Vector2"/>.</param>
        public Vector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        /// <summary>
        /// Creates a unit Vector2 rotated to the given angle. This is equivalent to doing
        /// <c>Vector2(Math.Cos(angle), Math.Sin(angle))</c> or <c>Vector2.Right.Rotated(angle)</c>.
        /// </summary>
        /// <param name="angle">Angle of the vector, in degrees.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector2 FromAngle(float angle)
        {
            return new Vector2(Math.Cos(angle), Math.Sin(angle));
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.x -= right.x;
            left.y -= right.y;
            return left;
        }

        public static Vector2 operator -(Vector2 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            return vec;
        }

        public static Vector2 operator *(Vector2 vec, float scale)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2 operator *(float scale, Vector2 vec)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            left.x *= right.x;
            left.y *= right.y;
            return left;
        }

        public static Vector2 operator /(Vector2 vec, float divisor)
        {
            vec.x /= divisor;
            vec.y /= divisor;
            return vec;
        }

        public static Vector2 operator /(Vector2 vec, Vector2 divisorv)
        {
            vec.x /= divisorv.x;
            vec.y /= divisorv.y;
            return vec;
        }

        public static Vector2 operator %(Vector2 vec, float divisor)
        {
            vec.x %= divisor;
            vec.y %= divisor;
            return vec;
        }

        public static Vector2 operator %(Vector2 vec, Vector2 divisorv)
        {
            vec.x %= divisorv.x;
            vec.y %= divisorv.y;
            return vec;
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Vector2 left, Vector2 right)
        {
            if (left.x == right.x)
            {
                return left.y < right.y;
            }
            return left.x < right.x;
        }

        public static bool operator >(Vector2 left, Vector2 right)
        {
            if (left.x == right.x)
            {
                return left.y > right.y;
            }
            return left.x > right.x;
        }

        public static bool operator <=(Vector2 left, Vector2 right)
        {
            if (left.x == right.x)
            {
                return left.y <= right.y;
            }
            return left.x <= right.x;
        }

        public static bool operator >=(Vector2 left, Vector2 right)
        {
            if (left.x == right.x)
            {
                return left.y >= right.y;
            }
            return left.x >= right.x;
        }

        public static implicit operator Vector2(Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static implicit operator Vector2(Vector2Int vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static implicit operator Vector2(Vector3Int vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the vector and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return Equals((Vector2)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are equal.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are equal.</returns>
        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Math.IsEqualApprox(float, float)"/> on each component.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are approximately equal.</returns>
        public bool Approx(Vector2 other)
        {
            return Math.Approx(x, other.x) && Math.Approx(y, other.y);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Vector2"/>.
        /// </summary>
        /// <returns>A hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return y.GetHashCode() ^ x.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector2"/> to a string.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return $"({x}, {y})";
        }

        /// <summary>
        /// Converts this <see cref="Vector2"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)})";
        }
    }
}
