using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Linq;

namespace N64ControllerSpy.Display
{
    // this isnt actually a 'game' but oh well
    class DisplayEngine : GameWindow
    {
        public bool IsRunning { get; private set; }

        Texture2D texture;
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
        float[] vertices = new float[]
        {
            -0.8f, -0.8f, 1.0f,
             0.0f,  0.8f, 1.0f,
             0.8f, -0.8f, 1.0f,
        };

        int vao; // vertex array obj
        int vbo; // vertix buffer obj 

        // Called first after RUn()
        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();
            // bind to gpu
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();

            // send to opengl idk
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.StaticDraw
            );

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);

            // FOR IMAGE
            GL.Enable(EnableCap.Blend);
            // GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.O);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);
            texture = Content.LoadTexture("C:\\Users\\tom\\Documents\\N64ControllerSpy\\N64ControllerSpy\\image\\controller.png");
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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.2f, 0.2f, 0.8f, 1.0f);

            //// render is done with vertex then render the backup then rinse and repeat
            //GL.BindVertexArray(vao);
            //GL.DrawArrays(BeginMode.Triangles, 0, vertices.Length);

            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Triangles);
            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(0, 0); GL.Vertex2(0, 1);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 0);
            GL.TexCoord2(0, 1); GL.Vertex2(0, 0);
            GL.TexCoord2(0, 0); GL.Vertex2(0, 1);
            GL.TexCoord2(1, 0); GL.Vertex2(1, 1);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 0);

            Context.SwapBuffers();
        }

        // called when engine destructs
        protected override void OnUnload()
        {
            base.OnUnload();
        }
  
    }
}
