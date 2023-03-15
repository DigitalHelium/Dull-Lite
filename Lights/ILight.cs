using Dull.Objects;
using OpenTK.Mathematics;

namespace Dull.Lights
{
    public enum LightType { PointLight };
    internal interface ILight
    {
        LightType GetLightType();
        Vector4[] GetStd140Data();
        int GetOffset();
        void SetOffset(int offset);
        int GetSizeInBytes();
        string GetInfo();
    }
}
