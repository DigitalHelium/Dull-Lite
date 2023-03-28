using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dull.Lights
{
    internal class LightList
    {
        List<ILight> _lights = new List<ILight>();

        private StorageBuffer _buf = new StorageBuffer();

        private int _pointLightLocation = 2;

        private const int SIZE_OFFSET = 16;

        private int _pointLightCount;

        private int _pointLightSizeInBytes = SIZE_OFFSET;

        public LightList() { }

        public void AddLight(ILight light) 
        {
            _lights.Add(light);
        }

        public void DataTobuffer(int shaderHandle)
        {
            CalcSizes();
            _buf.ResizeBuffer(_pointLightSizeInBytes);

            _buf.BindBufferRange(_pointLightLocation, 0, _pointLightSizeInBytes);

            _buf.AttachSubData(BitConverter.GetBytes(_pointLightCount), 0);

            foreach(ILight light in _lights)
            {
                Vector4[] tempData = light.GetStd140Data();
                _buf.AttachSubData(tempData, light.GetOffset());
            }
        }
        public void ChangeLight(ILight light)
        {
            Vector4[] tempData = light.GetStd140Data();
            _buf.AttachSubData(tempData, light.GetOffset());
        }
        public List<ILight> GetLights()
        {
            return _lights;
        }

        private void CalcSizes()
        {
            _pointLightSizeInBytes = GetOffSets(LightType.PointLight,_pointLightSizeInBytes,out _pointLightCount);
        }

        private int GetOffSets(LightType type, int offset, out int count)
        {
            count = 0;
            int objOffset = offset;
            foreach(ILight light in _lights)
            {
                if(light.GetLightType() == type)
                {
                    light.SetOffset(objOffset);
                    objOffset += light.GetSizeInBytes();
                    count++;
                }
            }
            return objOffset - offset + SIZE_OFFSET;
        }

    }
}
