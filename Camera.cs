using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Dull
{
    class Camera
    {
        private float _aspectRatio;
        private float _vfov;
        private Vector3 _lookFrom;
        private Vector3 _lookAt;
        
        private Vector3 _front;
        private Vector3 _right;
        private static Vector3 _vup = new Vector3(0, 1, 0);

        private float _speed = 1.5f;
        private float _sprintMult = 1.5f;
        private float _sensitivity = 0.2f;

        private float _yaw = -90.0f;
        private float _pitch = 0;

        private int _screenWidthLocation;
        private int _screenHeightLocation;
        private int _aspectRatioLocation;
        private int _vfovLocation;
        private int _lookFromLocation;
        private int _lookAtLocation;

        public Camera(float aspectRatio, float vfov, Vector3 lookFrom, float speed)
        {
            _aspectRatio = aspectRatio;
            _vfov = vfov;
            _lookFrom = lookFrom;
            _front = new Vector3(0, 0, -1);
            _right = Vector3.Cross(_front, _vup);
            _lookAt = lookFrom + _front;
            _speed = speed;
        }
        public Camera(float aspectRatio, float vfov, Vector3 lookFrom)
        {
            _aspectRatio = aspectRatio;
            _vfov = vfov;
            _lookFrom = lookFrom;
            _front = new Vector3(0, 0, -1);
            _right = Vector3.Cross(_front, _vup);
            _lookAt = lookFrom + _front;
        }
        public void UpdateParamLocations(int shaderHandle)
        {
            _screenWidthLocation = GL.GetUniformLocation(shaderHandle, "image_width");
            _screenHeightLocation = GL.GetUniformLocation(shaderHandle, "image_height");
            _aspectRatioLocation = GL.GetUniformLocation(shaderHandle, "aspect_ratio");
            _vfovLocation = GL.GetUniformLocation(shaderHandle, "vfov");
            _lookFromLocation = GL.GetUniformLocation(shaderHandle, "look_from");
            _lookAtLocation = GL.GetUniformLocation(shaderHandle, "look_at");
        } 
        public void UpdateCameraPosition(KeyboardState state, float deltaTime)
        {
            Vector3 offset = new Vector3(0, 0, 0);
            float speedDt = _speed * deltaTime;
            if (state.IsKeyDown(Keys.W))
            {
                if (state.IsKeyDown(Keys.LeftShift))
                    offset += _front * speedDt * _sprintMult;
                else
                    offset += _front * speedDt;
            }
            if (state.IsKeyDown(Keys.A))
            {
                offset -= _right * speedDt;
            }
            if (state.IsKeyDown(Keys.S))
            {
                offset -= _front * speedDt;
            }
            if (state.IsKeyDown(Keys.D))
            {
                offset += _right * speedDt;
            }

            if (state.IsKeyDown(Keys.Space))
            {
                offset.Y += speedDt;
            }

            if (state.IsKeyDown(Keys.LeftControl))
            {
                offset.Y -= speedDt;
            }
            if (state.IsKeyPressed(Keys.C))
            {
                _vfov /= 4;
                _sensitivity /= 5;
            }
            if (state.IsKeyReleased(Keys.C))
            {
                _vfov *= 4;
                _sensitivity *= 5;
            }
            _lookFrom += offset;
            _lookAt = _lookFrom + _front;
        } 

        public void UpdateCameraRotation(float deltaX, float deltaY)
        {
            _yaw += deltaX * _sensitivity;
            _pitch -= deltaY * _sensitivity;

            if (_pitch > 89.9f) _pitch = 89.9f;
            if (_pitch < -89.9f) _pitch = -89.9f;

            Vector3 offset = new Vector3();

            offset.X = MathF.Cos(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));
            offset.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            offset.Z = MathF.Sin(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));
            offset.Normalize();

            _front = offset;
            _right = Vector3.Cross(_front, _vup);
           _lookAt = _lookFrom + offset;
        }
        public int ScreenWidthLocation { get => _screenWidthLocation;}
        public int ScreenHeightLocation { get => _screenHeightLocation; }
        public int AspectRatioLocation { get => _aspectRatioLocation; }
        public int FovLocation { get => _vfovLocation; }
        public int LookFromLocation { get => _lookFromLocation; }
        public int LookAtLocation { get => _lookAtLocation; }
        public float AspectRatio { get => _aspectRatio; set => _aspectRatio = value; }
        public float FOV { get => _vfov; set => _vfov = value; }
        public Vector3 LookFrom { get => _lookFrom; set { _lookFrom = value; _lookAt = value + _front; } }
        public Vector3 LookAt { get => _lookAt; set => _lookAt = value; }
        public Vector3 Front { get => _front; set { _front = value; _right = Vector3.Cross(_front, _vup); } }
        public Vector3 Right { get => _right; }
        public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }
        public float Speed { get => _speed; set => _speed = value; }
    }
}
