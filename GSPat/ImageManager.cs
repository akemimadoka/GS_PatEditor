using GS_PatEditor.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.GSPat
{
    class ImageManager : IDisposable
    {
        private readonly GSPatFile _File;
        private readonly string _Path;
        private readonly Color[] _Palette;

        private readonly Dictionary<Frame, Bitmap> _LoadedBitmap = new Dictionary<Frame, Bitmap>();

        public ImageManager(GSPatFile file, string path, Color[] palette)
        {
            _File = file;
            _Path = path;
            _Palette = palette;
        }

        public Bitmap GetBitmap(Frame frame)
        {
            Bitmap ret;
            if (_LoadedBitmap.TryGetValue(frame, out ret))
            {
                return ret;
            }
            
            ret = InternalLoadBitmap(frame);
            _LoadedBitmap.Add(frame, ret);
            return ret;
        }

        public void Reset()
        {
            foreach (var bitmap in _LoadedBitmap.Values)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
            _LoadedBitmap.Clear();
        }

        public void Switch()
        {
            if (_LoadedBitmap.Count > 40)
            {
                Reset();
            }
        }

        public void Dispose()
        {
            Reset();
        }

        private Bitmap InternalLoadBitmap(Frame frame)
        {
            var imgFile = Path.Combine(_Path, _File.Images[frame.SpriteID]);
            AbstractImage img = null;
            Bitmap ret;
            try
            {
                if (Path.GetExtension(imgFile) == ".dds")
                {
                    if (File.Exists(imgFile))
                    {
                        img = new DDSImage(imgFile);
                    }
                }
                else if (Path.GetExtension(imgFile) == ".bmp" ||
                    Path.GetExtension(imgFile) == ".cv2" ||
                    Path.GetExtension(imgFile) == ".png")
                {
                    var imgFileCV2 = Path.ChangeExtension(imgFile, ".cv2");
                    if (File.Exists(imgFileCV2))
                    {
                        img = new CV2Image(imgFileCV2);
                    }
                }
                if (img == null)
                {
                    return null;
                }
                var rect = new Rectangle(frame.ViewOffsetX, frame.ViewOffsetY,
                    frame.ViewWidth, frame.ViewHeight);
                ret = img.ToBitmap(_Palette, rect);
            }
            finally
            {
                if (img != null)
                {
                    try
                    {
                        img.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
            if (frame.ImageManipulation != null && frame.ImageManipulation.AlphaBlend != 0)
            {
                AbstractImage.MakeAlphaBlendBitmap(ret);
            }
            return ret;
        }
    }
}
