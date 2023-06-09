﻿using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Materials
{
    class Transparent:IMaterial
    {
        private static int _objectSize = 1;
        private ITexture _tex;
        private float _transparency;
        private const MaterialType _type = MaterialType.Transparent;
        private Vector4[] _data;

        public Transparent(ITexture tex, float transparency)
        {
            _tex = tex;
            _transparency = transparency;
            _data = new Vector4[_objectSize + tex.GetSizeInVec4()];
            SetStd140Data();
        }
        public Transparent(ITexture tex)
        {
            _tex = tex;
            _transparency = 1.1f;
            _data = new Vector4[_objectSize + _tex.GetSizeInVec4()];
            SetStd140Data();
        }
        public MaterialType GetMaterialType()
        {
            return _type;
        }

        public int GetSizeInBytes()
        {
            return _data.Length * Vector4.SizeInBytes;
        }
        public int GetSizeInVec4()
        {
            return _data.Length;
        }
        private void SetStd140Data()
        {
            _data[0].X = (float)_type;
            _data[0].W = _transparency;
            Vector4[] texData = _tex.GetSTD140Data();
            for (int i = _objectSize; i < _data.Length; i++)
                _data[i] = texData[i - _objectSize];
        }

        public Vector4[] GetSTD140Data()
        {
            SetStd140Data();
            return _data;
        }
        public string GetInfo()
        {
            return $"Material:Transparent Transparency: {_data[0].W}\n  {_tex.GetInfo()}";
        }
        public float? GetParam()
        {
            return _transparency;
        }
        public void SetParam(float? param)
        {
            _transparency = param.Value;
        }
        public ITexture GetTexture()
        {
            return _tex;
        }
        public void SetTexture(ITexture tex)
        {
            _tex = tex;
        }
    }
}
