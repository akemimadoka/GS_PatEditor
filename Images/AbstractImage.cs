using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    abstract class AbstractImage : IDisposable
    {
        public abstract Bitmap ToBitmap(Color[] pal, Rectangle rect);
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract bool UsePalette { get; }

        public abstract void Dispose();

        public static int GetValidDimension(int size)
        {
            int ret = 16;
            while (ret < size)
            {
                ret *= 2;
            }
            return ret;
        }

        public static void MakeAlphaBlendBitmap(Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Width; ++i)
            {
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    var c = bitmap.GetPixel(i, j);
                    var cc = Color.FromArgb((int)((c.R + c.G + c.B) / 3.0f), c);
                    bitmap.SetPixel(i, j, cc);
                }
            }
        }

        public static void AdjustRectangle(ref Rectangle rect, Bitmap bitmap)
        {
            if (rect.Left < 0)
            {
                rect.X = 0;
            }
            if (rect.Top < 0)
            {
                rect.Y = 0;
            }
            if (rect.Right > bitmap.Width)
            {
                rect.Width = bitmap.Width - rect.X;
            }
            if (rect.Bottom > bitmap.Height)
            {
                rect.Height = bitmap.Height - rect.Y;
            }
        }
    }
}
