using Dull.ObjectTexture;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dull.Materials
{
    class Metal : IMaterial
    {
        private static int _objectSize = 1;
        private ITexture _tex;
        private float _fuzz;
        private const MaterialType _type = MaterialType.Metal;
        private Vector4[] _data;

        public Metal(ITexture tex,float fuzz)
        {
            _tex = tex;
            _fuzz = fuzz;
            _data = new Vector4[_objectSize + tex.GetSizeInVec4()];
            SetStd140Data();
        }
        public Metal(ITexture tex)
        {
            _tex = tex;
            _fuzz = 0;
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
            _data[0].Y = _fuzz;
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
            return $"Material:Metal Fuzz: {_data[0].Y}\n  {_tex.GetInfo()}";
        }
        public float? GetParam()
        {
            return _fuzz;
        }
        public void SetParam(float? param)
        {
            _fuzz = param.Value;
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
