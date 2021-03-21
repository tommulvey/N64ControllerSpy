using OpenTK.Windowing.Desktop;
using N64ControllerSpy.Controller;
using N64ControllerSpy.Display;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace N64ControllerSpy
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new N64Controller();
            // our first goal is to simply got our joystick inputs into the console..4Weird
            // controller.ReadJoyStickEvents();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(600, 600),
                Title = "n64 controller spy",
                WindowBorder = WindowBorder.Fixed
            };

            using (DisplayEngine game = new DisplayEngine(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Init();
                game.Loop();
            }
        }
    }

}
