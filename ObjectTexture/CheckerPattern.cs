﻿using OpenTK.Mathematics;

namespace Dull.ObjectTexture
{
    class CheckerPattern : ITexture
    {
        private static int _objectSize = 2;
        private Vector3 _odd;
        private Vector3 _even;
        private const TextureType _type = TextureType.Checker;
        private Vector4[] _data;
        public CheckerPattern(Vector3 odd, Vector3 even)
        {
            _odd = odd;
            _even = even;
            _data = new Vector4[_objectSize];
            SetStd140Data();
        }
        public CheckerPattern()
        {
            _odd = new Vector3(0);
            _even = new Vector3(1);
            _data = new Vector4[_objectSize];
            SetStd140Data();
        }
        public CheckerPattern(Vector3 albedo)
        {
            _odd = albedo;
            _even = new Vector3(1);
            _data = new Vector4[_objectSize];
            SetStd140Data();
        }
        private void SetStd140Data()
        {
            _data[0].Xyz = _odd;
            _data[0].W = (float)_type;
            _data[1].Xyz = _even;
        }
        public string GetInfo()
        {
            return $"Texture:Checker Pattern Color(odd): {_data[0].Xyz}, Color(even): {_data[1].Xyz}";
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
            SetStd140Data();
            return _data;
        }

        public TextureType GetTextureType()
        {
            return _type;
        }
        public Vector3[] GetAlbedo()
        {
            return new Vector3[] { _odd,_even };
        }

        public void SetAlbedo(params Vector3[] albedo)
        {
            _odd =  albedo[0];
            _even = albedo[1];
        }
    }
}
