using Dull.Materials;
using OpenTK.Mathematics;

namespace Dull.Objects
{
    public enum HittableType { Sphere, TriangleMT, Model };
    public interface IHittable
    {
        HittableType GetHitType();
        Vector4[] GetStd140Data();
        int GetOffset();
        void SetOffset(int offset);
        int GetSizeInBytes();
        string GetInfo();
        string GetName();
        Vector3 GetPostion();
        int GetCount();
        float GetScale();
        void SetScale(float scaleFactor);

        void SetRotation(float xAngle, float yAngle, float zAngle);
        Vector3 GetRotation();
        void SetPostion(Vector3 position);

        IMaterial GetMaterial();
        void SetMaterial(IMaterial material);
        void SetUpdatedState();
    }
}
