using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Editing
{
    public class ProjectImageFileList
    {
        private readonly Project _Project;

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
            //TODO cache loaded resource image (containing palette)
            var res = _Project.FindResource(ProjectDirectoryUsage.Image, image.Resource.ResourceID);
            if (res == null)
            {
                image.LoadedImage = new LoadedFrameImage { Bitmap = null };
            }
            else
            {
                image.LoadedImage = new LoadedFrameImage { Bitmap = null };
            }
        }
    }
}
