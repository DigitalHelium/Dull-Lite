using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;

namespace Dull.Scenes
{
    internal class RoomScene2:BaseScene
    {
        public RoomScene2(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 26.0f, new Vector3(0, 5.3f, 30), 3);


            Lambertian white = new Lambertian(new SolidColor(new Vector3i(180, 180, 180)));
            Lambertian green = new Lambertian(new SolidColor(new Vector3i(30, 255, 30)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3i(255, 30, 30)));
            Lambertian blue = new Lambertian(new SolidColor(new Vector3i(30, 30, 125)));
            Metal metal = new Metal(new SolidColor(new Vector3i(150, 150, 255)), 0);
            Dielectric die = new Dielectric(new SolidColor(new Vector3(0)), 1.4f);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3i(1000, 1000, 1000)));
            DiffuseLight light16 = new DiffuseLight(new SolidColor(new Vector3i(16000, 16000, 16000)));

            //_lights.AddLight(new PointLight(new Vector3(-2, 3.8f, 1), 1, new Vector3i(90, 70, 255)));
            //_lights.AddLight(new PointLight(new Vector3(2, 5, 1), 1, new Vector3i(255, 125, 50)));
            //_lights.AddLight(new PointLight(new Vector3(0, 9, 0), 1, new Vector3i(40, 40, 40)));


            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(5, 0, 5), new Vector3(-5, 0, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 0, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, -5), new Vector3(-5, 0, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, 5), new Vector3(-5, 10, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, -5), new Vector3(5, 0, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, 5), new Vector3(5, 10, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(-5, 10, -5), new Vector3(5, 10, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 10, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 10, -5), new Vector3(5, 10, -5), new Vector3(5, 10, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 10, -5), new Vector3(-5, 10, 5), new Vector3(5, 10, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-4, 9.9f, -4), new Vector3(4, 9.9f, -4), new Vector3(4, 9.9f, 4), false, light));
            _list.AddHittable(new TriangleMT(new Vector3(-4, 9.9f, -4), new Vector3(-4, 9.9f, 4), new Vector3(4, 9.9f, 4), false, light));

            //_list.AddHittable(new TriangleMT(new Vector3(-1, 9.9f, -1), new Vector3(1, 9.9f, -1), new Vector3(1, 9.9f, 1), false, light16));
           // _list.AddHittable(new TriangleMT(new Vector3(-1, 9.9f, -1), new Vector3(-1, 9.9f, 1), new Vector3(1, 9.9f, 1), false, light16));

            Vector3 a = new Vector3(2.81f, 0, -1.96f);
            Vector3 b = new Vector3(0, 0, -0.44f);
            Vector3 d = new Vector3(4.19f, 0, 0.75f);
            Vector3 e = new Vector3(1.39f, 0, 2.18f);
            Vector3 a1 = new Vector3(2.81f, 6, -1.96f);
            Vector3 b1 = new Vector3(0, 6, -0.44f);
            Vector3 d1 = new Vector3(4.19f, 6, 0.75f);
            Vector3 e1= new Vector3(1.39f, 6, 2.18f);
            _list.AddHittable(new TriangleMT(a1,b1,e1, false, white));
            _list.AddHittable(new TriangleMT(a1,e1,d1, false, white));
            _list.AddHittable(new TriangleMT(a,b,b1, false, white));
            _list.AddHittable(new TriangleMT(a, b1, a1, false, white));
            _list.AddHittable(new TriangleMT(b, e, e1, false, white));
            _list.AddHittable(new TriangleMT(b, e1, b1, false, white));
            _list.AddHittable(new TriangleMT(e, d, d1, false, white));
            _list.AddHittable(new TriangleMT(e, d1, e1, false, white));
            _list.AddHittable(new TriangleMT(d, a, a1, false, white));
            _list.AddHittable(new TriangleMT(d, a1, d1, false, white));

            _list.AddHittable(new Sphere(new Vector3(-2.5f, 1.4f, 1.2f), 1.4f, white));


        }
    }
}
