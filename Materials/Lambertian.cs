using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Materials
{
    class Lambertian : IMaterial
    {
        private static int _objectSize = 1;
        private ITexture _tex;
        private const MaterialType _type = MaterialType.Lambertian;
        private Vector4[] _data;
        public Lambertian(ITexture tex)
        {
            _tex = tex;
            _data = new Vector4[_objectSize+_tex.GetSizeInVec4()];
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
            return $"Material:Lambertian\n  {_tex.GetInfo()}";
        }

        public float? GetParam()
        {
            return null;
        }
        public void SetParam(float? param)
        {
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
