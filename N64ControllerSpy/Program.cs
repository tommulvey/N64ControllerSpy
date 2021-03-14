using OpenTK.Windowing.Desktop;
using N64ControllerSpy.Controller;
using N64ControllerSpy.Display;

namespace N64ControllerSpy
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new N64Controller();
            // our first goal is to simply got our joystick inputs into the console..4Weird
            // controller.ReadJoyStickEvents();

            using (DisplayEngine game = new DisplayEngine(GameWindowSettings.Default, NativeWindowSettings.Default))
            {
                game.Init();
                game.Loop();
            }
        }
    }

}
