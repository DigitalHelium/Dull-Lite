using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Scenes
{
    internal class LightTest: BaseScene
    {
        public LightTest(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 40.0f, new Vector3(0, 1, 6));

            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Lambertian white = new Lambertian(new SolidColor(new Vector3i(249,246,238)));
            Metal metal = new Metal(new SolidColor(new Vector3(1)), 0);

            float intensity = 0.5f;
            float height = 2;
            _lights.AddLight(new PointLight(new Vector3(0, height, 1), intensity, new Vector3i(0, 0, 255)));
            _lights.AddLight(new PointLight(new Vector3(-1, height, 1), intensity, new Vector3i(197, 0, 58)));
            _lights.AddLight(new PointLight(new Vector3(-1, height, 0), intensity, new Vector3i(116, 0, 139)));
            _lights.AddLight(new PointLight(new Vector3(-1, height, -1), intensity, new Vector3i(0, 0, 255)));
            _lights.AddLight(new PointLight(new Vector3(0, height, -1), intensity, new Vector3i(0, 10, 220)));
            _lights.AddLight(new PointLight(new Vector3(1, height, -1), intensity, new Vector3i(0, 120, 135)));
            _lights.AddLight(new PointLight(new Vector3(1, height, 0), intensity, new Vector3i(0, 219, 36)));
            _lights.AddLight(new PointLight(new Vector3(1, height, 1), intensity, new Vector3i(0, 255, 0)));

            _list.AddHittable(new TriangleMT(new Vector3(-4, 0, 4), new Vector3(4, 0, 4), new Vector3(-4, 0, -4), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(4, 0, -4), new Vector3(-4, 0, -4), new Vector3(4, 0, 4), false, white));


            _list.AddHittable(new Sphere(new Vector3(0, 0.8f, 0), 0.8f, white));

        }
    }
}
