using Dull.Materials;
using Dull.ObjectTexture;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dull.Objects
{
    internal class Model : IHittable
    {
        Vector3 _origin = new Vector3 (0, 0, 0);
        List<TriangleMT> _triangleMTs = new List<TriangleMT>();
        private int objectSize = 0;//in Vector4
        private HittableType _type = HittableType.Model;
        private float _scale = 1;

        Vector4[] _data;
        private int _byteOffset = -1;
        private IMaterial _mat;
        private bool _isUpdated = true;
        public Model(IMaterial mat)
        { 
            _mat = mat;
            _data = new Vector4[512];
        }

        public HittableType GetHitType()
        {
            return _type;
        }

        public string GetInfo()
        {
            return $"Object:Model triangles: {_triangleMTs.Count}, Origin: {_origin}";
        }

        public IMaterial GetMaterial()
        {
            return _mat;
        }

        public string GetName()
        {
            return "Model";
        }

        public int GetOffset()
        {
            return _byteOffset;
        }

        public Vector3 GetPostion()
        {
            return _origin;
        }

        public int GetSizeInBytes()
        {
            return objectSize * (TriangleMT.OBJECT_SIZE + Lambertian.OBJECT_SIZE + SolidColor.OBJECT_SIZE) * Vector4.SizeInBytes;
        }

        public Vector4[] GetStd140Data()
        {
            SetStd140Data();
            return _data;
        }

        public void SetMaterial(IMaterial material)
        {
            _mat = material;
            foreach(TriangleMT tr in _triangleMTs)
            {
                tr.SetMaterial(material);
            }
            SetUpdatedState();
        }

        public void SetOffset(int offset)
        {
            _byteOffset = offset;
        }
        public void addTriangle(TriangleMT triangle)
        {
            triangle.SetMaterial(_mat);
            _triangleMTs.Add(triangle);
            Vector3 newPos = new Vector3(0);
            foreach (TriangleMT tr in _triangleMTs)
            {
                newPos += tr.GetPostion();
            }
            _origin = newPos / _triangleMTs.Count;
            foreach (TriangleMT tr in _triangleMTs)
            {
               tr.SetCenter(_origin);
            }
            objectSize++;
        }

        public void SetPostion(Vector3 position)
        {
            foreach (TriangleMT tr in _triangleMTs)
            {
                tr.SetPostion(tr.GetPostion()+ position - _origin);
            }
            
            _origin = position;
            SetUpdatedState();
        }
        public void SetScale(float scaleFactor) 
        {
            foreach (TriangleMT tr in _triangleMTs)
            {
                tr.SetScale(scaleFactor);
            }
            _scale = scaleFactor;
            SetUpdatedState();
        }
        public float GetScale()
        {
            return _scale;
        }
        private void SetStd140Data()
        {
            if (_isUpdated && objectSize != 0)
            {
                _data = new Vector4[objectSize * (TriangleMT.OBJECT_SIZE + Lambertian.OBJECT_SIZE + SolidColor.OBJECT_SIZE)];
                int k = 0;
                foreach (TriangleMT tr in _triangleMTs)
                {
                    Vector4[] tempData = tr.GetStd140Data();
                    for (int i = 0; i < TriangleMT.OBJECT_SIZE+tr.GetMaterial().GetSizeInVec4(); i++)
                    {
                        _data[i+k] = tempData[i];
                    }
                    k += TriangleMT.OBJECT_SIZE + tr.GetMaterial().GetSizeInVec4();
                }
                _isUpdated = false;
            }
        }
        public int GetCount()
        {
            return _triangleMTs.Count;
        }
        public void SetUpdatedState()
        {
            _isUpdated = true;
        }
    }
}
