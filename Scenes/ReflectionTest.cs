using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Scenes
{
    public class ReflectionTest:BaseScene
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


            Model back = new Model(red);
            back.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(-2, 2, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), false, red));
            back.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(2, 0, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), false, red));
            _list.AddHittable(back);

            Model lights = new Model(light);
            lights.addTriangle(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(3, 3, -3), new Vector3(3, 3, 3), false, light));
            lights.addTriangle(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(-3, 3, 3), new Vector3(3, 3, 3), false, light));
            _list.AddHittable(lights);

            _list.AddHittable(new Sphere(new Vector3(0, 0.5f, -1), 0.5f, metal2));
            _list.AddHittable(new Sphere(new Vector3(-1.2f, 0.5f, -1), 0.5f, metal1));
            _list.AddHittable(new Sphere(new Vector3(1.2f, 0.5f, -1), 0.5f, metal3));
            _list.AddHittable(new Sphere(new Vector3(0, -100f, -1), 100f, checker));
        }
        public ReflectionTest()
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(1, 33.0f, new Vector3(0, 1, 6));

            Metal metal1 = new Metal(new SolidColor(new Vector3(1)), 0);
            Metal metal2 = new Metal(new SolidColor(new Vector3(1)), 0.3f);
            Metal metal3 = new Metal(new SolidColor(new Vector3(1)), 0.9f);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Lambertian paper = new Lambertian(new Wallpaper(new Vector3(1f, 0.1f, 0.1f), new Vector3(1.0f, 0.32f, 0.29f)));
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3i(1000, 1000, 1000)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3i(220, 20, 20)));
            Lambertian blue = new Lambertian(new SolidColor(new Vector3i(20, 20, 220)));


            Model back = new Model(red);
            back.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(-2, 2, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), false, red));
            back.addTriangle(new TriangleMT(new Vector3(-2, 0, -2), new Vector3(2, 0, -2), new Vector3(2, 2, -2), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), false, red));
            _list.AddHittable(back);

            Model lights = new Model(light);
            lights.addTriangle(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(3, 3, -3), new Vector3(3, 3, 3), false, light));
            lights.addTriangle(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(-3, 3, 3), new Vector3(3, 3, 3), false, light));
            _list.AddHittable(lights);

            _list.AddHittable(new Sphere(new Vector3(0, 0.5f, -1), 0.5f, metal2));
            _list.AddHittable(new Sphere(new Vector3(-1.2f, 0.5f, -1), 0.5f, metal1));
            _list.AddHittable(new Sphere(new Vector3(1.2f, 0.5f, -1), 0.5f, metal3));
            _list.AddHittable(new Sphere(new Vector3(0, -100f, -1), 100f, checker));
        }
    }
}
