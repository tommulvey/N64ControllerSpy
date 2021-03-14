using System;
using SharpDX.DirectInput;

namespace N64ControllerSpy.Controller
{
    public class N64Controller
    {
        public N64Controller()
        {
            // controlers use direct input.
            _di = new DirectInput();

            foreach (var deviceInstance in _di.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                _deviceGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (_deviceGuid == Guid.Empty)
            {
                // Console.WriteLine("UNABLE TO FIND GAMEPAD--SEARCHING FOR JOYSTICK");
                foreach (var deviceInstance in _di.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    _deviceGuid = deviceInstance.InstanceGuid;
            }

            // If Joystick not found, throws an error
            if (_deviceGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            _joystick = new Joystick(_di, _deviceGuid);
            _joystickState = new JoystickState();
        }

        private DirectInput _di;
        private Guid _deviceGuid = Guid.Empty;
        private Joystick _joystick;
        private JoystickState _joystickState;

        public void ReadJoyStickEvents()
        {
            // Query all suported ForceFeedback effects
            var allEffects = _joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            _joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            _joystick.Acquire();

            // Poll events from joystick
            while (true)
            {
                _joystick.Poll();
                _joystickState = _joystick.GetCurrentState(); // yaY this works!

                var datas = _joystick.GetBufferedData();
                foreach (var state in datas)
                {
                    Console.WriteLine(state);
                    // Console.WriteLine(_joystickState);
                }
            }
        }
    }
}
