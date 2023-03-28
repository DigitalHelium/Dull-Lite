using System;
using System.Collections.Generic;
using Dull.GUI;
using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Dull
{
    class Screen:GameWindow
    {
        ImGuiController _controller;

        private Shader _shader;
        private ComputeShader _intersectionShader;

        private Texture _renderQuad;

        private HittableList _list;
        private LightList _lights;

        private MiscUniforms _misc;

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        float[] _vertices = {
             1f,  1f, 0.0f,  // top right
             1f, -1f, 0.0f,  // bottom right
            -1f, -1f, 0.0f,  // bottom left
            -1f,  1f, 0.0f   // top left
        };
        uint[] _indices = {  
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        private Camera _camera;
        DateTime _startTime = new DateTime(1970, 1, 1);

        public Screen(int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height)}) { }
       
        protected override void OnLoad()
        {
            base.OnLoad();
            CursorState = CursorState.Grabbed;

            _shader = new Shader(@"..\..\..\Shaders\CameraShader.vert", @"..\..\..\Shaders\CameraShader.frag");

            _shader.Use();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);


            _intersectionShader = new ComputeShader(@"..\..\..\Shaders\Intersection.comp");
            _intersectionShader.Use();

            _camera = new Camera((float)Size.X / Size.Y, 90.0f, new Vector3(0, 0, 0));
            _camera.UpdateParamLocations(_intersectionShader.Handle);

            _misc = new MiscUniforms(0, 1, 16);
            _misc.UpdateParamLocations(_intersectionShader.Handle);
            GL.Uniform1(_misc.ModeLocation, _misc.Mode);
            GL.Uniform1(_misc.MaxSamplesLocation, _misc.MaxSamples);
            GL.Uniform1(_misc.MaxDepthLocation, _misc.MaxDepth);

            _list = new HittableList();
            
            Lambertian red = new Lambertian(new SolidColor(new Vector3(1.0f, 0.0f, 0.0f)));
            Lambertian green = new Lambertian(new SolidColor(new Vector3(0.0f, 1.0f, 0.0f)));
            Metal white = new Metal(new SolidColor(new Vector3(0)),0);
            Lambertian checker = new Lambertian(new CheckerPattern(new Vector3(0), new Vector3(1)));
            Transparent trans = new Transparent(new SolidColor(new Vector3(0.3f, 0.1f, 0.5f)), 1.1f);
            Dielectric die = new Dielectric(new SolidColor(new Vector3(0)), 1.5f);
            DiffuseLight light = new DiffuseLight(new SolidColor(new Vector3(0.7f, 0.3f,1f)));

            _lights = new LightList();

            _lights.AddLight(new PointLight(new Vector3(-2, 1, -1), 20, new Vector3(0.1f,0.3f,1)));
            _lights.AddLight(new PointLight(new Vector3(1, 1, -1), 20, new Vector3(1,0.3f,0.1f)));
            _lights.DataTobuffer(_intersectionShader.Handle);

            _list.AddHittable(new TriangleMT(new Vector3(-1, 0, -3), new Vector3(1, 0, -3), new Vector3(0, 1, -3), false, white));
            _list.AddHittable(new Sphere(new Vector3(0.8f, 0, -1), 0.5f, light));
            _list.AddHittable(new Sphere(new Vector3(0, -100.5f, -1), 100f, green));
            _list.AddHittable(new Sphere(new Vector3(-1.3f, 0.3f, -2), 0.8f, die));
            _list.AddHittable(new Sphere(new Vector3(-0.7f, 0, -1), 0.3f, checker));

            // _list.AddHittable(new TriangleMT(new Vector3(-1, 0, -1), new Vector3(-1, 0.2f, -3), new Vector3(1, 0, -1), false, white));
            // _list.AddHittable(new TriangleMT(new Vector3(-1, 0.2f, -3), new Vector3(1, 0, -1), new Vector3(1, 0.2f, -3), false, white));
            //_list.AddHittable(new Sphere(new Vector3(0, 0.7f, -2), 0.5f, red));
            _list.DataToBuffer(_intersectionShader.Handle);

            

            _renderQuad = new Texture(Size.X, Size.Y, PixelInternalFormat.Rgb32f, PixelFormat.Rgba, (IntPtr)0,4);

            _controller = new ImGuiController(Size.X, Size.Y);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            _controller.Update(this, (float)args.Time);//new
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (RenderTime < 0.6)
                Title = (int)(1 / RenderTime) + " FPS";
            else
                Title = Math.Round(RenderTime, 3).ToString() + " Seconds per Frame";
            SetNewCameraPosition();

            _intersectionShader.Use();
            GL.Uniform1(_camera.FovLocation, _camera.FOV);
            GL.Uniform3(_camera.LookFromLocation, _camera.LookFrom);
            GL.Uniform3(_camera.LookAtLocation, _camera.LookAt);


            GL.Uniform1(_misc.SeedLocation, (float)e.Time%1);

            GL.DispatchCompute(Size.X/8, Size.Y/4, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            _shader.Use();
            GL.ClearColor(0, 0, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _renderQuad.Handle);


            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);


            OnDrawGUI();//new
            _controller.Render();
            SwapBuffers();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            _intersectionShader.Use();
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.Uniform1(_camera.ScreenWidthLocation, e.Width);
            GL.Uniform1(_camera.ScreenHeightLocation, e.Height);
            _camera.AspectRatio = (float)e.Width / e.Height;
            GL.Uniform1(_camera.AspectRatioLocation, _camera.AspectRatio);
            _shader.Use();

            _renderQuad.UpdateTextureParams(e.Width, e.Height, (IntPtr)0);

            _controller.WindowResized(e.Width, e.Height);
            base.OnResize(e);
            
        }
        private void SetNewCameraPosition()
        {
            if(KeyboardState.IsAnyKeyDown || KeyboardState.WasKeyDown(Keys.C))
                _camera.UpdateCameraPosition(KeyboardState, (float)RenderTime);
        }
        bool isCursorGrabbed = true;
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Keys.Escape)
                Close();

            if (e.Key == Keys.Tab)
            {
                if (isCursorGrabbed) 
                {
                    CursorState = CursorState.Normal;
                    isCursorGrabbed = false;
                }
                else 
                {
                    CursorState = CursorState.Grabbed;
                    isCursorGrabbed = true;
                }
            }


            if (e.Key == Keys.D1)
            {
                _misc.Mode = 0;
                _misc.MaxSamples = 1;
            }
            if (e.Key == Keys.D2)
                _misc.Mode = 1;
            if (e.Key == Keys.D3)
                _misc.Mode = 2;
            if (e.Key == Keys.D4)
                _misc.MaxSamples = 128;
            _intersectionShader.Use();
            GL.Uniform1(_misc.ModeLocation, _misc.Mode);
            GL.Uniform1(_misc.MaxSamplesLocation, _misc.MaxSamples);

        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if(isCursorGrabbed)
                _camera.UpdateCameraRotation(e.DeltaX, e.DeltaY);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if(isCursorGrabbed)
                _camera.FOV -= e.OffsetY; 
        }


        void OnDrawGUI()
        {
            ImGui.ShowDemoWindow();

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Exit", "Esc"))
                    {
                        Close();
                    }

                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            if (ImGui.Begin("Objects"))
            {
                if (ImGui.TreeNode("Objects"))
                {
                    foreach(IHittable hittable in _list.GetHittables())
                    {
                        if (ImGui.TreeNode(hittable.GetOffset().ToString()))
                        {
                            Vector3 v = hittable.GetPostion();
                            System.Numerics.Vector3 k = new System.Numerics.Vector3(v.X, v.Y, v.Z);
                            if (ImGui.DragFloat3("Postion", ref k, 0.1f))
                                hittable.SetPostion(new Vector3(k.X, k.Y, k.Z));
                            ;
                            IMaterial mat = hittable.GetMaterial();
                            int type = (int)mat.GetMaterialType();
                            if (ImGui.SliderInt("Material", ref type, 0, 4, mat.GetMaterialType().ToString()))
                            {
                                switch (type)
                                {
                                    case ((int)MaterialType.Dielectric): mat = new Dielectric(mat.GetTexture()); break;
                                    case ((int)MaterialType.DiffuseLight): mat = new DiffuseLight(mat.GetTexture()); break;
                                    case ((int)MaterialType.Lambertian): mat = new Lambertian(mat.GetTexture()); break;
                                    case ((int)MaterialType.Metal): mat = new Metal(mat.GetTexture()); break;
                                    case ((int)MaterialType.Transparent): mat = new Transparent(mat.GetTexture()); break;
                                }
                                hittable.SetMaterial(mat);
                            }
                            if (type == (int)MaterialType.Dielectric || type == (int)MaterialType.Metal || type == (int)MaterialType.Transparent)
                            {
                                float param = mat.GetParam().Value;
                                if (ImGui.DragFloat("param", ref param, 0.005f))
                                    mat.SetParam(param);
                            }

                            ITexture tex = mat.GetTexture();
                            type = (int)tex.GetTextureType();
                            if (ImGui.SliderInt("Texture", ref type, 10, 11, tex.GetTextureType().ToString()))
                            {
                                switch (type)
                                {
                                    case ((int)TextureType.Solid): tex = new SolidColor(tex.GetAlbedo()[0]); break;
                                    case ((int)TextureType.Checker): tex = new CheckerPattern(tex.GetAlbedo()[0]); break;
                                }
                                mat.SetTexture(tex);
                            }
                            switch (type) {
                                case((int)TextureType.Solid):
                                {
                                    Vector3 col = tex.GetAlbedo()[0];
                                    System.Numerics.Vector3 c = new System.Numerics.Vector3(col.X, col.Y, col.Z);
                                    if (ImGui.ColorEdit3("Color", ref c))
                                        tex.SetAlbedo(new Vector3[] { new Vector3(c.X, c.Y, c.Z) });
                                    break;
                                }
                                case ((int)TextureType.Checker):
                                {
                                    Vector3 odd = tex.GetAlbedo()[0];
                                    Vector3 even = tex.GetAlbedo()[1];
                                    System.Numerics.Vector3 c1 = new System.Numerics.Vector3(odd.X, odd.Y, odd.Z);
                                    System.Numerics.Vector3 c2 = new System.Numerics.Vector3(even.X, even.Y, even.Z);
                                    if (ImGui.ColorEdit3("Odd Color", ref c1) | ImGui.ColorEdit3("Even Color", ref c2))
                                        tex.SetAlbedo(new Vector3[] { new Vector3(c1.X, c1.Y, c1.Z), new Vector3(c2.X, c2.Y, c2.Z) });
                                    break;
                                }
                            }
                            _list.ChangeHittable(hittable);
                            ImGui.TreePop();
                        }
                    }

                    ImGui.TreePop();
                }

                ImGui.End();
            }
        }

    }
}
