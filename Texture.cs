using System;
using OpenTK.Graphics.OpenGL;

namespace Dull
{
    class Texture
    {
        private int _handle;
        public Texture(int w, int h, PixelInternalFormat internalFormat, PixelFormat format, IntPtr data, int unit)
        {
            _handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _handle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, w, h, 0, format, PixelType.Float, data);
            
            GL.BindImageTexture(unit, _handle, 0, false, 0, TextureAccess.ReadWrite, SizedInternalFormat.Rgba32f);
        }
        public void UpdateTextureParams(int w, int h, IntPtr data)
        {
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, w, h, 0, PixelFormat.Rgba, PixelType.Float, data);
        }
        public int Handle { get => _handle; }
    }
}
