using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Scenes
{
    class GrassScene:BaseScene
    {
        public GrassScene(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 90.0f, new Vector3(0, 0, 0));


            Lambertian red = new Lambertian(new SolidColor(new Vector3(1.0f, 0.0f, 0.0f)));
            Lambertian green = new Lambertian(new SolidColor(new Vector3(0.0f, 1.0f, 0.0f)));
            Metal white = new Metal(new SolidColor(new Vector3(1)), 0);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Transparent trans = new Transparent(new SolidColor(new Vector3(0.3f, 0.1f, 0.5f)), 1.1f);
            Dielectric die = new Dielectric(new SolidColor(new Vector3(0)), 1.5f);
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3(0.7f, 0.3f, 1f)));


            _lights.AddLight(new PointLight(new Vector3(-2, 1, -1), 20, new Vector3(0.1f, 0.3f, 1)));
            _lights.AddLight(new PointLight(new Vector3(1, 1, -1), 20, new Vector3(1, 0.3f, 0.1f)));
            

            _list.AddHittable(new TriangleMT(new Vector3(-1, 0, -3), new Vector3(1, 0, -3), new Vector3(0, 1, -3), false, white));
            _list.AddHittable(new Sphere(new Vector3(0.8f, 0, -1), 0.5f, light));
            _list.AddHittable(new Sphere(new Vector3(0, -100.5f, -1), 100f, green));
            _list.AddHittable(new Sphere(new Vector3(-1.3f, 0.3f, -2), 0.8f, die));
            _list.AddHittable(new Sphere(new Vector3(-0.7f, 0, -1), 0.3f, checker));
        }
    }
}
