using System;
using System.Collections.Generic;
using Dull.GUI;
using Dull.Lights;
using Dull.Materials;
using Dull.Objects;
using Dull.ObjectTexture;
using Dull.Scenes;
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
        private ImGuiController _controller;

        private Shader _shader;
        private ComputeShader _intersectionShader;

        private Texture _renderQuad;

        private BaseScene _scene;

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
        private bool _isObjectWindowOpened = false;
        private bool _isLightsWindowOpened = false;

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

            _scene = new RoomScene((float)Size.X / Size.Y);
            _scene.Camera.UpdateParamLocations(_intersectionShader.Handle);

            _misc = new MiscUniforms(0, 1, 16);
            _misc.UpdateParamLocations(_intersectionShader.Handle);
            GL.Uniform1(_misc.ModeLocation, _misc.Mode);
            GL.Uniform1(_misc.MaxSamplesLocation, _misc.MaxSamples);
            GL.Uniform1(_misc.MaxDepthLocation, _misc.MaxDepth);
    

            _scene.HitList.DataToBuffer(_intersectionShader.Handle);
            _scene.LightList.DataTobuffer(_intersectionShader.Handle);


            _renderQuad = new Texture(Size.X, Size.Y, PixelInternalFormat.Rgb32f, PixelFormat.Rgba, (IntPtr)0,4);

            _controller = new ImGuiController(Size.X, Size.Y);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            //_controller.Update(this, (float)args.Time);//new
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _controller.Update(this, (float)e.Time);

            if (RenderTime < 0.6)
                Title = (int)(1 / RenderTime) + " FPS";
            else
                Title = Math.Round(RenderTime, 3).ToString() + " Seconds per Frame";
            SetNewCameraPosition();

            _intersectionShader.Use();
            GL.Uniform1(_scene.Camera.FovLocation, _scene.Camera.FOV);
            GL.Uniform3(_scene.Camera.LookFromLocation, _scene.Camera.LookFrom);
            GL.Uniform3(_scene.Camera.LookAtLocation, _scene.Camera.LookAt);


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
            GL.Uniform1(_scene.Camera.ScreenWidthLocation, e.Width);
            GL.Uniform1(_scene.Camera.ScreenHeightLocation, e.Height);
            _scene.Camera.AspectRatio = (float)e.Width / e.Height;
            GL.Uniform1(_scene.Camera.AspectRatioLocation, _scene.Camera.AspectRatio);
            _shader.Use();

            _renderQuad.UpdateTextureParams(e.Width, e.Height, (IntPtr)0);

            _controller.WindowResized(e.Width, e.Height);
            base.OnResize(e);
            
        }
        private void SetNewCameraPosition()
        {
            if(KeyboardState.IsAnyKeyDown || KeyboardState.WasKeyDown(Keys.C))
                _scene.Camera.UpdateCameraPosition(KeyboardState, (float)RenderTime);
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
            _intersectionShader.Use();
            GL.Uniform1(_misc.ModeLocation, _misc.Mode);
            GL.Uniform1(_misc.MaxSamplesLocation, _misc.MaxSamples);

        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if(isCursorGrabbed)
                _scene.Camera.UpdateCameraRotation(e.DeltaX, e.DeltaY);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if(isCursorGrabbed)
                _scene.Camera.FOV -= e.OffsetY; 
        }


        void OnDrawGUI()
        {

            //ImGui.ShowDemoWindow();

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
                if (ImGui.BeginMenu("Windows"))
                {
                    ImGui.Checkbox("Objects", ref _isObjectWindowOpened);
                    ImGui.Checkbox("Lights", ref _isLightsWindowOpened);

                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Scene"))
                {
                    if (ImGui.MenuItem("Grass Scene"))
                    {
                        _intersectionShader.Use();
                        _scene = new GrassScene((float)Size.X / Size.Y);
                        _scene.UpdateData(_intersectionShader.Handle);
                    }
                    if (ImGui.MenuItem("Room Scene"))
                    {
                        _intersectionShader.Use();
                        _scene = new RoomScene((float)Size.X / Size.Y);
                        _scene.UpdateData(_intersectionShader.Handle);
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Render Settings"))
                {
                    if (ImGui.BeginMenu("Render Mode"))
                    {
                        if (ImGui.MenuItem("Full Render","1"))
                        {
                            _misc.Mode = 0;
                        }
                        if (ImGui.MenuItem("Depth Map","2"))
                        {
                            _misc.Mode = 1;
                        }
                        if (ImGui.MenuItem("Normal Map", "3"))
                        {
                            _misc.Mode = 2;
                        }
                        _intersectionShader.Use();
                        _misc.UpdateParams();
                        ImGui.EndMenu();
                        
                    }
                    int maxSamples= _misc.MaxSamples;
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text("Max Samples"); ImGui.SameLine();
                    ImGui.PushItemWidth(100);
                    if (ImGui.InputInt("", ref maxSamples, 1, 128))
                    {
                        _misc.MaxSamples = maxSamples;
                        _intersectionShader.Use();
                        _misc.UpdateParams();
                    }
                    int maxDepth = _misc.MaxDepth;
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text("Max Depth"); ImGui.SameLine();
                    ImGui.PushItemWidth(100);
                    if (ImGui.InputInt(" ", ref maxDepth,1,1))
                    {
                        _misc.MaxDepth = maxDepth;
                        _intersectionShader.Use();
                        _misc.UpdateParams();
                    }

                    bool isBackground = _misc.IsBackground;
                    if(ImGui.Checkbox("Background", ref isBackground))
                    {
                        _misc.IsBackground = isBackground;
                        _intersectionShader.Use();
                        _misc.UpdateParams();
                    }
                    ImGui.Text($"Camera FOV:{_scene.Camera.FOV}");
                    ImGui.Text($"Camera Postion:{_scene.Camera.LookFrom}");
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            if (_isObjectWindowOpened && ImGui.Begin("Objects",ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoNavInputs |ImGuiWindowFlags.NoFocusOnAppearing))
            {
                foreach (IHittable hittable in _scene.HitList.GetHittables())
                {
                    if (ImGui.TreeNode(hittable.GetName() + " " + hittable.GetOffset().ToString()))
                    {
                        Vector3 v = hittable.GetPostion();
                        System.Numerics.Vector3 objPos = new System.Numerics.Vector3(v.X, v.Y, v.Z);
                        if (ImGui.DragFloat3("Position", ref objPos, 0.1f))
                            hittable.SetPostion(new Vector3(objPos.X, objPos.Y, objPos.Z));

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
                        switch (type)
                        {
                            case ((int)TextureType.Solid):
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
                        _scene.HitList.ChangeHittable(hittable);
                        ImGui.TreePop();
                    }
                }

                ImGui.End();
            }

            if (_isLightsWindowOpened && ImGui.Begin("Lights", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoNavInputs | ImGuiWindowFlags.NoFocusOnAppearing))
            {
                foreach (ILight light in _scene.LightList.GetLights())
                {
                    if (ImGui.TreeNode(light.GetName()+ " " + light.GetOffset().ToString()))
                    {
                        Vector3 v = light.GetPostion();
                        System.Numerics.Vector3 lightPos = new System.Numerics.Vector3(v.X, v.Y, v.Z);
                        if (ImGui.DragFloat3("Position", ref lightPos, 0.1f))
                            light.SetPostion(new Vector3(lightPos.X, lightPos.Y, lightPos.Z));

                        Vector3 col = light.GetColor();
                        System.Numerics.Vector3 c = new System.Numerics.Vector3(col.X, col.Y, col.Z);
                        if (ImGui.ColorEdit3("Color", ref c))
                            light.SetColor(new Vector3(c.X, c.Y, c.Z));

                        int intens = light.GetIntensity();
                        if (ImGui.SliderInt("Intensity", ref intens, 0, 50))
                            light.SetIntensity(intens);

                        _scene.LightList.ChangeLight(light);
                        ImGui.TreePop();
                    }
                }
                ImGui.End();
            }
        }

    }
}
