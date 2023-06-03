using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Scenes
{
    internal class ReflectionTest:BaseScene
    {
        public ReflectionTest(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 33.0f, new Vector3(0, 1, 6));

            Metal metal1 = new Metal(new SolidColor(new Vector3(1)), 0);
            Metal metal2 = new Metal(new SolidColor(new Vector3(1)), 0.3f);
            Metal metal3 = new Metal(new SolidColor(new Vector3(1)), 0.9f);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Lambertian paper = new Lambertian(new Wallpaper(new Vector3(1f,0.1f,0.1f), new Vector3(1.0f, 0.32f, 0.29f)));
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3i(1000, 1000, 1000)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3i(220,20,20)));
            Lambertian blue = new Lambertian(new SolidColor(new Vector3i(20, 20, 220)));

            _list.AddHittable(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(3, 3, -3), new Vector3(3, 3, 3), false, light));
            _list.AddHittable(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(-3, 3, 3), new Vector3(3, 3, 3), false, light));

            
       

            //var loader = new ObjLoader();
            //loader.Load("C:\\Users\\amis1\\Downloads\\monkey.obj");
            //Model m = new Model(red);
            //for (int i=0;i<loader._faces.Count;i++)
            //{
            //    var face = loader._faces[i];
            //    m.addTriangle(new TriangleMT(loader._verts[face.X-1], loader._verts[face.Y - 1], loader._verts[face.Z - 1], false, red));
            //}
            //_list.AddHittable(m);

            Model m1 = new Model(red);
            m1.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(-2, 2, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), false, red));
            m1.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(2, 0, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), false, red));
            _list.AddHittable(m1);

            _list.AddHittable(new Sphere(new Vector3(0, 0.5f, -1), 0.5f, metal2));
            _list.AddHittable(new Sphere(new Vector3(-1.2f, 0.5f, -1), 0.5f, metal1));
            _list.AddHittable(new Sphere(new Vector3(1.2f, 0.5f, -1), 0.5f, metal3));
            _list.AddHittable(new Sphere(new Vector3(0, -100f, -1), 100f, checker));
        }
    }
}
