using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters
{
    class ImageListExporter
    {
        private Dictionary<string, int> _ImageToFile = new Dictionary<string, int>();
        private Dictionary<string, Pat.FrameImage> _ImageIDToObj = new Dictionary<string, Pat.FrameImage>();

        public void AddImage(GSPat.GSPatFile file, Pat.FrameImage image)
        {
            _ImageToFile.Add(image.ImageID, file.Images.Count);
            file.Images.Add(image.Resource.ResourceID);
            _ImageIDToObj.Add(image.ImageID, image);
        }

        public int GetImageIntID(string id)
        {
            return _ImageToFile[id];
        }

        public Pat.FrameImage GetImage(string id)
        {
            return _ImageIDToObj[id];
        }
    }
}
