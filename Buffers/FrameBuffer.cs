using OpenTK.Graphics.OpenGL;

namespace Dull
{
    class FrameBuffer
    {
        int _handle;
        public FrameBuffer(int textureHandle)
        {
            _handle = GL.GenFramebuffer();
            AttachTexture(textureHandle);
        }
        public FrameBuffer(int[] textureHandles)
        {
            _handle = GL.GenFramebuffer();
            AttachTexture(textureHandles);
        }
        public FrameBuffer()
        {
            _handle = GL.GenFramebuffer();
        }
        public void AttachTexture(int textureHandle)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle, 0);
        }
        public void DetachTexture()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        }
        ~FrameBuffer()
        {
            GL.DeleteFramebuffer(_handle);
        }
        public byte[] ReadBufferToByteArray(int width, int height, PixelFormat pixelFormat, PixelType pixelType)
        {
            int data_size = width * height * 4;
            byte[] pixels = new byte[data_size];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            return pixels;
        }
        public void AttachTexture(int[] textureHandles)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);

            for(int i = 0; i < textureHandles.Length; i++)
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0+i, TextureTarget.Texture2D, textureHandles[i], 0);
         
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public int Handle { get => _handle; }
    }
}
