using Dull.Lights;
using Dull.Objects;
namespace Dull.Scenes
{
    abstract class BaseScene
    {
        protected HittableList _list;
        protected LightList _lights;
        protected Camera _camera;

        public HittableList HitList { get => _list; set => _list = value; }
        public LightList LightList { get => _lights; set => _lights = value; }
        public Camera Camera { get => _camera; set => _camera = value; }

    }
}
