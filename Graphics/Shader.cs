using Utubz.Internal.Native.Glad;
using Utubz.Internal.Native;
using System.Runtime.InteropServices;

using System;

namespace Utubz.Graphics
{
    public sealed class Shader : Object
    {
        internal const string DefaultVertexString = @"#version 460
in vec3 vPosition;
in vec4 vColor;
in vec2 vTexCoord;

out vec4 uColor;
out vec2 uTexCoord;

uniform mat4 tModel;
uniform mat4 tView;
uniform mat4 tProjection;

void main()
{
    uColor = vColor;
    uTexCoord = vTexCoord;
    gl_Position = tProjection * tView * tModel * vec4(vPosition, 1.0);
}";
        internal const string DefaultFragmentString = @"#version 460
in vec4 uColor;
in vec2 uTexCoord;

uniform sampler2D uTexture;

out vec4 fColor;

void main()
{
    fColor = texture(uTexture, uTexCoord);
}";

        internal const string DebugVertexString = @"#version 460
in vec3 vPosition;
in vec4 vColor;
in vec2 vTexCoord;

out vec4 uColor;
out vec2 uTexCoord;

uniform mat4 tModel;
uniform mat4 tView;
uniform mat4 tProjection;

void main()
{
    uColor = vColor;
    uTexCoord = vTexCoord;
    gl_Position = tProjection * tView * tModel * vec4(vPosition, 1.0);
}";
        internal const string DebugFragmentString = @"#version 460
in vec4 uColor;
in vec2 uTexCoord;

uniform sampler2D uTexture;

out vec4 fColor;

void main()
{
    fColor = texture(uTexture, uTexCoord);
}";

        private uint vsh;
        private uint fsh;
        private uint prg;

        public uint VertId => vsh;
        public uint FragId => fsh;
        public uint ShaderId => prg;

        internal static Shader defSh;
        internal static Shader dbgSh;
        public static Shader Default { get { if (Null(defSh)) defSh = Shader.ParseGlsl(DefaultVertexString, DefaultFragmentString); return defSh; } }
        public static Shader Debug { get { if (Null(dbgSh)) dbgSh = Shader.ParseGlsl(DebugVertexString, DebugFragmentString); return dbgSh; } }

        internal class ShaderCompilationException : Exception
        {
            public ShaderCompilationException(string type, string log) : base($"The {type} shader was not able to compile: {log}")
            {

            }
        }

        public static unsafe Shader ParseGlsl(string vertex, string fragment)
        {
            Shader s = new Shader();

            byte* p = (byte*)UTF8Marshaller.marshaler.MarshalManagedToNative(vertex);
            glad.GLShaderSource(s.vsh, 1, &p, null);

            int status;
            glad.GLCompileShader(s.vsh);
            Marshal.FreeHGlobal((IntPtr)p);
            glad.GLGetShaderiv(s.vsh, glad.GL_COMPILE_STATUS, &status);

            if (status == 0)
            {
                byte* buf = stackalloc byte[512];
                glad.GLGetShaderiv(s.fsh, glad.GL_INFO_LOG_LENGTH, &status);
                glad.GLGetShaderInfoLog(s.vsh, 512, &status, buf);
                throw new ShaderCompilationException("vertex", (string)UTF8Marshaller.marshaler.MarshalNativeToManaged((IntPtr)buf));
            }
            
            p = (byte*)UTF8Marshaller.marshaler.MarshalManagedToNative(fragment);
            glad.GLShaderSource(s.fsh, 1, &p, null);

            glad.GLCompileShader(s.fsh);
            Marshal.FreeHGlobal((IntPtr)p);
            glad.GLGetShaderiv(s.fsh, glad.GL_COMPILE_STATUS, &status);

            if (status == 0)
            {
                byte* buf = stackalloc byte[512];
                glad.GLGetShaderiv(s.fsh, glad.GL_INFO_LOG_LENGTH, &status);
                glad.GLGetShaderInfoLog(s.fsh, 512, &status, buf);
                throw new ShaderCompilationException("fragment", (string)UTF8Marshaller.marshaler.MarshalNativeToManaged((IntPtr)buf));
            }

            s.Link();
            return s;
        }

        internal void Link()
        {
            glad.GLAttachShader(prg, vsh);
            glad.GLAttachShader(prg, fsh);

            glad.GLLinkProgram(prg);
        }

        protected override void Clean()
        {
            glad.GLDetachShader(prg, vsh);
            glad.GLDetachShader(prg, fsh);
            glad.GLDeleteShader(vsh);
            glad.GLDeleteShader(fsh);
        }

        internal Shader()
        {
            prg = glad.GLCreateProgram();
            vsh = glad.GLCreateShader(glad.GL_VERTEX_SHADER);
            fsh = glad.GLCreateShader(glad.GL_FRAGMENT_SHADER);
        }
    }
}
