using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    class CV2Image : AbstractImage
    {
        private readonly bool usePalette;
        private Bitmap bitmap;

        public CV2Image(string filename)
        {
            using (FileStream f_in = File.OpenRead(filename))
            {
                byte[] header = new byte[1 + 4 + 4 + 4 + 4];
                f_in.Read(header, 0, header.Length);

                int width = BitConverter.ToInt32(header, 1);
                int height = BitConverter.ToInt32(header, 5);
                int stride = BitConverter.ToInt32(header, 9);

                this.usePalette = (header[0] == 8);

                if (header[0] == 8)
                {
                    this.bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    var data = this.bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    byte[] readBuffer = new byte[stride];
                    for (int i = 0; i < height; ++i)
                    {
                        f_in.Read(readBuffer, 0, stride);
                        Marshal.Copy(readBuffer, 0, data.Scan0 + data.Stride * i, width * 1);
                    }
                    this.bitmap.UnlockBits(data);
                }
                else if (header[0] == 16)
                {
                    this.bitmap = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                    var data = this.bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                    byte[] readBuffer = new byte[2 * stride];
                    for (int j = 0; j < height; ++j)
                    {
                        f_in.Read(readBuffer, 0, 2 * stride);
                        Marshal.Copy(readBuffer, 0, data.Scan0 + data.Stride * j, width * 2);
                    }
                    this.bitmap.UnlockBits(data);
                }
                else if (header[0] == 24 || header[0] == 32)
                {
                    this.bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    var data = this.bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    byte[] readBuffer = new byte[4 * stride];
                    for (int j = 0; j < height; ++j)
                    {
                        f_in.Read(readBuffer, 0, 4 * stride);
                        Marshal.Copy(readBuffer, 0, data.Scan0 + data.Stride * j, width * 4);
                    }
                    this.bitmap.UnlockBits(data);
                }
            }
        }

        public override Bitmap ToBitmap(Color[] pal, Rectangle rect)
        {
            var palette = bitmap.Palette;
            for (int i = 0; i < 256; ++i)
            {
                palette.Entries[i] = pal[i];
            }
            bitmap.Palette = palette;
            return bitmap.Clone(rect, PixelFormat.Format32bppArgb);
        }

        public override int Width
        {
            get { return bitmap.Width; }
        }

        public override int Height
        {
            get { return bitmap.Height; }
        }
    }
}
