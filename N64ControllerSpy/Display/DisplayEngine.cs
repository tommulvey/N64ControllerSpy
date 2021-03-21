using N64ControllerSpy.Display.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

// TODO :::: https://stackoverflow.com/questions/19169452/opentk-texture-not-display-the-image

namespace N64ControllerSpy.Display
{
    // this isnt actually a 'game' but oh well
    class DisplayEngine : GameWindow
    {
        public bool IsRunning { get; private set; }

        public DisplayEngine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings): 
            base(gameWindowSettings,nativeWindowSettings)
        {
        }

        public bool Init()
        {
            IsRunning = false;
            return InitOpenGl();
        }

        public bool InitOpenGl()
        {
            GLFWBindingsContext binding = new GLFWBindingsContext();
            GL.LoadBindings(binding);

            if (GLFW.Init()) { 
                return true; 
            }

            Console.WriteLine("Error initalizing opengl and glfw");
            return false;
        }

        public void Loop()
        {
            if (!IsRunning)
            {
                IsRunning = true;

                base.Run();
            }
        }

        // vertices represent the 3 dimensions and 3 coords between -1 to 1...idk this will prob be removed later.
        float[] _vertices =
        {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;

        // Called first after RUn()
        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader = new Shader("Display/Shaders/shader.vert", "Display/Shaders/shader.frag");
            _shader.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify this from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the first vertex coordinate comes after the first vertex
            // and change the amount of data to 2 because there's only 2 floats for vertex coordinates
            int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile("image/controller.png");
            _texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            base.OnLoad();
        }

        // ovveride methods from gamewindow class, physics schtuff here. Run 2k times a sec.
        // youtube told me to do this
        // insert shrug here
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            WindowTime.Delta = float.Parse(args.Time.ToString());
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            SwapBuffers();
        }

        // called when engine destructs
        protected override void OnUnload()
        {
            base.OnUnload();
        }
  
    }
}
