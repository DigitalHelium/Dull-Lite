using Dull.Materials;
using OpenTK.Mathematics;
using System;

namespace Dull.Objects
{
    class TriangleMT : IHittable
    {
        private const float EPSILON = 0.000001f;
        private Vector3 _v0, _v1, _v2;
        private Vector3 _n;
        private Vector3 _v2v0, _v1v0;
        private bool _isOneSided;

        private static int _objectSize = 3;//in Vector4
        private HittableType _type = HittableType.TriangleMT;
        Vector4[] _data;
        private int _byteOffset = -1;
        private IMaterial _mat;

        public TriangleMT(Vector3 v0, Vector3 v1, Vector3 v2, bool isOneSided, IMaterial mat)
        {
            _v0 = v0;
            _v1 = v1;
            _v2 = v2;
            _v2v0 = v2 - v0;
            _v1v0 = v1 - v0;
            _n = Vector3.Cross(_v2v0, _v1v0);
            _isOneSided = isOneSided;
            _mat = mat;

            _data = new Vector4[_objectSize + mat.GetSizeInVec4()];
            SetStd140Data();

        }
        public HittableType GetHitType()
        {
            return _type;
        }

        public Vector4[] GetStd140Data()
        {
            SetStd140Data();
            Console.WriteLine(GetInfo());
            return _data;
        }
        private void SetStd140Data()
        {
            _data[0].Xyz = _v0;
            _data[0].W = _isOneSided ? 1.0f : 0.0f;
            _data[1].Xyz = _v2v0;
            _data[2].Xyz = _v1v0;
            Vector4[] matData = _mat.GetSTD140Data();
            for (int i = _objectSize; i < _data.Length; i++)
                _data[i] = matData[i - _objectSize];
        }

        public int GetSizeInBytes()
        {
            return _data.Length * Vector4.SizeInBytes;
        }
        public int GetOffset()
        {
            return _byteOffset;
        }

        public void SetOffset(int offset)
        {
            _byteOffset = offset;
        }
        public string GetInfo()
        {
            return $"Object:TriangleMT Position: v0: {_data[0]}, v2v0: {_data[1]}, v1v0: {_data[2]}\n {_mat.GetInfo()}";
        }
    }
}
