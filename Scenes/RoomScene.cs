using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;

namespace Dull.Scenes
{
    class RoomScene:BaseScene
    {
        public RoomScene(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 90.0f, new Vector3(0, 5, 10),3);


            Lambertian white = new Lambertian(new SolidColor(new Vector3(1)));
            Lambertian green = new Lambertian(new SolidColor(new Vector3(0,1,0)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3(1,0,0)));
            Metal metal = new Metal(new SolidColor(new Vector3(0)), 0);
            DiffuseLight diffuseLight = new DiffuseLight(new SolidColor(new Vector3(1)));


            _lights.AddLight(new PointLight(new Vector3(-2, 1, -1), 5, new Vector3(1f, 0.5f, 0.3f)));
            _lights.AddLight(new PointLight(new Vector3(1, 1, -1), 5, new Vector3(1, 0.3f, 0.1f)));


            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(5, 0, 5), new Vector3(-5, 0, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 0, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, -5), new Vector3(-5, 0, -5), false, red));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, 5), new Vector3(-5, 10, -5), false, red));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, -5), new Vector3(5, 0, -5), false, green));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, 5), new Vector3(5, 10, -5), false, green));

            _list.AddHittable(new Sphere(new Vector3(0, 5, -5), 1, metal));
            //_list.AddHittable(new Sphere(new Vector3(0, 2, -0), 0.5f, diffuseLight));
        }
    }
}
