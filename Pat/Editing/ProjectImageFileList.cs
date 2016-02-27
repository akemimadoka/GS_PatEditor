using GS_PatEditor.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Editing
{
    public class ProjectImageFileList
    {
        private readonly Project _Project;

        private int _SelectedPalette;
        public int SelectedPalette
        {
            get
            {
                if (_Project.Settings.Palettes.Count == 0)
                {
                    _SelectedPalette = -1;
                }
                else if (_SelectedPalette >= _Project.Settings.Palettes.Count)
                {
                    _SelectedPalette = 0;
                }
                return _SelectedPalette;
            }
            set
            {
                if (_Project.Settings.Palettes.Count == 0)
                {
                    _SelectedPalette = -1;
                }
                else if (value >= _Project.Settings.Palettes.Count)
                {
                    _SelectedPalette = 0;
                }
                else
                {
                    _SelectedPalette = value;
                    OnPaletteChange();
                }
            }
        }

        private Color[] _Palette;

        public ProjectImageFileList(Project proj)
        {
            _Project = proj;
        }

        public void Refresh()
        {

        }

        public Bitmap GetImage(string id)
        {
            var imgDesc = _Project.Images.FirstOrDefault(f => f.ImageID == id);
            if (imgDesc == null)
            {
                return null;
            }
            if (imgDesc.LoadedImage == null)
            {
                LoadImage(imgDesc);
            }
            return imgDesc.LoadedImage.Bitmap;
        }

        private void LoadImage(FrameImage image)
        {
            //TODO cache loaded resource image (containing palette: clear cache when changing palette)
            var res = _Project.FindResource(ProjectDirectoryUsage.Image, image.Resource.ResourceID);
            if (res != null)
            {
                if (Path.GetExtension(res) == ".dds")
                {
                    image.LoadedImage = new LoadedFrameImage { Bitmap = ClipBitmap(new DDSImage(res), image) };
                    return;
                }
                else if (Path.GetExtension(res) == ".cv2")
                {
                    image.LoadedImage = new LoadedFrameImage { Bitmap = ClipBitmap(new CV2Image(res), image) };
                    return;
                }
            }
            image.LoadedImage = new LoadedFrameImage { Bitmap = null };
        }

        private Bitmap ClipBitmap(AbstractImage res, FrameImage img)
        {
            return res.ToBitmap(_Palette, new Rectangle(img.X, img.Y, img.W, img.H));
        }

        private void OnPaletteChange()
        {
            if (SelectedPalette == -1)
            {
                _Palette = null;
                return;
            }
            var palName = _Project.FindResource(ProjectDirectoryUsage.Image, _Project.Settings.Palettes[SelectedPalette]);
            if (palName == null)
            {
                _Palette = null;
                return;
            }
            _Palette = CV2Palette.ReadPaletteFile(palName);
        }
    }
}
