﻿using Dull.Lights;
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
            _camera = new Camera(ratio, 40.0f, new Vector3(0, 5, 20),3);


            Lambertian white = new Lambertian(new SolidColor(new Vector3i(180,180,180)));
            Lambertian green = new Lambertian(new SolidColor(new Vector3i(30,255,30)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3i(255,30,30)));
            Lambertian blue = new Lambertian(new SolidColor(new Vector3i(30, 30, 125)));
            Metal metal = new Metal(new SolidColor(new Vector3i(150,150,255)), 0);
            Dielectric die = new Dielectric(new SolidColor(new Vector3(0)), 1.4f);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3(0.7f, 0.3f, 1f)));


            _lights.AddLight(new PointLight(new Vector3(-2, 3.8f, 1), 1, new Vector3i(90,70,255)));
            _lights.AddLight(new PointLight(new Vector3(2, 5, 1), 1, new Vector3i(255, 125, 50)));


            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(5, 0, 5), new Vector3(-5, 0, -5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 0, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, -5), new Vector3(-5, 0, -5), false, red));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, 5), new Vector3(-5, 10, -5), false, red));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, -5), new Vector3(5, 0, -5), false, green));
            _list.AddHittable(new TriangleMT(new Vector3(5, 0, 5), new Vector3(5, 10, 5), new Vector3(5, 10, -5), false, green));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(-5, 10, -5), new Vector3(5, 10, -5), false, checker));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, -5), new Vector3(5, 0, -5), new Vector3(5, 10, -5), false, checker));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 10, -5), new Vector3(5, 10, -5), new Vector3(5, 10, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 10, -5), new Vector3(-5, 10, 5), new Vector3(5, 10, 5), false, white));
            _list.AddHittable(new TriangleMT(new Vector3(-5, 0, 5), new Vector3(-5, 10, 5), new Vector3(5, 10, 5), true, blue));
            _list.AddHittable(new TriangleMT(new Vector3(5, 10, 5), new Vector3(5, 0, 5), new Vector3(-5, 0, 5), true, blue));

            _list.AddHittable(new Sphere(new Vector3(-2.2f, 2, -0.7f), 2f, metal));
            _list.AddHittable(new Sphere(new Vector3(-3.5f, 1.4f, 2.5f), 1.2f, die));
            _list.AddHittable(new Sphere(new Vector3(2.5f, 1.5f, -0.7f), 1f, checker));
            _list.AddHittable(new Sphere(new Vector3(0.4f, 1.2f, 2.4f), 1f, light));
            //_list.AddHittable(new Sphere(new Vector3(0, 2, -0), 0.5f, diffuseLight));
        }
    }
}
