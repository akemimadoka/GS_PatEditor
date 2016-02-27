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
    }
}
