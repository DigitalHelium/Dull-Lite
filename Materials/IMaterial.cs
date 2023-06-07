using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Materials
{
    public enum MaterialType { Lambertian = 0, Metal = 1, Dielectric = 2, Transparent = 3, DiffuseLight = 4 };
    public interface IMaterial
    {
        int GetSizeInBytes();
        int GetSizeInVec4();
        MaterialType GetMaterialType();
        Vector4[] GetSTD140Data();
        string GetInfo();

        float? GetParam();
        void SetParam(float? param);

        ITexture GetTexture();
        void SetTexture(ITexture tex);
    }
}
