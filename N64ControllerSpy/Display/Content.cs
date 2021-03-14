using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace N64ControllerSpy.Display
{
    class Content
    {
        public static Texture2D LoadTexture(string path)
        {
            Bitmap bitmap = new Bitmap(path);

            // create mem for a new texture
            int id = GL.GenTexture();

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            // let opengl know this is the active texture
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmap.Width,
                bitmap.Height,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                bitmapData.Scan0
            );

            bitmap.UnlockBits(bitmapData);

            // linear smoothing...just tells opengl to not make look stupid on bg
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            return new Texture2D(id, bitmap.Width, bitmap.Height);
        }
    }
}
