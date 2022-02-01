using Utubz.Internal.Native.Glad;
using System;
using System.Runtime.InteropServices;

namespace Utubz.Graphics
{
    public static unsafe class GL
    {
        #region Buffers

        public abstract class Buffer : Object
        {
            protected abstract uint GlType { get; }
            internal uint id;
            internal IntPtr data;
            internal int len;
            internal int size;

            public void Bind()
            {
                glad.GLBindBuffer(GlType, id);
            }

            public void Set<T>(T[] arr) where T : unmanaged
            {
                glad.GLBindBuffer(GlType, id);

                fixed (T* ptr = arr) { data = (IntPtr)ptr; }
                len = arr.Length;
                size = sizeof(T);
                glad.GLBufferData(GlType, len * size, data, 0);
            }

            public void Set<T>(T* ptr, int length) where T : unmanaged
            {
                glad.GLBindBuffer(GlType, id);

                data = (IntPtr)ptr;
                len = length;
                size = sizeof(T);
                glad.GLBufferData(GlType, len * size, data, 0);
            }

            protected override void Clean()
            {
                uint _id = id;
                glad.GLDeleteBuffers(1, &_id);
            }

            public Buffer()
            {
                uint _id = 0;
                glad.GLGenBuffers(1, &_id);
                id = _id;
            }
        }

        public sealed class VertexArray : Object
        {
            internal uint id;
            internal IntPtr data;

            public void Bind()
            {
                glad.GLBindVertexArray(id);
            }

            protected override void Clean()
            {
                uint _id = id;
                glad.GLDeleteVertexArrays(1, &_id);
            }

            public VertexArray()
            {
                uint _id = 0;
                glad.GLGenVertexArrays(1, &_id);
                id = _id;
            }
        }

        public sealed class ArrayBuffer : Buffer
        {
            protected override uint GlType { get; } = glad.GL_ARRAY_BUFFER;
        }
        public sealed class ElementArrayBuffer : Buffer
        {
            protected override uint GlType { get; } = glad.GL_ELEMENT_ARRAY_BUFFER;
        }

        #endregion

        #region Attributes

        public sealed class VertexAttribute : Object
        {
            public string Name { get; }
            public uint Location { get; }


            public void Set<T>(Buffer buffer, int step, int start) where T : unmanaged
            {
                Type t;
                switch (typeof(T).Name)
                {
                    case "SByte":
                        t = Type.Sbyte;
                        break;
                    case "Byte":
                        t = Type.Byte;
                        break;
                    case "Int16":
                        t = Type.Short;
                        break;
                    case "UInt16":
                        t = Type.Ushort;
                        break;
                    case "Int32":
                        t = Type.Int;
                        break;
                    case "UInt32":
                        t = Type.Uint;
                        break;
                    case "Single":
                        t = Type.Float;
                        break;
                    default:
                        return;
                }

                buffer.Bind();
                glad.GLVertexAttribPointer(Location, buffer.len, (uint)t, 0, step * buffer.size, (IntPtr)(start * buffer.size));
            }

            public void Enable()
            {
                glad.GLEnableVertexAttribArray(Location);
            }

            public void Disable()
            {
                glad.GLDisableVertexAttribArray(Location);
            }

            public static VertexAttribute Find(Shader shader, string name)
            {
                uint loc = glad.GLGetAttribLocation(shader.ShaderId, name);
                if (loc >= 0)
                    return new VertexAttribute(name, loc);
                return null;
            }

            private VertexAttribute(string name, uint id)
            {
                Name = name;
                Location = id;
            }
        }

        #endregion

        #region Enums

        public enum Type
        {
            Sbyte   = 0x1400,
            Byte  = 0x1401,
            Short  = 0x1402,
            Ushort = 0x1403,
            Int    = 0x1404,
            Uint   = 0x1405,
            Float  = 0x1406,
        }

        #endregion

        public static void Bind(Buffer buf)
        {
            buf.Bind();
        }
    }
}
