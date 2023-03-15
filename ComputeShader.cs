using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Dull
{
    class ComputeShader:IDisposable
    {
        private int _handle;
        private bool disposedValue = false;
        public ComputeShader(string shaderPath)
        {
            string ComputeShaderSource;
            using (StreamReader reader = new StreamReader(shaderPath, Encoding.UTF8))
            {
                ComputeShaderSource = reader.ReadToEnd();
            }
                        
            int ComputeShader = GL.CreateShader(ShaderType.ComputeShader);
            GL.ShaderSource(ComputeShader, ComputeShaderSource);
            
            GL.CompileShader(ComputeShader);
            GL.GetShader(ComputeShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(ComputeShader));

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, ComputeShader);

            GL.LinkProgram(_handle);

            GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success2);
            if (success2 == 0) Console.WriteLine(GL.GetProgramInfoLog(_handle));

            GL.DetachShader(_handle, ComputeShader);
            GL.DeleteShader(ComputeShader);
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
        ~ComputeShader()
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
