using OpenTK.Graphics.OpenGL;
namespace Dull
{
    class MiscUniforms
    {
        private int _mode;
        private int _maxSamples;
        private int _maxDepth;
        private int _isBackground;

        private int _seedLocation;
        private int _modeLocation;
        private int _maxSamplesLocation;
        private int _maxDepthLocation;
        private int _isBackgroundLocation;
       

        public MiscUniforms(int mode,int maxSamples, int maxDepth)
        {
            _mode = mode;
            _maxSamples = maxSamples;
            _maxDepth = maxDepth;
        }
        public void UpdateParamLocations(int shaderHandle)
        {
            _seedLocation = GL.GetUniformLocation(shaderHandle, "seed");
            _modeLocation = GL.GetUniformLocation(shaderHandle, "mode");
            _maxSamplesLocation = GL.GetUniformLocation(shaderHandle, "max_samples");
            _maxDepthLocation = GL.GetUniformLocation(shaderHandle, "max_depth");
            _isBackgroundLocation = GL.GetUniformLocation(shaderHandle, "is_background");
        }
        public void UpdateParams()
        {
            GL.Uniform1(_modeLocation, _mode);
            GL.Uniform1(_maxSamplesLocation, _maxSamples);
            GL.Uniform1(_maxDepthLocation, _maxDepth);
            GL.Uniform1(_isBackgroundLocation, _isBackground);
        }
        public int SeedLocation { get => _seedLocation; }
        public int ModeLocation { get => _modeLocation; }
        public int MaxSamplesLocation { get => _maxSamplesLocation; }
        public int MaxDepthLocation { get => _maxDepthLocation; }

        public int Mode { get => _mode; set => _mode = value; }
        public int MaxSamples { get => _maxSamples; set => _maxSamples = value; }
        public int MaxDepth { get => _maxDepth; set => _maxDepth = value; }
        public bool IsBackground { get => _isBackground==1; set => _isBackground = value==true?1:0; }
    }
}
