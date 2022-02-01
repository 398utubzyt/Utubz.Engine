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
        public float m31 { get => AxisW.y; set => AxisW.y = value; }
        public float m32 { get => AxisW.z; set => AxisW.z = value; }
        public float m33 { get => Clip; set => Clip = value; }

        public float this[int index] 
        { 
            get => index switch
            {
                0 => m00,
                1 => m01,
                2 => m02,
                3 => m03,
                4 => m10,
                5 => m11,
                6 => m12,
                7 => m13,
                8 => m20,
                9 => m21,
                10 => m22,
                11 => m23,
                12 => m30,
                13 => m31,
                14 => m32,
                15 => m33,

                _ => 0
            };
            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m03 = value; break;
                    case 4: m10 = value; break;
                    case 5: m11 = value; break;
                    case 6: m12 = value; break;
                    case 7: m13 = value; break;
                    case 8: m20 = value; break;
                    case 9: m21 = value; break;
                    case 10: m22 = value; break;
                    case 11: m23 = value; break;
                    case 12: m30 = value; break;
                    case 13: m31 = value; break;
                    case 14: m32 = value; break;
                    case 15: m33 = value; break;
                }
            }
        }

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

        public override string ToString()
        {
            return $"\nX Axis: {AxisX}\nY Axis: {AxisY}\nZ Axis: {AxisZ}\nW Axis: {AxisW}\nP Axis: {Position}\nClip: {Clip}";
        }

        public static bool operator ==(TMatrix left, TMatrix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TMatrix left, TMatrix right)
        {
            return !(left == right);
        }

        public static TMatrix operator *(TMatrix a, float b)
        {
            a.m00 *= b;
            a.m01 *= b;
            a.m02 *= b;
            a.m03 *= b;
            a.m10 *= b;
            a.m11 *= b;
            a.m12 *= b;
            a.m13 *= b;
            a.m20 *= b;
            a.m21 *= b;
            a.m22 *= b;
            a.m23 *= b;
            a.m30 *= b;
            a.m31 *= b;
            a.m32 *= b;
            a.m33 *= b;

            return a;
        }

        public static TMatrix operator /(TMatrix a, float b)
        {
            a.m00 /= b;
            a.m01 /= b;
            a.m02 /= b;
            a.m03 /= b;
            a.m10 /= b;
            a.m11 /= b;
            a.m12 /= b;
            a.m13 /= b;
            a.m20 /= b;
            a.m21 /= b;
            a.m22 /= b;
            a.m23 /= b;
            a.m30 /= b;
            a.m31 /= b;
            a.m32 /= b;
            a.m33 /= b;

            return a;
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
            /*
                Matrix4 out;
                out.m[0]  = b.m[0]*m[0]  + b.m[4]*m[1]  + b.m[8] *m[2]  + b.m[12]*m[3];
                out.m[1]  = b.m[1]*m[0]  + b.m[5]*m[1]  + b.m[9] *m[2]  + b.m[13]*m[3];
                out.m[2]  = b.m[2]*m[0]  + b.m[6]*m[1]  + b.m[10]*m[2]  + b.m[14]*m[3];
                out.m[3]  = b.m[3]*m[0]  + b.m[7]*m[1]  + b.m[11]*m[2]  + b.m[15]*m[3];

                out.m[4]  = b.m[0]*m[4]  + b.m[4]*m[5]  + b.m[8] *m[6]  + b.m[12]*m[7];
                out.m[5]  = b.m[1]*m[4]  + b.m[5]*m[5]  + b.m[9] *m[6]  + b.m[13]*m[7];
                out.m[6]  = b.m[2]*m[4]  + b.m[6]*m[5]  + b.m[10]*m[6]  + b.m[14]*m[7];
                out.m[7]  = b.m[3]*m[4]  + b.m[7]*m[5]  + b.m[11]*m[6]  + b.m[15]*m[7];

                out.m[8]  = b.m[0]*m[8]  + b.m[4]*m[9]  + b.m[8] *m[10] + b.m[12]*m[11];
                out.m[9]  = b.m[1]*m[8]  + b.m[5]*m[9]  + b.m[9] *m[10] + b.m[13]*m[11];
                out.m[10] = b.m[2]*m[8]  + b.m[6]*m[9]  + b.m[10]*m[10] + b.m[14]*m[11];
                out.m[11] = b.m[3]*m[8]  + b.m[7]*m[9]  + b.m[11]*m[10] + b.m[15]*m[11];

                out.m[12] = b.m[0]*m[12] + b.m[4]*m[13] + b.m[8] *m[14] + b.m[12]*m[15];
                out.m[13] = b.m[1]*m[12] + b.m[5]*m[13] + b.m[9] *m[14] + b.m[13]*m[15];
                out.m[14] = b.m[2]*m[12] + b.m[6]*m[13] + b.m[10]*m[14] + b.m[14]*m[15];
                out.m[15] = b.m[3]*m[12] + b.m[7]*m[13] + b.m[11]*m[14] + b.m[15]*m[15];
                return out;
             */

            TMatrix m = new TMatrix();
            m[0] = b[0] * a[0] + b[4] * a[1] + b[8] * a[2] + b[12] * a[3];
            m[1] = b[1] * a[0] + b[5] * a[1] + b[9] * a[2] + b[13] * a[3];
            m[2] = b[2] * a[0] + b[6] * a[1] + b[10] * a[2] + b[14] * a[3];
            m[3] = b[3] * a[0] + b[7] * a[1] + b[11] * a[2] + b[15] * a[3];

            m[4] = b[0] * a[4] + b[4] * a[5] + b[8] * a[6] + b[12] * a[7];
            m[5] = b[1] * a[4] + b[5] * a[5] + b[9] * a[6] + b[13] * a[7];
            m[6] = b[2] * a[4] + b[6] * a[5] + b[10] * a[6] + b[14] * a[7];
            m[7] = b[3] * a[4] + b[7] * a[5] + b[11] * a[6] + b[15] * a[7];

            m[8] = b[0] * a[8] + b[4] * a[9] + b[8] * a[10] + b[12] * a[11];
            m[9] = b[1] * a[8] + b[5] * a[9] + b[9] * a[10] + b[13] * a[11];
            m[10] = b[2] * a[8] + b[6] * a[9] + b[10] * a[10] + b[14] * a[11];
            m[11] = b[3] * a[8] + b[7] * a[9] + b[11] * a[10] + b[15] * a[11];

            m[12] = b[0] * a[12] + b[4] * a[13] + b[8] * a[14] + b[12] * a[15];
            m[13] = b[1] * a[12] + b[5] * a[13] + b[9] * a[14] + b[13] * a[15];
            m[14] = b[2] * a[12] + b[6] * a[13] + b[10] * a[14] + b[14] * a[15];
            m[15] = b[3] * a[12] + b[7] * a[13] + b[11] * a[14] + b[15] * a[15];

            return m;
        }

        public TMatrix Inverse { get
            {
                /* C++
                  //Inverse
                  Matrix4 Inverse() const {
                    Matrix4 inv;

                    inv.m[0] = m[5] * m[10] * m[15] -
                      m[5] * m[11] * m[14] -
                      m[9] * m[6] * m[15] +
                      m[9] * m[7] * m[14] +
                      m[13] * m[6] * m[11] -
                      m[13] * m[7] * m[10];
                 */
                TMatrix inv = new TMatrix();

                inv.m00 =
                    m11 * m22 * m33 -
                    m11 * m23 * m32 -
                    m21 * m12 * m33 +
                    m21 * m13 * m32 +
                    m31 * m12 * m23 -
                    m31 * m13 * m22;
                /*
                
                inv.m[4] = -m[4] * m[10] * m[15] +
                      m[4] * m[11] * m[14] +
                      m[8] * m[6] * m[15] -
                      m[8] * m[7] * m[14] -
                      m[12] * m[6] * m[11] +
                      m[12] * m[7] * m[10];
                 */
                inv.m10 =
                   -m10 * m22 * m33 +
                    m10 * m23 * m32 +
                    m20 * m12 * m33 -
                    m20 * m13 * m32 -
                    m30 * m12 * m23 +
                    m30 * m13 * m22;
                /*

                    inv.m[8] = m[4] * m[9] * m[15] -
                      m[4] * m[11] * m[13] -
                      m[8] * m[5] * m[15] +
                      m[8] * m[7] * m[13] +
                      m[12] * m[5] * m[11] -
                      m[12] * m[7] * m[9];
                 */
                inv.m20 =
                    m10 * m21 * m33 -
                    m10 * m23 * m31 -
                    m20 * m11 * m33 +
                    m20 * m13 * m31 +
                    m30 * m11 * m23 -
                    m30 * m13 * m21;
                /*

                    inv.m[12] = -m[4] * m[9] * m[14] +
                      m[4] * m[10] * m[13] +
                      m[8] * m[5] * m[14] -
                      m[8] * m[6] * m[13] -
                      m[12] * m[5] * m[10] +
                      m[12] * m[6] * m[9];
                 */
                inv.m30 =
                   -m10 * m21 * m32 +
                    m10 * m22 * m31 +
                    m20 * m11 * m32 -
                    m20 * m12 * m31 -
                    m30 * m11 * m22 +
                    m30 * m12 * m21;
                /*

                    inv.m[1] = -m[1] * m[10] * m[15] +
                      m[1] * m[11] * m[14] +
                      m[9] * m[2] * m[15] -
                      m[9] * m[3] * m[14] -
                      m[13] * m[2] * m[11] +
                      m[13] * m[3] * m[10];
                 */
                inv.m01 =
                   -m01 * m22 * m33 +
                    m01 * m23 * m32 +
                    m21 * m02 * m33 -
                    m21 * m03 * m32 -
                    m31 * m02 * m23 +
                    m31 * m03 * m22;
                /*

                    inv.m[5] = m[0] * m[10] * m[15] -
                      m[0] * m[11] * m[14] -
                      m[8] * m[2] * m[15] +
                      m[8] * m[3] * m[14] +
                      m[12] * m[2] * m[11] -
                      m[12] * m[3] * m[10];
                 */
                inv.m11 =
                    m00 * m22 * m33 -
                    m00 * m23 * m32 -
                    m20 * m02 * m33 +
                    m20 * m03 * m32 +
                    m30 * m02 * m23 -
                    m30 * m03 * m22;
                /*

                    inv.m[9] = -m[0] * m[9] * m[15] +
                      m[0] * m[11] * m[13] +
                      m[8] * m[1] * m[15] -
                      m[8] * m[3] * m[13] -
                      m[12] * m[1] * m[11] +
                      m[12] * m[3] * m[9];
                 */
                inv.m21 =
                   -m00 * m21 * m33 +
                    m00 * m23 * m31 +
                    m20 * m01 * m33 -
                    m20 * m03 * m31 -
                    m30 * m01 * m23 +
                    m30 * m03 * m21;
                /*

                    inv.m[13] = m[0] * m[9] * m[14] -
                      m[0] * m[10] * m[13] -
                      m[8] * m[1] * m[14] +
                      m[8] * m[2] * m[13] +
                      m[12] * m[1] * m[10] -
                      m[12] * m[2] * m[9];
                 */
                inv.m31 =
                    m00 * m21 * m32 -
                    m00 * m22 * m31 -
                    m20 * m01 * m32 +
                    m20 * m02 * m31 +
                    m30 * m01 * m22 -
                    m30 * m02 * m21;
                /*

                    inv.m[2] = m[1] * m[6] * m[15] -
                      m[1] * m[7] * m[14] -
                      m[5] * m[2] * m[15] +
                      m[5] * m[3] * m[14] +
                      m[13] * m[2] * m[7] -
                      m[13] * m[3] * m[6];
                 */
                inv.m02 =
                    this[1] * this[6] * this[15] -
                    this[1] * this[7] * this[14] -
                    this[5] * this[2] * this[15] +
                    this[5] * this[3] * this[14] +
                    this[13] * this[2] * this[7] -
                    this[13] * this[3] * this[6];
                /*

                    inv.m[6] = -m[0] * m[6] * m[15] +
                      m[0] * m[7] * m[14] +
                      m[4] * m[2] * m[15] -
                      m[4] * m[3] * m[14] -
                      m[12] * m[2] * m[7] +
                      m[12] * m[3] * m[6];
                 */
                inv.m12 =
                   -this[0] * this[6] * this[15] +
                    this[0] * this[7] * this[14] +
                    this[4] * this[2] * this[15] -
                    this[4] * this[3] * this[14] -
                    this[12] * this[2] * this[7] +
                    this[12] * this[3] * this[6];
                /*

                    inv.m[10] = m[0] * m[5] * m[15] -
                      m[0] * m[7] * m[13] -
                      m[4] * m[1] * m[15] +
                      m[4] * m[3] * m[13] +
                      m[12] * m[1] * m[7] -
                      m[12] * m[3] * m[5];
                 */
                inv.m22 =
                    this[0] * this[5] * this[15] -
                    this[0] * this[7] * this[13] -
                    this[4] * this[1] * this[15] +
                    this[4] * this[3] * this[13] +
                    this[12] * this[1] * this[7] -
                    this[12] * this[3] * this[5];
                /*

                    inv.m[14] = -m[0] * m[5] * m[14] +
                      m[0] * m[6] * m[13] +
                      m[4] * m[1] * m[14] -
                      m[4] * m[2] * m[13] -
                      m[12] * m[1] * m[6] +
                      m[12] * m[2] * m[5];
                 */
                inv.m32 =
                   -this[0] * this[5] * this[14] +
                    this[0] * this[6] * this[13] +
                    this[4] * this[1] * this[14] -
                    this[4] * this[2] * this[13] -
                    this[12] * this[1] * this[6] +
                    this[12] * this[2] * this[5];
                /*

                    inv.m[3] = -m[1] * m[6] * m[11] +
                      m[1] * m[7] * m[10] +
                      m[5] * m[2] * m[11] -
                      m[5] * m[3] * m[10] -
                      m[9] * m[2] * m[7] +
                      m[9] * m[3] * m[6];
                 */
                inv.m03 =
                   -this[1] * this[6] * this[11] +
                    this[1] * this[7] * this[10] +
                    this[5] * this[2] * this[11] -
                    this[5] * this[3] * this[10] -
                    this[9] * this[2] * this[7] +
                    this[9] * this[3] * this[6];
                /*

                    inv.m[7] = m[0] * m[6] * m[11] -
                      m[0] * m[7] * m[10] -
                      m[4] * m[2] * m[11] +
                      m[4] * m[3] * m[10] +
                      m[8] * m[2] * m[7] -
                      m[8] * m[3] * m[6];
                 */
                inv.m13 =
                    this[0] * this[6] * this[11] -
                    this[0] * this[7] * this[10] -
                    this[4] * this[2] * this[11] +
                    this[4] * this[3] * this[10] +
                    this[8] * this[2] * this[7] -
                    this[8] * this[3] * this[6];
                /*

                    inv.m[11] = -m[0] * m[5] * m[11] +
                      m[0] * m[7] * m[9] +
                      m[4] * m[1] * m[11] -
                      m[4] * m[3] * m[9] -
                      m[8] * m[1] * m[7] +
                      m[8] * m[3] * m[5];
                 */
                inv.m23 =
                   -this[0] * this[5] * this[11] +
                    this[0] * this[7] * this[9] +
                    this[4] * this[1] * this[11] -
                    this[4] * this[3] * this[9] -
                    this[8] * this[1] * this[7] +
                    this[8] * this[3] * this[5];
                /*

                    inv.m[15] = m[0] * m[5] * m[10] -
                      m[0] * m[6] * m[9] -
                      m[4] * m[1] * m[10] +
                      m[4] * m[2] * m[9] +
                      m[8] * m[1] * m[6] -
                      m[8] * m[2] * m[5];
                 */
                inv.m33 =
                    this[0] * this[5] * this[10] -
                    this[0] * this[6] * this[9] -
                    this[4] * this[1] * this[10] +
                    this[4] * this[2] * this[9] +
                    this[8] * this[1] * this[6] -
                    this[8] * this[2] * this[5];
                /*

                    const float det = m[0] * inv.m[0] + m[1] * inv.m[4] + m[2] * inv.m[8] + m[3] * inv.m[12];
                    inv /= det;
                    return inv;
                  }
                 */
                inv /= m00 * inv.m00 + m01 * inv.m10 + m02 * inv.m20 + m03 * inv.m30;
                return inv;
            }
        }

        public Vector3 MultiplyOrtho(Vector3 b)
        {
            Vector3 v = new Vector3();
            v.x = b.x * m00 + b.x * m01 + b.x * m02 + b.x * m03;
            v.y = b.y * m00 + b.y * m01 + b.y * m02 + b.y * m03;
            v.z = b.z * m00 + b.z * m01 + b.z * m02 + b.z * m03;
            return v;
        }

        public static TMatrix Translation(Vector3 p)
        {
            return Identity.Translate(p);
        }

        public static TMatrix Rotation(Vector3 v)
        {
            return RotationZ(v.z) * RotationX(v.x) * RotationY(v.y);
        }

        public static TMatrix RotationX(float a)
        {
            return Identity.RotateX(a);
        }

        public static TMatrix RotationY(float a)
        {
            return Identity.RotateY(a);
        }

        public static TMatrix RotationZ(float a)
        {
            return Identity.RotateZ(a);
        }

        public static TMatrix Dilation(Vector3 d)
        {
            return Identity.Dilate(d);
        }

        public TMatrix Translate(Vector3 p)
        {
            return Translate(this, p);
        }

        public TMatrix Rotate(Vector3 v)
        {
            return Rotate(this, v);
        }

        public TMatrix RotateX(float a)
        {
            return RotateX(this, a);
        }

        public TMatrix RotateY(float a)
        {
            return RotateY(this, a);
        }

        public TMatrix RotateZ(float a)
        {
            return RotateZ(this, a);
        }

        public TMatrix Dilate(Vector3 d)
        {
            return Dilate(this, d);
        }

        private TMatrix Translate(TMatrix t, Vector3 p)
        {
            t.Position = p;
            return t;
        }

        private TMatrix Rotate(TMatrix t, Vector3 a)
        {
            //_scrx = Math.SinCos(a.z);
            //_scry = Math.SinCos(a.y);
            //_scrz = Math.SinCos(a.x);

            //t.AxisY.y += Math.Cos(a.x);
            //t.AxisY.z += Math.Sin(a.x);
            //t.AxisZ.y += -Math.Sin(a.x);
            //t.AxisZ.z += Math.Cos(a.x);

            //t.AxisX.x += Math.Cos(a.y);
            //t.AxisX.z += -Math.Sin(a.y);
            //t.AxisZ.x += Math.Sin(a.y);
            //t.AxisZ.z += Math.Cos(a.y);

            //t.AxisX.x += Math.Cos(a.z);
            //t.AxisX.y += Math.Sin(a.z);
            //t.AxisY.x += -Math.Sin(a.z);
            //t.AxisY.y += Math.Cos(a.z);

            //t.AxisX.x = _scry.Cos * _scrz.Cos;
            //t.AxisX.y = _scry.Cos * _scrz.Sin;
            //t.AxisX.z = -_scry.Sin;

            //t.AxisY.x = _scrx.Sin * _scry.Sin * _scrz.Cos - _scrx.Cos * _scrz.Sin;
            //t.AxisY.y = _scrx.Sin * _scry.Sin * _scrz.Sin + _scrx.Cos * _scrz.Cos;
            //t.AxisY.z = _scrx.Sin * _scry.Cos;

            //t.AxisZ.x = _scrx.Cos * _scry.Sin * _scrz.Cos + _scrx.Sin * _scrz.Sin;
            //t.AxisZ.y = _scrx.Cos * _scry.Sin * _scrz.Sin - _scrx.Sin * _scrz.Cos;
            //t.AxisZ.z = _scrx.Cos * _scry.Cos;
            return t.RotateZ(a.z) * t.RotateX(a.x) * t.RotateY(a.y);
        }

        private TMatrix RotateX(TMatrix t, float a)
        {
            t.AxisY.y = Math.Cos(a);
            t.AxisY.z = -Math.Sin(a);
            t.AxisZ.y = Math.Sin(a);
            t.AxisZ.z = Math.Cos(a);
            return t;
        }

        private TMatrix RotateY(TMatrix t, float a)
        {
            t.AxisX.x = Math.Cos(a);
            t.AxisX.z = Math.Sin(a);
            t.AxisZ.x = -Math.Sin(a);
            t.AxisZ.z = Math.Cos(a);
            return t;
        }

        private TMatrix RotateZ(TMatrix t, float a)
        {
            t.AxisX.x = Math.Cos(a);
            t.AxisX.y = -Math.Sin(a);
            t.AxisY.x = Math.Sin(a);
            t.AxisY.y = Math.Cos(a);
            return t;
        }

        private TMatrix Dilate(TMatrix t, Vector3 d)
        {
            t.AxisX.x *= d.x;
            t.AxisY.y *= d.y;
            t.AxisZ.z *= d.z;
            return t;
        }

        internal void ModifyView(Transform transform)
        {
            AxisX = Vector3.ToRightAxis(transform.Rotation);
            AxisY = Vector3.ToUpAxis(transform.Rotation);
            AxisZ = Vector3.ToForwardAxis(transform.Rotation);

            //AxisX.x = Math.DivideCatchZero(AxisX.x, transform.Scale.x);
            //AxisY.y = Math.DivideCatchZero(AxisY.y, transform.Scale.y);
            //AxisZ.z = Math.DivideCatchZero(AxisZ.z, transform.Scale.z);

            Position = -transform.Position;
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

        internal void ModifyPerspective(float fov, float aspect, float near, float far, float scale)
        {
            // C++
            // T const tanHalfFovy = tan(fovy / static_cast<T>(2));

            // mat < 4, 4, T, defaultp > Result(static_cast<T>(0));
            // Result[0][0] = static_cast<T>(1) / (aspect * tanHalfFovy);
            // Result[1][1] = static_cast<T>(1) / (tanHalfFovy);
            // Result[2][2] = -(zFar + zNear) / (zFar - zNear);
            // Result[2][3] = -static_cast<T>(1);
            // Result[3][2] = -(static_cast<T>(2) * zFar * zNear) / (zFar - zNear);
            // return Result;

            if (fov < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            AxisX = Vector3.Zero;
            AxisY = Vector3.Zero;
            AxisZ = Vector3.Zero;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 0f;

            _xylen = Math.Tan(fov * 0.5f);
            AxisX.x = 1f / (aspect * _xylen);
            AxisY.y = 1f / _xylen;
            AxisZ.z = (near + far) / (near - far);
            AxisW.z = -1f;
            Position.z = (2f * near * far) / (near - far);
        }

        internal void ModifyOrthographic(float size, float aspect, float near, float far)
        {
            if (size < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            AxisX.x = 2f / (size * aspect);
            AxisY.y = 2f / size;
            AxisZ.z = -2f / (far - near);
            Position.x = 0f;
            Position.y = 0f;
            Position.z = -(far + near) / (far - near);
            AxisW = Vector3.Zero;
            Clip = 1f;
        }

        internal void ModifyLookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
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

            //_xyzscale = (target - eye).Normalized();   // f
            //target = up.Cross(_xyzscale).Normalized(); // s
            //up = _xyzscale.Cross(target);              // u

            //AxisX.x = target.x;
            //AxisY.x = target.y;
            //AxisZ.x = target.z;
            //AxisX.y = up.x;
            //AxisY.y = up.y;
            //AxisZ.y = up.z;
            //AxisX.z = _xyzscale.x;
            //AxisY.z = _xyzscale.y;
            //AxisZ.z = _xyzscale.z;

            //Position.x = -target.Dot(eye);
            //Position.y = -up.Dot(eye);
            //Position.z = -_xyzscale.Dot(eye);

            _xyzscale = (target - eye).Normalized();   // f
            target = up.Cross(_xyzscale).Normalized(); // s
            up = _xyzscale.Cross(target);              // u

            AxisX.x = target.x;
            AxisY.x = target.y;
            AxisZ.x = target.z;
            AxisX.y = up.x;
            AxisY.y = up.y;
            AxisZ.y = up.z;
            AxisX.z = _xyzscale.x;
            AxisY.z = _xyzscale.y;
            AxisZ.z = _xyzscale.z;

            AxisW.x = -target.Dot(eye);
            AxisW.y = -up.Dot(eye);
            AxisW.z = -_xyzscale.Dot(eye);
        }

        internal static TMatrix GetXYRotation(float x, float y)
        {
            return Identity.ModifyRotation(Vector3.Zero, x, y);
        }

        internal TMatrix ModifyRotation(Vector3 eye, float x, float y)
        {
            /*
            mat4 FPSViewRH( vec3 eye, float pitch, float yaw )
            {
                // I assume the values are already converted to radians.
                float cosPitch = cos(pitch);
                float sinPitch = sin(pitch);
                float cosYaw = cos(yaw);
                float sinYaw = sin(yaw);

                vec3 xaxis = { cosYaw, 0, -sinYaw };
                vec3 yaxis = { sinYaw * sinPitch, cosPitch, cosYaw * sinPitch };
                vec3 zaxis = { sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw };

                // Create a 4x4 view matrix from the right, up, forward and eye position vectors
                mat4 viewMatrix = {
                    vec4(       xaxis.x,            yaxis.x,            zaxis.x,      0 ),
                    vec4(       xaxis.y,            yaxis.y,            zaxis.y,      0 ),
                    vec4(       xaxis.z,            yaxis.z,            zaxis.z,      0 ),
                    vec4( -dot( xaxis, eye ), -dot( yaxis, eye ), -dot( zaxis, eye ), 1 )
            };
            return viewMatrix;
            */

            _scrx = Math.SinCos(x);
            _scry = Math.SinCos(y);

            AxisX.x = _scrx.Cos;
            AxisX.y = 0f;
            AxisX.z = _scrx.Sin;

            AxisY.x = _scrx.Sin * _scry.Sin;
            AxisY.y = _scry.Cos;
            AxisY.z = _scrx.Cos * _scry.Sin;

            AxisZ.x = _scrx.Sin * _scry.Cos;
            AxisZ.y = -_scry.Sin;
            AxisZ.z = _scry.Cos * _scrx.Cos;

            Position.x = -AxisX.Dot(eye);
            Position.y = -AxisY.Dot(eye);
            Position.z = -AxisZ.Dot(eye);

            return this;
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
            ptr[13] = m13;
            ptr[14] = m23;
            ptr[15] = m33;
        }

        internal float[] ToArray()
        {
            return new float[16]
            {
                m00,
                m10,
                m20,
                m30,
                m01,
                m11,
                m21,
                m31,
                m02,
                m12,
                m22,
                m32,
                m03,
                m13,
                m23,
                m33
            };
        }

        internal void SetArray(ref float[] arr)
        {
            arr[0] = m00;
            arr[1] = m10;
            arr[2] = m20;
            arr[3] = m30;
            arr[4] = m01;
            arr[5] = m11;
            arr[6] = m21;
            arr[7] = m31;
            arr[8] = m02;
            arr[9] = m12;
            arr[10] = m22;
            arr[11] = m32;
            arr[12] = m03;
            arr[13] = m13;
            arr[14] = m23;
            arr[15] = m33;
        }

        private class TMatrixFovLessThanEqualToZeroException : Exception
        {
            public TMatrixFovLessThanEqualToZeroException() : base("Field of view angle cannot be less than or equal to zero.")
            {
            }
        }

        /// <summary>
        /// Creates a right-handed perspective projection <see cref="TMatrix"/>.
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

            AxisX = Vector3.Zero;
            AxisY = Vector3.Zero;
            AxisZ = Vector3.Zero;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init
            
            if (fov < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            ModifyPerspective(fov, aspect, near, far, scale);
        }

        /// <summary>
        /// Creates a right-handed orthographic projection <see cref="TMatrix"/>.
        /// </summary>
        /// <param name="size">The size of the projection.</param>
        /// <param name="near">The near clip-plane.</param>
        /// <param name="far">The far clip-plane.</param>
        public TMatrix(float size, float aspect, float near, float far)
        {
            // Init
            _xylen = 0f;
            _xyzscale = Vector3.Zero;
            _scrx = new SinCosResult();
            _scry = new SinCosResult();
            _scrz = new SinCosResult();

            AxisX = Vector3.Zero;
            AxisY = Vector3.Zero;
            AxisZ = Vector3.Zero;
            Position = Vector3.Zero;
            AxisW = Vector3.Zero;
            Clip = 1f;
            // End Init

            if (size < Math.Epsilon)
                throw new TMatrixFovLessThanEqualToZeroException();

            ModifyOrthographic(size, aspect, near, far);
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

            ModifyLookAt(eye, target, up);
        }

        /// <summary>
        /// Creates a right-handed rotation <see cref="TMatrix"/>
        /// </summary>
        /// <param name="eye"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public TMatrix(Vector3 eye, float x, float y)
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

            ModifyRotation(eye, x, y);
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

            Rotate(transform.Rotation);
            Translate(transform.Position);
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

        private static readonly TMatrix zero = new TMatrix(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
        private static readonly TMatrix identity = new TMatrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
        private static readonly TMatrix orthoIdentity = new TMatrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f);
        public static TMatrix Zero => zero;
        public static TMatrix Identity => identity;
        public static TMatrix OrthographicIdentity => orthoIdentity;
    }
}
