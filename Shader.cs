using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace Dull
{
    class Shader:IDisposable
    {
        private int _handle;
        private bool disposedValue = false;
        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;
            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(VertexShader));

            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success1);
            if (success1 == 0) Console.WriteLine(GL.GetShaderInfoLog(FragmentShader));

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, VertexShader);
            GL.AttachShader(_handle, FragmentShader);

            GL.LinkProgram(_handle);

            GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success2);
            if (success2 == 0) Console.WriteLine(GL.GetProgramInfoLog(_handle));

            GL.DetachShader(_handle, VertexShader);
            GL.DetachShader(_handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }

        public int Handle { get => _handle; }
        public void Use()
        {
            GL.UseProgram(_handle);
        }
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(_handle);
                disposedValue = true;
            }
        }
        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
