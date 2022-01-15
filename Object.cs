using System;
using System.Runtime.InteropServices;

namespace Utubz
{
    public class Object : IEquatable<Object>
    {
        private static int nextObjectId;
        private GCHandle handle;
        private bool destroyed;
        private int id;

        /// <summary>
        /// The instance id of the <see cref="Object"/>.
        /// </summary>
        public int Id => id;

        public override bool Equals(object obj)
        {
            if (obj is Object)
                return Equals((Object)obj);
            return false;
        }

        public bool Equals(Object obj)
        {
            return obj.id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override string ToString()
        {
            return id.ToString();
        }

        public void Destroy()
        {
            if (destroyed)
                return;

            Clean();

            destroyed = true;
            handle.Free();
            id = -1;
        }

        protected virtual void Clean() { }

        public Object()
        {
            id = nextObjectId++;
            handle = GCHandle.Alloc(id, GCHandleType.Normal);
        }

        public static implicit operator bool(Object obj)
        {
            return obj != (DBNull)null && obj.id != -1;
        }

        public static bool operator ==(Object a, Object b)
        {
            return a.id == b.id;
        }

        public static bool operator !=(Object a, Object b)
        {
            return a.id != b.id;
        }

        public static bool operator ==(Object a, DBNull n)
        {
            return ReferenceEquals(a, n);
        }

        public static bool operator !=(Object a, DBNull n)
        {
            return !ReferenceEquals(a, n);
        }

        public static bool Null(Object obj)
        {
            return obj == (DBNull)null;
        }

        public static bool NotNull(Object obj)
        {
            return obj != (DBNull)null;
        }
    }
}
