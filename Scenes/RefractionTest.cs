using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;
using static OpenTK.Graphics.OpenGL.GL;

namespace Dull.Scenes
{
    internal class RefractionTest:BaseScene
    {
        public RefractionTest(float ratio)
        {
            _list = new HittableList();
            _lights = new LightList();
            _camera = new Camera(ratio, 33.0f, new Vector3(0, 1, 6));

            Metal metal1 = new Metal(new SolidColor(new Vector3(1)), 0);
            Metal metal2 = new Metal(new SolidColor(new Vector3(1)), 0.3f);
            Metal metal3 = new Metal(new SolidColor(new Vector3(1)), 0.9f);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Lambertian paper = new Lambertian(new Wallpaper(new Vector3(1f, 0.1f, 0.1f), new Vector3(1.0f, 0.32f, 0.29f)));
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3i(1000, 1000, 1000)));
            Lambertian red = new Lambertian(new SolidColor(new Vector3i(220, 20, 20)));
            Dielectric die1 = new Dielectric(new SolidColor(new Vector3(0)), 1);
            Dielectric die2 = new Dielectric(new SolidColor(new Vector3(0)), 1.3f);
            Dielectric die3 = new Dielectric(new SolidColor(new Vector3(0)), 2);

            _list.AddHittable(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(3, 3, -3), new Vector3(3, 3, 3), false, light));
            _list.AddHittable(new TriangleMT(new Vector3(-3, 3, -3), new Vector3(-3, 3, 3), new Vector3(3, 3, 3), false, light));

            _list.AddHittable(new Sphere(new Vector3(-1.1f, 0.5f, -1.7f), 0.1f, red));
            _list.AddHittable(new Sphere(new Vector3(0.1f, 0.5f, -1.7f), 0.1f, red));
            _list.AddHittable(new Sphere(new Vector3(1.3f, 0.5f, -1.7f), 0.1f, red));

            _list.AddHittable(new Sphere(new Vector3(0, 0.5f, -1), 0.5f, die2));
            _list.AddHittable(new Sphere(new Vector3(-1.2f, 0.5f, -1), 0.5f, die1));
            _list.AddHittable(new Sphere(new Vector3(1.2f, 0.5f, -1), 0.5f, die3));
            _list.AddHittable(new Sphere(new Vector3(0, -100f, -1), 100f, checker));
        }
    }
}
