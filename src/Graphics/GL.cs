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
            internal uint type;

            public void Bind()
            {
                glad.GLBindBuffer(GlType, id);
            }

            public void Set<T>(T[] arr) where T : unmanaged
            {
                Bind();

                fixed (T* ptr = arr) { data = (IntPtr)ptr; }
                len = arr.Length;
                size = sizeof(T);
                glad.GLBufferData(GlType, len * size, data, glad.GL_DYNAMIC_DRAW);

                type = (uint)GetGraphicsType<T>();
            }

            public void Set<T>(T* ptr, int length) where T : unmanaged
            {
                Bind();

                data = (IntPtr)ptr;
                len = length;
                size = sizeof(T);
                glad.GLBufferData(GlType, len * size, data, glad.GL_DYNAMIC_DRAW);

                type = (uint)GetGraphicsType<T>();
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
            #region Set
            public void Set(float x)
            {
                glad.GLVertexAttrib1f(Location, x);
            }
            public void Set(Vector2 x)
            {
                glad.GLVertexAttrib2f(Location, x.x, x.y);
            }
            public void Set(Vector3 x)
            {
                glad.GLVertexAttrib3f(Location, x.x, x.y, x.z);
            }
            public void Set(Color x)
            {
                glad.GLVertexAttrib4f(Location, x.r, x.g, x.b, x.a);
            }

            public void Set(Buffer buffer, int size, int step, int start)
            {
                buffer.Bind();
                Disable();
                glad.GLVertexAttribPointer(Location, size, buffer.type, 0, step * buffer.size, (IntPtr)(start * buffer.size));
                Enable();
            }
            #endregion

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

        public sealed class ShaderUniform : Object
        {
            public string Name { get; }
            public uint Location { get; }
            private void* data;

            #region Set
            public void Set(float x)
            {
                ConvertToPtr(x, (float*)data);
                glad.GLUniform1fv(Location, 1, (float*)data);
            }
            public void Set(Vector2 x)
            {
                ConvertToPtr(x, (float*)data);
                glad.GLUniform2fv(Location, 1, (float*)data);
            }
            public void Set(Vector3 x)
            {
                ConvertToPtr(x, (float*)data);
                glad.GLUniform1fv(Location, 1, (float*)data);
            }
            public void Set(Color x)
            {
                ConvertToPtr(x, (float*)data);
                glad.GLUniform1fv(Location, 1, (float*)data);
            }
            public void Set(TMatrix x)
            {
                ConvertToPtr(x, (float*)data);
                glad.GLUniformMatrix4fv(Location, 1, 0, (float*)data);
            }
            #endregion

            public static ShaderUniform Find(Shader shader, string name)
            {
                uint loc = glad.GLGetUniformLocation(shader.ShaderId, name);
                if (loc >= 0)
                    return new ShaderUniform(name, loc);
                return null;
            }

            protected override void Clean()
            {
                Marshal.FreeHGlobal((IntPtr)data);
            }

            private ShaderUniform(string name, uint id)
            {
                Name = name;
                Location = id;
                data = (void*)Marshal.AllocHGlobal(16 * sizeof(float));
            }
        }

        #endregion

        #region Enums

        public enum Type
        {
            Unknown = 0x0000,
            Sbyte   = 0x1400,
            Byte    = 0x1401,
            Short   = 0x1402,
            Ushort  = 0x1403,
            Int     = 0x1404,
            Uint    = 0x1405,
            Float   = 0x1406,
        }

        #endregion

        #region Util

        public static void Bind(Buffer buf)
        {
            buf.Bind();
        }

        public static Type GetGraphicsType<T>() where T : unmanaged
        {
            switch (typeof(T).Name)
            {
                case "SByte":
                    return Type.Sbyte;
                case "Byte":
                    return Type.Byte;
                case "Int16":
                    return Type.Short;
                case "UInt16":
                    return Type.Ushort;
                case "Int32":
                    return Type.Int;
                case "UInt32":
                    return Type.Uint;
                case "Single":
                    return Type.Float;
                default:
                    return Type.Unknown;
            }
        }

        public static void TargetTexture(uint index)
        {
            glad.GLActiveTexture(0x84C0 + index);
        }

        public static void Bind(Texture texture)
        {
            glad.GLBindTexture(0x0DE1, texture.TextureId);
        }

        public static void UseTexture(Texture texture, uint index)
        {
            TargetTexture(index);
            Bind(texture);
        }

        public static void DrawTriangles(ArrayBuffer vbo, ElementArrayBuffer ebo, VertexArray vao)
        {
            vbo.Bind();
            ebo.Bind();
            vao.Bind();
            glad.GLDrawElements(glad.GL_TRIANGLES, ebo.len, ebo.type, (IntPtr)0);
        }

        #endregion

        #region Pointer Conversion

        public static void ConvertToPtr(byte x, byte* ptr)
        {
            ptr[0] = x;
        }

        public static void ConvertToPtr(int x, int* ptr)
        {
            ptr[0] = x;
        }

        public static void ConvertToPtr(float x, float* ptr)
        {
            ptr[0] = x;
        }

        public static void ConvertToPtr(Vector2 x, float* ptr)
        {
            ptr[0] = x.x;
            ptr[1] = x.y;
        }

        public static void ConvertToPtr(Vector3 x, float* ptr)
        {
            ptr[0] = x.x;
            ptr[1] = x.y;
            ptr[2] = x.z;
        }

        public static void ConvertToPtr(Color x, float* ptr)
        {
            ptr[0] = x.r;
            ptr[1] = x.g;
            ptr[2] = x.b;
            ptr[3] = x.a;
        }

        public static void ConvertToPtr(TMatrix x, float* ptr)
        {
            x.ToArrayPtr(ptr);
        }

        #endregion
    }
}
