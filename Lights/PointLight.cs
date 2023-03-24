﻿using Dull.Materials;
using Dull.Objects;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using System;

namespace Dull.Lights
{
    internal class PointLight : ILight
    {
        private Vector3 _position;
        private int _intensity;
        private Vector3 _color;

        private static int _objectSize = 2;//in Vector4
        private LightType _type = LightType.PointLight;
        Vector4[] _data;
        private int _byteOffset = -1;
        public PointLight(Vector3 position, int intensity, Vector3 color)
        {
            _position = position;
            _intensity = intensity;
            _color = color;
            _data = new Vector4[_objectSize];
            SetStd140Data();
        }
        private void SetStd140Data()
        {
            _data[0].Xyz = _position;
            _data[0].W = _intensity;
            _data[1].Xyz = _color;
        }

        public LightType GetLightType()
        {
            return _type;
        }

        public string GetInfo()
        {
            return $"Light:PointLight Position:{_data[0]} Intensity: {_intensity} Color: {_color}\n";
        }

        public int GetOffset()
        {
            return _byteOffset;
        }

        public int GetSizeInBytes()
        {
            return _data.Length * Vector4.SizeInBytes;
        }

        public Vector4[] GetStd140Data()
        {
            SetStd140Data();
            Console.WriteLine(GetInfo());
            return _data;
        }

        public void SetOffset(int offset)
        {
            _byteOffset = offset;
        }
    }
}