using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dull.ObjectTexture
{
    class SolidColor : ITexture
    {
        private static int _objectSize = 2;
        private Vector3 _albedo;
        private const TextureType _type = TextureType.Solid;
        private Vector4[] _data;
        public SolidColor(Vector3 albedo)
        {
            _albedo = albedo;
            _data = new Vector4[_objectSize];
            SetStd140Data();
        }
        private void SetStd140Data()
        {
            _data[0].Xyz = _albedo;
            _data[0].W = (float)_type;
            _data[1] = new Vector4(0);
        }
        public string GetInfo()
        {
            return $"Texture:Solid Color: {_data[0].Xyz}";
        }

        public int GetSizeInBytes()
        {
            return _data.Length * Vector4.SizeInBytes;
        }

        public int GetSizeInVec4()
        {
            return _data.Length;
        }

        public Vector4[] GetSTD140Data()
        {
            return _data;
        }

        public TextureType GetTextureType()
        {
            return _type;
        }
    }
}
