using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    class DDSImage : AbstractImage
    {
        private Bitmap _Bitmap;

        public DDSImage(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                using (var reader = new BinaryReader(file))
                {
                    _Bitmap = DDSLoader.LoadDDS(reader);
                }
            }
        }

        public override Bitmap ToBitmap(Color[] pal, Rectangle rect)
        {
            return _Bitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public override int Width
        {
            get { return _Bitmap.Width; }
        }

        public override int Height
        {
            get { return _Bitmap.Height; }
        }

        public override void Dispose()
        {
            _Bitmap.Dispose();
        }
    }
}
