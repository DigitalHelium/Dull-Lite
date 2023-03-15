using System;
using OpenTK.Windowing.Common;
namespace Dull
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Screen scrn = new Screen(1600, 1200))
            {
                scrn.CursorState = CursorState.Grabbed;
                scrn.Run();
            }
        }
    }
}
