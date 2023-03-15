using Dull.ObjectTexture;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dull.Materials
{
    class Dielectric:IMaterial
    {
        private static int _objectSize = 1;
        private ITexture _tex;
        private float _ir;
        private const MaterialType _type = MaterialType.Dielectric;
        private Vector4[] _data;

        public Dielectric(ITexture tex, float ir)
        {
            _tex = tex;
            _ir = ir;
            _data = new Vector4[_objectSize + tex.GetSizeInVec4()];
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
            _data[0].Z = _ir;
            Vector4[] texData = _tex.GetSTD140Data();
            for (int i = _objectSize; i < _data.Length; i++)
                _data[i] = texData[i - _objectSize];
        }

        public Vector4[] GetSTD140Data()
        {

            return _data;
        }
        public string GetInfo()
        {
            return $"Material:Dielectric IR: {_data[0].Z}\n  {_tex.GetInfo()}";
        }
    }
}
