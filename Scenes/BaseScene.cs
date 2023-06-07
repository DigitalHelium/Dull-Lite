using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;

using OpenTK.Mathematics;
using System;

namespace Dull.Scenes
{
    public abstract class BaseScene
    {
        protected HittableList _list;
        protected LightList _lights;
        protected Camera _camera;

        public HittableList HitList { get => _list; set => _list = value; }
        public LightList LightList { get => _lights; set => _lights = value; }
        public Camera Camera { get => _camera; set => _camera = value; }

        public void UpdateData(int shaderHandle)
        {
            Camera.UpdateParamLocations(shaderHandle);
            HitList.DataToBuffer(shaderHandle);
            LightList.DataTobuffer(shaderHandle);
        }
        public void AddModel(string path) 
        {
            var loader = new ObjLoader();
            loader.Load(path);
            Random rnd = new Random();
            Lambertian r = new Lambertian(new SolidColor(new Vector3i(rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255))));
            Model m = new Model(r);
            for (int i = 0; i < loader._faces.Count; i++)
            {
                var face = loader._faces[i];
                if (i < loader._facesTex.Count)
                {
                    var faceTex = loader._facesTex[i];
                    m.addTriangle(new TriangleMT(loader._verts[face.X - 1], loader._verts[face.Y - 1], loader._verts[face.Z - 1], loader._texcoords[faceTex.X - 1], loader._texcoords[faceTex.Y - 1], loader._texcoords[faceTex.Z - 1], false, r));
                }
                else
                    m.addTriangle(new TriangleMT(loader._verts[face.X - 1], loader._verts[face.Y - 1], loader._verts[face.Z - 1], false, r));
            }
            _list.AddHittable(m);
        }
        public void AddObject()
        {
            Lambertian r = new Lambertian(new SolidColor(new Vector3i(220, 20, 20)));
            _list.AddHittable(new Sphere(new Vector3(0,0.5f,0),0.3f,r));
        }

    }
}
