﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dull.Objects
{
    public class HittableList
    {
        List<IHittable> _hitList = new List<IHittable>();

        private StorageBuffer _buf;

        private int _spheresLocation = 0;
        private int _trianglemtLocation = 1;

        private const int SIZE_OFFSET = 16;

        private int _sphereCount;
        private int _trianglemtCount;
        private int _modelTriangleCount;

        private int _sphereSizeInBytes = SIZE_OFFSET;
        private int _trianglemtSizeInBytes = SIZE_OFFSET;
        private int _modelSizeInBytes = 0;
        public HittableList()
        {
            _buf = new StorageBuffer();
        }
        public void ChangeProgram(int shaderHandle)
        {
            _spheresLocation = GL.GetProgramResourceIndex(shaderHandle, ProgramInterface.ShaderStorageBlock, "Spheres");
            _trianglemtLocation = GL.GetProgramResourceIndex(shaderHandle, ProgramInterface.ShaderStorageBlock, "TrianglesMT");
        }
        public void AddHittable(IHittable hittable)
        {
            _hitList.Add(hittable);
        }
        public void RemoveHittable(IHittable hittable)
        {
            _hitList.Remove(hittable);
        }
        public void DataToBuffer(int shaderHandle)
        {
            _trianglemtSizeInBytes = SIZE_OFFSET;
            _sphereSizeInBytes = SIZE_OFFSET;
            _modelSizeInBytes = 0;

            CalcSizes();
            _buf.ResizeBuffer(_sphereSizeInBytes+_trianglemtSizeInBytes+_modelSizeInBytes);

            _buf.BindBufferRange(_spheresLocation, 0, _sphereSizeInBytes);
            _buf.BindBufferRange(_trianglemtLocation, _sphereSizeInBytes, _trianglemtSizeInBytes+_modelSizeInBytes);

            _buf.AttachSubData(BitConverter.GetBytes(_sphereCount), 0);
            _buf.AttachSubData(BitConverter.GetBytes(_trianglemtCount + _modelTriangleCount), _sphereSizeInBytes);

                                                
            foreach(IHittable obj in _hitList)
            {
               Vector4[] tempData = obj.GetStd140Data();
               _buf.AttachSubData(tempData, obj.GetOffset());
            }
            

        }
        public void ClearBuffer()
        {
            _buf.ClearData();
        }
        public void ChangeHittable(IHittable hittable)
        {
            Vector4[] tempData = hittable.GetStd140Data();
            _buf.AttachSubData(tempData, hittable.GetOffset());
        }
        public List<IHittable> GetHittables()
        {
            return _hitList;
        }
        private void CalcSizes()
        {
            _sphereSizeInBytes = GetOffsets(HittableType.Sphere, _sphereSizeInBytes, out _sphereCount, SIZE_OFFSET);
            _trianglemtSizeInBytes = GetOffsets(HittableType.TriangleMT, _sphereSizeInBytes + _trianglemtSizeInBytes, out _trianglemtCount, SIZE_OFFSET);
            _modelSizeInBytes = GetOffsets(HittableType.Model, _sphereSizeInBytes + _trianglemtSizeInBytes + _modelSizeInBytes, out _modelTriangleCount,0);
        }

        private int GetOffsets(HittableType type,int offset, out int count, int sizeOffset)
        {
            count = 0;
            int objOffset = offset;
            foreach (IHittable obj in _hitList)
            {
                if (obj.GetHitType() == type)
                {
                    obj.SetOffset(objOffset);
                    objOffset += obj.GetSizeInBytes();
                    count+=obj.GetCount();
                }
            }
            return objOffset-offset+ sizeOffset;
        }

    }
}
