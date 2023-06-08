using Dull.Materials;
using OpenTK.Mathematics;
using System;

namespace Dull.Objects
{
    class Sphere : IHittable
    {
        private Vector3 _center;
        private float _radius;

        private static int _objectSize = 1;//in Vector4
        private HittableType _type = HittableType.Sphere;
        Vector4[] _data;
        private int _byteOffset = -1;
        private IMaterial _mat;
        private bool _isUpdated = true;
        private float _scale = 1;
        private Vector3 _rotation = new Vector3();
        public Sphere(Vector3 center, float radius, IMaterial mat)
        {
            _center = center;
            _radius = radius;
            _mat = mat;
            _data = new Vector4[_objectSize+mat.GetSizeInVec4()];
            SetStd140Data();
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
                _data[0].Xyz = _center;
                _data[0].W = _radius;
                _isUpdated = false;
                Vector4[] matData = _mat.GetSTD140Data();
                for (int i = _objectSize; i < _data.Length; i++)
                    _data[i] = matData[i - _objectSize];
            }
            
        }

        public HittableType GetHitType()
        {
            return _type;
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
            return $"Object:Sphere Position:{_data[0]}\n {_mat.GetInfo()}";
        }
        public string GetName()
        {
            return "Sphere";
        }

        public Vector3 GetPostion()
        {
            return _center;
        }

        public void SetPostion(Vector3 position)
        {
            _center = position;
            SetUpdatedState();
        }

        public IMaterial GetMaterial()
        {
            return _mat;
        }

        public void SetMaterial(IMaterial material)
        {
            _mat = material;
            SetUpdatedState();
        }

        public int GetCount()
        {
            return 1;
        }
        public void SetUpdatedState()
        {
            _isUpdated = true;
        }

        public float GetScale()
        {
            return _scale;
        }

        public void SetScale(float scaleFactor)
        {
            _radius = _radius/_scale*scaleFactor;
            _scale = scaleFactor;
            SetUpdatedState();
        }

        public void SetRotation(float xAngle, float yAngle, float zAngle)
        {
            _rotation = new Vector3
            {
                X = xAngle,
                Y = yAngle,
                Z = zAngle
            };
        }

        public Vector3 GetRotation()
        {
            return _rotation;
        }
    }
}
