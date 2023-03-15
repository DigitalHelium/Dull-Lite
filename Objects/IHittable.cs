using OpenTK.Mathematics;

namespace Dull.Objects
{
    public enum HittableType { Sphere, TriangleMT };
    interface IHittable
    {
        HittableType GetHitType();
        Vector4[] GetStd140Data();
        int GetOffset();
        void SetOffset(int offset);
        int GetSizeInBytes();
        string GetInfo();
    }
}
