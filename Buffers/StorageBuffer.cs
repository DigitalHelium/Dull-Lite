using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Runtime.InteropServices;

namespace Dull
{
    class StorageBuffer
    {
        private int _handle = 0;
  
        public StorageBuffer(int size)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, size, (IntPtr)null, BufferUsageHint.StreamCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public StorageBuffer()
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, (IntPtr)null, BufferUsageHint.StreamCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void AttachData(byte[] data)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, data.Length, data, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void AttachSubData(byte[] data,int offset)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferSubData(BufferTarget.ShaderStorageBuffer, (IntPtr)offset, data.Length, data);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void AttachData(Vector4[] data)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, data.Length * Vector4.SizeInBytes, data, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void AttachSubData(Vector4[] data, int offset)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferSubData(BufferTarget.ShaderStorageBuffer, (IntPtr)offset, data.Length* Vector4.SizeInBytes, data);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void BindBuffer(int index)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, index, _handle);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void BindBufferRange(int index, int offset,int size)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BindBufferRange(BufferRangeTarget.ShaderStorageBuffer, index, _handle, (IntPtr)offset, size);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }
        public void ResizeBuffer(int size)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, size, (IntPtr)null, BufferUsageHint.StreamCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public int Handle { get => _handle; }
    }
}
