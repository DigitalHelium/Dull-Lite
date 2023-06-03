using Dull.Materials;
using OpenTK.Mathematics;
using System;

namespace Dull.Objects
{
    class TriangleMT : IHittable
    {
        private Vector3 _v0, _v1, _v2;
        private Vector3 _vn0, _vn1, _vn2;
        private Vector3 _n;
        private Vector3 _v2v0, _v1v0;
        private bool _isOneSided;
        private Vector3 _postion;

        public static int OBJECT_SIZE = 6;//in Vector4
        private HittableType _type = HittableType.TriangleMT;
        Vector4[] _data;
        private int _byteOffset = -1;
        private IMaterial _mat;
        private bool _isUpdated = true;
        public TriangleMT(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 vn0, Vector3 vn1, Vector3 vn2, bool isOneSided, IMaterial mat)
        {
            _v0 = v0;
            _v1 = v1;
            _v2 = v2;
            _vn0 = vn0;
            _vn1 = vn1;
            _vn2 = vn2;
            _v2v0 = v2 - v0;
            _v1v0 = v1 - v0;
            _postion = new Vector3((v0.X + v1.X + v2.X) / 3, (v0.Y + v1.Y + v2.Y) / 3, (v0.Z + v1.Z + v2.Z) / 3);
            _n = Vector3.Cross(_v2v0, _v1v0);
            _isOneSided = isOneSided;
            _mat = mat;

            _data = new Vector4[OBJECT_SIZE + mat.GetSizeInVec4()];
            SetStd140Data();

        }
        public TriangleMT(Vector3 v0, Vector3 v1, Vector3 v2, bool isOneSided, IMaterial mat)
        {
            _v0 = v0;
            _v1 = v1;
            _v2 = v2;
            _vn0 = new Vector3(0);
            _vn1 = new Vector3(0);
            _vn2 = new Vector3(0);
            _v2v0 = v2 - v0;
            _v1v0 = v1 - v0;
            _postion = new Vector3((v0.X + v1.X + v2.X) / 3, (v0.Y + v1.Y + v2.Y) / 3, (v0.Z + v1.Z + v2.Z) / 3);
            _n = Vector3.Cross(_v2v0, _v1v0);
            _isOneSided = isOneSided;
            _mat = mat;

            _data = new Vector4[OBJECT_SIZE + mat.GetSizeInVec4()];
            SetStd140Data();

        }
        public HittableType GetHitType()
        {
            return _type;
        }

        public Vector4[] GetStd140Data()
        {
            SetStd140Data();
            return _data;
        }
        private void SetStd140Data()
        {
            if (_isUpdated)
            {
                _data[0].Xyz = _v0;
                _data[0].W = _isOneSided ? 1.0f : 0.0f;
                _data[1].Xyz = _v2v0;
                _data[2].Xyz = _v1v0;
                _data[3].Xyz = _vn0;
                _data[4].Xyz = _vn1;
                _data[5].Xyz = _vn2;
                _isUpdated = false;
            }
            Vector4[] matData = _mat.GetSTD140Data();
            for (int i = OBJECT_SIZE; i < _data.Length; i++)
                _data[i] = matData[i - OBJECT_SIZE];
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
        public string GetName()
        {
            return "Triangle";
        }
        public Vector3 GetPostion()
        {
            return _postion;
        }

        public void SetPostion(Vector3 position)
        {
            Vector3 posOffset = _postion - position;
            _v0 = _v0 + posOffset;
            _v1 = _v1 + posOffset;
            _v2 = _v2 + posOffset;
            _v2v0 = _v2 - _v0;
            _v1v0 = _v1 - _v0;
            _postion = position;
            _isUpdated = true;
        }
        public IMaterial GetMaterial()
        {
            return _mat;
        }
        public void SetMaterial(IMaterial material)
        {
            _mat = material;
        }
        public int GetCount()
        {
            return 1;
        }
        public void SetUpdatedState()
        {
            _isUpdated = true;
        }
    }
}
