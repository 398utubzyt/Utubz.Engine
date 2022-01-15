using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    /// <summary>
    /// Supplies transform data for the <see cref="Entity"/>.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Transform : IEquatable<Transform>
    {
        private static readonly Transform identity = new Transform();
        public static Transform Identity => identity;

        private TMatrix mat;
        private Vector3 vec;

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public TMatrix Matrix { get { mat.Modify(this); return mat; } set { mat = value; } }

        public void Translate(Vector3 tra) => Position += tra;
        public void Rotate(Vector3 rot) => Rotation += rot;
        public void Dilate(Vector3 dil) => Scale += dil;

        public Vector3 Forward => Vector3.ToForwardAxis(Rotation);
        public Vector3 Right => Vector3.ToRightAxis(Rotation);
        public Vector3 Up => Vector3.ToUpAxis(Rotation);

        public Transform Inverse { get { Transform t = identity; t.Position = -Position; t.Rotation = -Rotation; t.Scale = Vector3.One / Scale; return t; } }

        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            mat = TMatrix.Identity;
            vec = Vector3.Zero;
        }

        public Transform(Vector3 position)
        {
            Position = position;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            mat = TMatrix.Identity;
            vec = Vector3.Zero;
        }

        public Transform(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            Scale = Vector3.One;
            mat = TMatrix.Identity;
            vec = Vector3.Zero;
        }

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            mat = TMatrix.Identity;
            vec = Vector3.Zero;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>Whether or not the vector and the other object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Transform)
            {
                return Equals((Transform)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true"/> if this vector and <paramref name="other"/> are equal
        /// </summary>
        /// <param name="other">The other vector to compare.</param>
        /// <returns>Whether or not the vectors are equal.</returns>
        public bool Equals(Transform other)
        {
            return Position == other.Position && Rotation == other.Rotation && Scale == other.Scale;
        }

        /// <summary>
        /// Serves as the hash function for <see cref="Vector3"/>.
        /// </summary>
        /// <returns>A hash code for this vector.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Rotation.GetHashCode() ^ Scale.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector3"/> to a string.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return $"[{Position}, {Rotation}, {Scale}]";
        }

        /// <summary>
        /// Converts this <see cref="Vector3"/> to a string with the given <paramref name="format"/>.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public string ToString(string format)
        {
            return $"[{Position.ToString(format)}, {Rotation.ToString(format)}, {Scale.ToString(format)}]";
        }

        public static bool operator ==(Transform a, Transform b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Transform a, Transform b)
        {
            return !a.Equals(b);
        }
    }
}
