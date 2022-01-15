using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct TMatrix : IEquatable<TMatrix>
    {
        private SinCosResult _scrx;
        private SinCosResult _scry;
        private SinCosResult _scrz;
        private float _xylen;
        private Vector3 _xyzscale;

        public Vector3 AxisX;
        public Vector3 AxisY;
        public Vector3 AxisZ;
        public Vector3 Position;
        public Vector3 AxisW;
        public float Clip;

        public float m00 { get => AxisX.x; set => AxisX.x = value; }
        public float m01 { get => AxisY.x; set => AxisY.x = value; }
        public float m02 { get => AxisZ.x; set => AxisZ.x = value; }
        public float m03 { get => Position.x; set => Position.x = value; }
        public float m10 { get => AxisX.y; set => AxisX.y = value; }
        public float m11 { get => AxisY.y; set => AxisY.y = value; }
        public float m12 { get => AxisZ.y; set => AxisZ.y = value; }
        public float m13 { get => Position.y; set => Position.y = value; }
        public float m20 { get => AxisX.z; set => AxisX.z = value; }
        public float m21 { get => AxisY.z; set => AxisY.z = value; }
        public float m22 { get => AxisZ.z; set => AxisZ.z = value; }
        public float m23 { get => Position.z; set => Position.z = value; }
        public float m30 { get => AxisW.x; set => AxisW.z = value; }
        public float m31 { get => AxisW.y; set => Clip = value; }
        public float m32 { get => AxisW.z; set => AxisW.z = value; }
        public float m33 { get => Clip; set => Clip = value; }

        public float this[int row, int column] => column switch
        {
            0 => row switch
            {
                0 => m00,
                1 => m01,
                2 => m02,
                3 => m03,
                _ => 0
            },
            1 => row switch
            {
                0 => m10,
                1 => m11,
                2 => m12,
                3 => m13,
                _ => 0
            },
            2 => row switch
            {
                0 => m20,
                1 => m21,
                2 => m22,
                3 => m23,
                _ => 0
            },
            3 => row switch
            {
                0 => m30,
                1 => m31,
                2 => m32,
                3 => m33,
                _ => 0
            },

            _ => 0
        };

        public override bool Equals(object obj)
        {
            if (obj is TMatrix)
            {
                return Equals((TMatrix)obj);
            }

            return false;
        }

        public bool Equals(TMatrix other)
        {
            return 
                m00 == other.m00 && m01 == other.m01 && m02 == other.m02 && m03 == other.m03 && 
                m10 == other.m10 && m11 == other.m11 && m12 == other.m12 && m13 == other.m13 && 
                m20 == other.m20 && m21 == other.m21 && m22 == other.m22 && m23 == other.m23 && 
                m30 == other.m30 && m31 == other.m31 && m32 == other.m32 && m33 == other.m33;
        }

        public override int GetHashCode()
        {
            return
                m00.GetHashCode() ^ m01.GetHashCode() ^ m02.GetHashCode() ^ m03.GetHashCode() ^
                m10.GetHashCode() ^ m11.GetHashCode() ^ m12.GetHashCode() ^ m13.GetHashCode() ^
                m20.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode() ^ m23.GetHashCode() ^
                m30.GetHashCode() ^ m31.GetHashCode() ^ m32.GetHashCode() ^ m33.GetHashCode();
        }

        public static bool operator ==(TMatrix left, TMatrix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TMatrix left, TMatrix right)
        {
            return !(left == right);
        }

        public static TMatrix operator +(TMatrix a, TMatrix b)
        {
            a.m00 += b.m00;
            a.m01 += b.m01;
            a.m02 += b.m02;
            a.m03 += b.m03;
            a.m10 += b.m10;
            a.m11 += b.m11;
            a.m12 += b.m12;
            a.m13 += b.m13;
            a.m20 += b.m20;
            a.m21 += b.m21;
            a.m22 += b.m22;
            a.m23 += b.m23;
            a.m30 += b.m30;
            a.m31 += b.m31;
            a.m32 += b.m32;
            a.m33 += b.m33;

            return a;
        }

        public static TMatrix operator -(TMatrix a, TMatrix b)
        {
            a.m00 -= b.m00;
            a.m01 -= b.m01;
            a.m02 -= b.m02;
            a.m03 -= b.m03;
            a.m10 -= b.m10;
            a.m11 -= b.m11;
            a.m12 -= b.m12;
            a.m13 -= b.m13;
            a.m20 -= b.m20;
            a.m21 -= b.m21;
            a.m22 -= b.m22;
            a.m23 -= b.m23;
            a.m30 -= b.m30;
            a.m31 -= b.m31;
            a.m32 -= b.m32;
            a.m33 -= b.m33;

            return a;
        }

        public static TMatrix operator *(TMatrix a, TMatrix b)
        {
            TMatrix m = new TMatrix();
            m.m00 = a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30;
            m.m01 = a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31;
            m.m02 = a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32;
            m.m03 = a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33;
            m.m10 = a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30;
            m.m11 = a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
            m.m12 = a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
            m.m13 = a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;
            m.m20 = a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30;
            m.m21 = a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31;
            m.m22 = a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32;
            m.m23 = a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33;
            m.m30 = a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30;
            m.m31 = a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m20 + a.m33 * b.m31;
            m.m32 = a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m20 + a.m33 * b.m32;
            m.m33 = a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m20 + a.m33 * b.m33;

            return a;
        }

        public Vector3 MultiplyOrtho(Vector3 b)
        {
            Vector3 v = new Vector3();
            v.x = b.x * m00 + b.x * m01 + b.x * m02 + b.x * m03;
            v.y = b.y * m00 + b.y * m01 + b.y * m02 + b.y * m03;
            v.z = b.z * m00 + b.z * m01 + b.z * m02 + b.z * m03;
            return v;
        }

        internal void Modify(Transform transform)
        {
            AxisX = transform.Right;
            AxisY = transform.Up;
            AxisZ = transform.Forward;

            AxisX.x *= transform.Scale.x;
            AxisY.y *= transform.Scale.y;
            AxisZ.z *= transform.Scale.z;

            Position = transform.Position;
        }

        internal void ModifyView(Transform transform)
        {
            AxisX = Vector3.ToRightAxis(-transform.Rotation);
            AxisY = Vector3.ToUpAxis(-transform.Rotation);
            AxisZ = Vector3.ToForwardAxis(-transform.Rotation);

            AxisX.x = Math.DivideCatchZero(AxisX.x, transform.Scale.x);
            AxisY.y = Math.DivideCatchZero(AxisY.y, transform.Scale.y);
            AxisZ.z = Math.DivideCatchZero(AxisZ.z, transform.Scale.z);

            Position = MultiplyOrtho(-transform.Position) + transform.Position;
        }

        internal Transform GetTransform()
        {
            Transform t = new Transform();

            t.Position = Position;

            _xylen = Math.Length(AxisZ.x, AxisZ.y);
            if (_xylen > Math.Epsilon)
                t.Rotation = new Vector3(
                    Math.Atan2(AxisY.x * AxisZ.y - AxisY.y, AxisX.x * AxisZ.y - AxisZ.x),
                    Math.Atan2(_xylen, AxisZ.z),
                    Math.Atan2(-AxisZ.x, AxisZ.y));
            else
                t.Rotation = new Vector3(0f, (AxisZ.z > 0f) ? 0f : 90f, -Math.Atan2(AxisX.y, AxisX.x));

            
            t.Scale = new Vector3(AxisX.Length(), AxisY.Length(), AxisZ.Length());

            return t;
        }

        internal void ModifyProjection(float fov, float aspect, float near, float far, float scale)
        {
            if (fov < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            _xylen = Math.Tan(fov * 0.5f);
            AxisX.x = scale / (aspect * _xylen);
            AxisY.y = scale / _xylen;
            AxisZ.z = (scale * far) / (near - far);
            Position.z = -1f;
            AxisW.z = -(far * near) / (far - near);
        }

        internal void ModifyLookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            AxisX = (target - eye).Normalized();
            AxisY = AxisX.Cross(up).Normalized();
            AxisZ = -AxisY.Cross(AxisX);

            AxisW.x = -AxisX.Dot(eye);
            AxisW.y = -AxisY.Dot(eye);
            AxisW.z = -AxisZ.Dot(eye);
        }

        internal unsafe void ToArrayPtr(float* ptr)
        {
            // XY
            //ptr[0] = m00;
            //ptr[1] = m01;
            //ptr[2] = m02;
            //ptr[3] = m03;
            //ptr[4] = m10;
            //ptr[5] = m11;
            //ptr[6] = m12;
            //ptr[7] = m13;
            //ptr[8] = m20;
            //ptr[9] = m21;
            //ptr[10] = m22;
            //ptr[11] = m23;
            //ptr[12] = m30;
            //ptr[13] = m31;
            //ptr[14] = m32;
            //ptr[15] = m33;

            // YX
            ptr[0] = m00;
            ptr[1] = m10;
            ptr[2] = m20;
            ptr[3] = m30;
            ptr[4] = m01;
            ptr[5] = m11;
            ptr[6] = m21;
            ptr[7] = m31;
            ptr[8] = m02;
            ptr[9] = m12;
            ptr[10] = m22;
            ptr[11] = m32;
            ptr[12] = m03;
            ptr[13] = m31;
            ptr[14] = m32;
            ptr[15] = m33;
        }

        private class TMatrixFovLessThanEqualToZeroException : Exception
        {
            public TMatrixFovLessThanEqualToZeroException() : base("Field of view angle cannot be less than or equal to zero.")
            {
            }
        }

        /// <summary>
        /// Creates a right-handed projection <see cref="TMatrix"/>.
        /// </summary>
        /// <param name="fov">The field of view of the projection.</param>
        /// <param name="near">The near clip-plane.</param>
        /// <param name="far">The far clip-plane.</param>
        public TMatrix(float fov, float aspect, float near, float far, float scale)
        {
            // Init
            _xylen = 0f;
            _xyzscale = Vector3.Zero;
            _scrx = new SinCosResult();
            _scry = new SinCosResult();
            _scrz = new SinCosResult();

            AxisX = Vector3.Right;
            AxisY = Vector3.Up;
            AxisZ = Vector3.Forward;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init

            if (fov < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            // C++
            // T const tanHalfFovy = tan(fovy / static_cast<T>(2));

            // mat < 4, 4, T, defaultp > Result(static_cast<T>(0));
            // Result[0][0] = static_cast<T>(1) / (aspect * tanHalfFovy);
            // Result[1][1] = static_cast<T>(1) / (tanHalfFovy);
            // Result[2][2] = zFar / (zNear - zFar);
            // Result[2][3] = -static_cast<T>(1);
            // Result[3][2] = -(zFar * zNear) / (zFar - zNear);
            // return Result;

            _xylen = Math.Tan(fov * 0.5f);
            AxisX.x = scale / (aspect * _xylen);
            AxisY.y = scale / _xylen;
            AxisZ.z = (scale * far) / (near - far);
            Position.z = -1f;
            AxisW.z = -(far * near) / (far - near);
        }

        /// <summary>
        /// Creates a right-handed look-at <see cref="TMatrix"/>
        /// </summary>
        /// <param name="eye"></param>
        /// <param name="target"></param>
        /// <param name="up"></param>
        public TMatrix(Vector3 eye, Vector3 target, Vector3 up)
        {
            // Init
            _xylen = 0f;
            _xyzscale = Vector3.Zero;
            _scrx = new SinCosResult();
            _scry = new SinCosResult();
            _scrz = new SinCosResult();

            AxisX = Vector3.Right;
            AxisY = Vector3.Up;
            AxisZ = Vector3.Forward;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init

            // C++
            // vec <3, T, Q> const f(normalize(center - eye));
            // vec <3, T, Q> const s(normalize(cross(f, up)));
            // vec <3, T, Q> const u(cross(s, f));

            // mat <4, 4, T, Q> Result(1);
            // Result[0][0] = s.x;
            // Result[1][0] = s.y;
            // Result[2][0] = s.z;
            // Result[0][1] = u.x;
            // Result[1][1] = u.y;
            // Result[2][1] = u.z;
            // Result[0][2] = -f.x;
            // Result[1][2] = -f.y;
            // Result[2][2] = -f.z;
            // Result[3][0] = -dot(s, eye);
            // Result[3][1] = -dot(u, eye);
            // Result[3][2] = dot(f, eye);
            // return Result;

            AxisX = (target - eye).Normalized();
            AxisY = AxisX.Cross(up).Normalized();
            AxisZ = -AxisY.Cross(AxisX);

            AxisW.x = -AxisX.Dot(eye);
            AxisW.y = -AxisY.Dot(eye);
            AxisW.z = -AxisZ.Dot(eye);
        }

        /// <summary>
        /// Constructs a <see cref="TMatrix"/> from a <see cref="Transform"/> applying rotation in XYZ order.
        /// </summary>
        /// <param name="transform"></param>
        public TMatrix(Transform transform)
        {
            // Init
            _xylen = 0f;
            _xyzscale = Vector3.Zero;
            _scrx = new SinCosResult();
            _scry = new SinCosResult();
            _scrz = new SinCosResult();

            AxisX = Vector3.Right;
            AxisY = Vector3.Up;
            AxisZ = Vector3.Forward;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init

            AxisX = transform.Right;
            AxisY = transform.Up;
            AxisZ = transform.Forward;

            AxisX.x *= transform.Scale.x;
            AxisY.y *= transform.Scale.y;
            AxisZ.z *= transform.Scale.z;

            Position = transform.Position;
        }

        public TMatrix(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            // Init
            _xylen = 0f;
            _xyzscale = Vector3.Zero;
            _scrx = new SinCosResult();
            _scry = new SinCosResult();
            _scrz = new SinCosResult();

            AxisX = Vector3.Right;
            AxisY = Vector3.Up;
            AxisZ = Vector3.Forward;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init

            AxisX.x = m00;
            AxisX.y = m10;
            AxisX.z = m20;
            AxisY.x = m01;
            AxisY.y = m11;
            AxisY.z = m21;
            AxisZ.x = m02;
            AxisZ.y = m12;
            AxisZ.z = m22;
            Position.x = m03;
            Position.y = m13;
            Position.z = m23;
            AxisW.x = m30;
            AxisW.y = m31;
            AxisW.z = m32;
            Clip = m33;
        }

        private static readonly TMatrix identity = new TMatrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
        public static TMatrix Identity => identity;
    }
}
