using System;
using OpenTK.Windowing.Common;
namespace Dull
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Screen scrn = new Screen(1000, 1000))
            {
                scrn.CursorState = CursorState.Grabbed;
                scrn.VSync = VSyncMode.On;
                //scrn.RenderFrequency = 30.0f;
                scrn.Run();
            }
        }
    }
}
