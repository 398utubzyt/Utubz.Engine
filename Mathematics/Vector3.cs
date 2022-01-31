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
    public struct Vector3 : IEquatable<Vector3>
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
        /// The vector's Z component. Also accessible by using the index position <c>[2]</c>.
        /// </summary>
        public float z;

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

        internal void Normalize()
        {
            float lengthsq = LengthSquared();

            if (lengthsq == 0)
            {
                x = y = z = 0f;
            } else
            {
                float length = Math.Sqrt(lengthsq);
                x /= length;
                y /= length;
                z /= length;
            }
        }

        /// <summary>
        /// Returns a new vector with all components in absolute values (i.e. positive).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Abs(float)"/> called on each component.</returns>
        public Vector3 Abs()
        {
            return new Vector3(Math.Abs(x), Math.Abs(y), Math.Abs(z));
        }

        /// <summary>
        /// Returns the unsigned minimum angle to the given vector, in radians.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <returns>The unsigned angle between the two vectors, in radians.</returns>
        public float AngleTo(Vector3 to)
        {
            return Math.Atan2(Cross(to).Length(), Dot(to));
        }

        /// <summary>
        /// Returns this vector "bounced off" from a plane defined by the given normal.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to bounce off. Must be normalized.</param>
        /// <returns>The bounced vector.</returns>
        public Vector3 Bounce(Vector3 normal)
        {
            return -Reflect(normal);
        }

        /// <summary>
        /// Returns a new vector with all components rounded up (towards positive infinity).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Ceil"/> called on each component.</returns>
        public Vector3 Ceil()
        {
            return new Vector3(Math.Ceil(x), Math.Ceil(y), Math.Ceil(z));
        }

        /// <summary>
        /// Returns a new vector with all components clamped between the
        /// components of <paramref name="min"/> and <paramref name="max"/> using
        /// <see cref="Math.Clamp(float, float, float)"/>.
        /// </summary>
        /// <param name="min">The vector with minimum allowed values.</param>
        /// <param name="max">The vector with maximum allowed values.</param>
        /// <returns>The vector with all components clamped.</returns>
        public Vector3 Clamp(Vector3 min, Vector3 max)
        {
            return new Vector3
            (
                Math.Clamp(x, min.x, max.x),
                Math.Clamp(y, min.y, max.y),
                Math.Clamp(z, min.z, max.z)
            );
        }

        /// <summary>
        /// Returns the cross product of this vector and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>The cross product vector.</returns>
        public Vector3 Cross(Vector3 b)
        {
            return new Vector3
            (
                (y * b.z) - (z * b.y),
                (z * b.x) - (x * b.z),
                (x * b.y) - (y * b.x)
            );
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
        public Vector3 CubicInterpolate(Vector3 b, Vector3 preA, Vector3 postB, float weight)
        {
            Vector3 p0 = preA;
            Vector3 p1 = this;
            Vector3 p2 = b;
            Vector3 p3 = postB;

            float t = weight;
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f * (
                (p1 * 2.0f) + ((-p0 + p2) * t) +
                (((2.0f * p0) - (5.0f * p1) + (4f * p2) - p3) * t2) +
                ((-p0 + (3.0f * p1) - (3.0f * p2) + p3) * t3)
            );
        }

        /// <summary>
        /// Gets the average of the 3 axis.
        /// </summary>
        /// <returns>The average value of the 3 axis of this vector.</returns>
        public float Average()
        {
            return (x + y + z) / 3f;
        }

        /// <summary>
        /// Returns the normalized vector pointing from this vector to <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector to point towards.</param>
        /// <returns>The direction from this vector to <paramref name="b"/>.</returns>
        public Vector3 DirectionTo(Vector3 b)
        {
            return new Vector3(b.x - x, b.y - y, b.z - z).Normalized();
        }

        /// <summary>
        /// Returns the squared distance between this vector and <paramref name="b"/>.
        /// This method runs faster than <see cref="DistanceTo"/>, so prefer it if
        /// you need to compare vectors or need the squared distance for some formula.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public float DistanceSquaredTo(Vector3 b)
        {
            return (b - this).LengthSquared();
        }

        /// <summary>
        /// Returns the distance between this vector and <paramref name="b"/>.
        /// </summary>
        /// <seealso cref="DistanceSquaredTo(Vector3)"/>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float DistanceTo(Vector3 b)
        {
            return (b - this).Length();
        }

        /// <summary>
        /// Returns the dot product of this vector and <paramref name="b"/>.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public float Dot(Vector3 b)
        {
            return (x * b.x) + (y * b.y) + (z * b.z);
        }

        /// <summary>
        /// Returns a new vector with all components rounded down (towards negative infinity).
        /// </summary>
        /// <returns>A vector with <see cref="Math.Floor"/> called on each component.</returns>
        public Vector3 Floor()
        {
            return new Vector3(Math.Floor(x), Math.Floor(y), Math.Floor(z));
        }

        /// <summary>
        /// Returns the inverse of this vector. This is the same as <c>new Vector3(1 / v.x, 1 / v.y, 1 / v.z)</c>.
        /// </summary>
        /// <returns>The inverse of this vector.</returns>
        public Vector3 Inverse()
        {
            return new Vector3(1 / x, 1 / y, 1 / z);
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
        /// Returns the length (magnitude) of this vector.
        /// </summary>
        /// <seealso cref="LengthSquared"/>
        /// <returns>The length of this vector.</returns>
        public float Length()
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;

            return Math.Sqrt(x2 + y2 + z2);
        }

        /// <summary>
        /// Returns the squared length (squared magnitude) of this vector.
        /// This method runs faster than <see cref="Length"/>, so prefer it if
        /// you need to compare vectors or need the squared length for some formula.
        /// </summary>
        /// <returns>The squared length of this vector.</returns>
        public float LengthSquared()
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;

            return x2 + y2 + z2;
        }

        public static Vector3 ToForwardAxis(float xt, float yt, float zt)
        {
            Vector3 v = Vector3.Zero;

            SinCosResult _scrx = Math.SinCos(xt);
            SinCosResult _scry = Math.SinCos(yt);
            SinCosResult _scrz = Math.SinCos(zt);

            v.x = -_scrx.Cos * _scrz.Cos * _scry.Sin + _scrx.Sin * _scrz.Sin;
            v.y = _scrz.Cos * _scrx.Sin + _scrx.Cos * _scry.Sin * _scrz.Sin;
            v.z = _scrx.Cos * _scry.Cos;

            return v;
        }

        public static Vector3 ToForwardAxis(Vector3 vec)
            => ToForwardAxis(vec.x, vec.y, vec.z);

        public static Vector3 ToUpAxis(float xt, float yt, float zt)
        {
            Vector3 v = Vector3.Zero;

            SinCosResult _scrx = Math.SinCos(xt);
            SinCosResult _scry = Math.SinCos(yt);
            SinCosResult _scrz = Math.SinCos(zt);

            v.x = _scrz.Cos * _scrx.Sin * _scry.Sin + _scrx.Cos * _scrz.Sin;
            v.y = _scrx.Cos * _scrz.Cos - _scrx.Sin * _scry.Sin * _scrz.Sin;
            v.z = -_scry.Cos * _scrx.Sin;

            return v;
        }

        public static Vector3 ToUpAxis(Vector3 vec)
            => ToUpAxis(vec.x, vec.y, vec.z);

        public static Vector3 ToRightAxis(float xt, float yt, float zt)
        {
            Vector3 v = Vector3.Zero;

            SinCosResult _scry = Math.SinCos(yt);
            SinCosResult _scrz = Math.SinCos(zt);

            v.x = _scry.Cos * _scrz.Cos;
            v.y = -_scry.Cos * _scrz.Sin;
            v.z = _scry.Sin;

            return v;
        }

        public static Vector3 ToRightAxis(Vector3 vec)
            => ToRightAxis(vec.x, vec.y, vec.z);

        /// <summary>
        /// Returns the result of the linear interpolation between
        /// this vector and <paramref name="to"/> by amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination vector for interpolation.</param>
        /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting vector of the interpolation.</returns>
        public Vector3 Lerp(Vector3 to, float weight)
        {
            return new Vector3
            (
                Math.Lerp(x, to.x, weight),
                Math.Lerp(y, to.y, weight),
                Math.Lerp(z, to.z, weight)
            );
        }

        /// <summary>
        /// Returns the result of the linear interpolation between
        /// this vector and <paramref name="to"/> by the vector amount <paramref name="weight"/>.
        /// </summary>
        /// <param name="to">The destination vector for interpolation.</param>
        /// <param name="weight">A vector with components on the range of 0.0 to 1.0, representing the amount of interpolation.</param>
        /// <returns>The resulting vector of the interpolation.</returns>
        public Vector3 Lerp(Vector3 to, Vector3 weight)
        {
            return new Vector3
            (
                Math.Lerp(x, to.x, weight.x),
                Math.Lerp(y, to.y, weight.y),
                Math.Lerp(z, to.z, weight.z)
            );
        }

        /// <summary>
        /// Returns the vector with a maximum length by limiting its length to <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The length to limit to.</param>
        /// <returns>The vector with its length limited.</returns>
        public Vector3 LimitLength(float length = 1.0f)
        {
            Vector3 v = this;
            float l = Length();

            if (l > 0 && length < l)
            {
                v /= l;
                v *= length;
            }

            return v;
        }

        /// <summary>
        /// Returns the axis of the vector's largest value.
        /// If all components are equal, this method returns X.
        /// </summary>
        /// <returns>The value of the largest axis.</returns>
        public float MaxAxis()
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
        public float MinAxis()
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
        /// Moves this vector toward <paramref name="to"/> by the fixed <paramref name="delta"/> amount.
        /// </summary>
        /// <param name="to">The vector to move towards.</param>
        /// <param name="delta">The amount to move towards by.</param>
        /// <returns>The resulting vector.</returns>
        public Vector3 Move(Vector3 to, float delta)
        {
            Vector3 v = this;
            Vector3 vd = to - v;
            float len = vd.Length();
            if (len <= delta || len < Math.Epsilon)
                return to;

            return v + (vd / len * delta);
        }

        /// <summary>
        /// Returns the vector scaled to unit length. Equivalent to <c>v / v.Length()</c>.
        /// </summary>
        /// <returns>A normalized version of the vector.</returns>
        public Vector3 Normalized()
        {
            Vector3 v = this;
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
        public Vector3 PosMod(float mod)
        {
            Vector3 v;
            v.x = Math.PosMod(x, mod);
            v.y = Math.PosMod(y, mod);
            v.z = Math.PosMod(z, mod);
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
        public Vector3 PosMod(Vector3 modv)
        {
            Vector3 v;
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
        public Vector3 Project(Vector3 onNormal)
        {
            return onNormal * (Dot(onNormal) / onNormal.LengthSquared());
        }

        /// <summary>
        /// Returns this vector reflected from a plane defined by the given <paramref name="normal"/>.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to reflect from. Must be normalized.</param>
        /// <returns>The reflected vector.</returns>
        public Vector3 Reflect(Vector3 normal)
        {
#if DEBUG
            if (!normal.IsNormalized())
            {
                throw new ArgumentException("Argument is not normalized", nameof(normal));
            }
#endif
            return (2.0f * Dot(normal) * normal) - this;
        }

        /// <summary>
        /// Returns this vector with all components rounded to the nearest integer,
        /// with halfway cases rounded towards the nearest multiple of two.
        /// </summary>
        /// <returns>The rounded vector.</returns>
        public Vector3 Round()
        {
            return new Vector3(Math.Round(x), Math.Round(y), Math.Round(z));
        }

        /// <summary>
        /// Returns a vector with each component set to one or negative one, depending
        /// on the signs of this vector's components, or zero if the component is zero,
        /// by calling <see cref="Math.Sign(float)"/> on each component.
        /// </summary>
        /// <returns>A vector with all components as either <c>1</c>, <c>-1</c>, or <c>0</c>.</returns>
        public Vector3 Sign()
        {
            Vector3 v;
            v.x = Math.Sign(x);
            v.y = Math.Sign(y);
            v.z = Math.Sign(z);
            return v;
        }

        /// <summary>
        /// Returns the signed angle to the given vector, in radians.
        /// The sign of the angle is positive in a counter-clockwise
        /// direction and negative in a clockwise direction when viewed
        /// from the side specified by the <paramref name="axis"/>.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <param name="axis">The reference axis to use for the angle sign.</param>
        /// <returns>The signed angle between the two vectors, in radians.</returns>
        public float SignedAngleTo(Vector3 to, Vector3 axis)
        {
            Vector3 crossTo = Cross(to);
            float unsignedAngle = Math.Atan2(crossTo.Length(), Dot(to));
            float sign = crossTo.Dot(axis);
            return (sign < 0) ? -unsignedAngle : unsignedAngle;
        }

        /// <summary>
        /// Returns this vector slid along a plane defined by the given <paramref name="normal"/>.
        /// </summary>
        /// <param name="normal">The normal vector defining the plane to slide on.</param>
        /// <returns>The slid vector.</returns>
        public Vector3 Slide(Vector3 normal)
        {
            return this - (normal * Dot(normal));
        }

        /// <summary>
        /// Returns this vector with each component snapped to the nearest multiple of <paramref name="step"/>.
        /// This can also be used to round to an arbitrary number of decimals.
        /// </summary>
        /// <param name="step">A vector value representing the step size to snap to.</param>
        /// <returns>The snapped vector.</returns>
        public Vector3 Snap(Vector3 step)
        {
            return new Vector3
            (
                Math.Snap(x, step.x),
                Math.Snap(y, step.y),
                Math.Snap(z, step.z)
            );
        }

        // Constants
        private static readonly Vector3 _zero = new Vector3(0, 0, 0);
        private static readonly Vector3 _one = new Vector3(1, 1, 1);
        private static readonly Vector3 _inf = new Vector3(Math.Infinity, Math.Infinity, Math.Infinity);

        private static readonly Vector3 _up = new Vector3(0, 1, 0);
        private static readonly Vector3 _down = new Vector3(0, -1, 0);
        private static readonly Vector3 _right = new Vector3(1, 0, 0);
        private static readonly Vector3 _left = new Vector3(-1, 0, 0);
        private static readonly Vector3 _forward = new Vector3(0, 0, 1);
        private static readonly Vector3 _back = new Vector3(0, 0, -1);

        /// <summary>
        /// Zero vector, a vector with all components set to <c>0</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(0, 0, 0)</c>.</value>
        public static Vector3 Zero { get { return _zero; } }
        /// <summary>
        /// One vector, a vector with all components set to <c>1</c>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(1, 1, 1)</c>.</value>
        public static Vector3 One { get { return _one; } }
        /// <summary>
        /// Infinity vector, a vector with all components set to <see cref="Math.Infinity"/>.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(Math.Inf, Math.Inf, Math.Inf)</c>.</value>
        public static Vector3 Infinity { get { return _inf; } }

        /// <summary>
        /// Up unit vector.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(0, 1, 0)</c>.</value>
        public static Vector3 Up { get { return _up; } }
        /// <summary>
        /// Down unit vector.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(0, -1, 0)</c>.</value>
        public static Vector3 Down { get { return _down; } }
        /// <summary>
        /// Right unit vector. Represents the local direction of right,
        /// and the global direction of east.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(1, 0, 0)</c>.</value>
        public static Vector3 Right { get { return _right; } }
        /// <summary>
        /// Left unit vector. Represents the local direction of left,
        /// and the global direction of west.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(-1, 0, 0)</c>.</value>
        public static Vector3 Left { get { return _left; } }
        /// <summary>
        /// Forward unit vector. Represents the local direction of forward,
        /// and the global direction of north.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(0, 0, -1)</c>.</value>
        public static Vector3 Forward { get { return _forward; } }
        /// <summary>
        /// Back unit vector. Represents the local direction of back,
        /// and the global direction of south.
        /// </summary>
        /// <value>Equivalent to <c>new Vector3(0, 0, 1)</c>.</value>
        public static Vector3 Back { get { return _back; } }

        /// <summary>
        /// Constructs a new <see cref="Vector3"/> with the given components.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        /// <param name="z">The vector's Z component.</param>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        /// <summary>
        /// Constructs a new <see cref="Vector3"/> with the given components scaled by <paramref name="s"/>.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        /// <param name="z">The vector's Z component.</param>
        /// <param name="s">The vector's scalar value.</param>
        public Vector3(float x, float y, float z, float s)
        {
            this.x = x * s;
            this.y = y * s;
            this.z = z * s;
        }

        /// <summary>
        /// Constructs a new <see cref="Vector3"/> from an existing <see cref="Vector3"/>.
        /// </summary>
        /// <param name="v">The existing <see cref="Vector3"/>.</param>
        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.x += right.x;
            left.y += right.y;
            left.z += right.z;
            return left;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.x -= right.x;
            left.y -= right.y;
            left.z -= right.z;
            return left;
        }

        public static Vector3 operator -(Vector3 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            vec.z = -vec.z;
            return vec;
        }

        public static Vector3 operator *(Vector3 vec, float scale)
        {
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        public static Vector3 operator *(float scale, Vector3 vec)
        {
            vec.x *= scale;
            vec.y *= scale;
            vec.z *= scale;
            return vec;
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            left.x *= right.x;
            left.y *= right.y;
            left.z *= right.z;
            return left;
        }

        public static Vector3 operator /(Vector3 vec, float divisor)
        {
            vec.x /= divisor;
            vec.y /= divisor;
            vec.z /= divisor;
            return vec;
        }

        public static Vector3 operator /(Vector3 vec, Vector3 divisorv)
        {
            vec.x /= divisorv.x;
            vec.y /= divisorv.y;
            vec.z /= divisorv.z;
            return vec;
        }

        public static Vector3 operator %(Vector3 vec, float divisor)
        {
            vec.x %= divisor;
            vec.y %= divisor;
            vec.z %= divisor;
            return vec;
        }

        public static Vector3 operator %(Vector3 vec, Vector3 divisorv)
        {
            vec.x %= divisorv.x;
            vec.y %= divisorv.y;
            vec.z %= divisorv.z;
            return vec;
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Vector3 left, Vector3 right)
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

        public static bool operator >(Vector3 left, Vector3 right)
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

        public static bool operator <=(Vector3 left, Vector3 right)
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

        public static bool operator >=(Vector3 left, Vector3 right)
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

        public static implicit operator Vector3(Vector2 vec)
        {
            return new Vector3(vec.x, vec.y, 0f);
        }

        public static implicit operator Vector3(Vector3Int vec)
        {
            return new Vector3(vec.x, vec.y, vec.z);
        }

        public static implicit operator Vector3(Vector2Int vec)
        {
            return new Vector3(vec.x, vec.y, 0f);
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the vector and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                return Equals((Vector3)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are equal
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are equal.</returns>
        public bool Equals(Vector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are approximately equal,
        /// by running <see cref="Math.Approx(float, float)"/> on each component.
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are approximately equal.</returns>
        public bool Approx(Vector3 other)
        {
            return Math.Approx(x, other.x) && Math.Approx(y, other.y) && Math.Approx(z, other.z);
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Vector3"/>.
        /// </summary>
        /// <returns>A hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return y.GetHashCode() ^ x.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector3"/> to a string.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        /// <summary>
        /// Converts this <see cref="Vector3"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)})";
        }
    }
}
