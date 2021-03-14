using System;

using N64ControllerSpy.Controller;

namespace N64ControllerSpy
{
    class Program
    {
        static void Main(string[] args)
        {
            var contrallah = new N64Controller();
            // our first goal is to simply got our joystick inputs into the console..4Weird
            contrallah.ReadJoyStickEvents();
        }
    }

}
