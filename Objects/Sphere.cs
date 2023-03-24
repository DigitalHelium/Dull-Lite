﻿using Dull.Materials;
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
            Console.WriteLine(GetInfo());
            return _data;
        }
        private void SetStd140Data()
        {
            _data[0].Xyz = _center;
            _data[0].W = _radius;
            Vector4[] matData = _mat.GetSTD140Data();
            for (int i = _objectSize; i < _data.Length; i++)
                _data[i] = matData[i - _objectSize];
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
    }
}