using OpenTK.Mathematics;

namespace Dull.ObjectTexture
{
    public enum TextureType { Solid = 10, Checker = 11, Wallpaper = 12 };
    public interface ITexture
    {
        int GetSizeInBytes();
        int GetSizeInVec4();
        TextureType GetTextureType();
        Vector4[] GetSTD140Data();
        string GetInfo();
        Vector3[] GetAlbedo();
        void SetAlbedo(params Vector3[] albedo);
    }
}
