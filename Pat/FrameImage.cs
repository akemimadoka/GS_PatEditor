using GS_PatEditor.Pat.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class FrameImage
    {
        [XmlAttribute]
        public string ImageID;

        [XmlElement]
        public ResourceReference Resource;

        [XmlElement]
        public bool AlphaBlendMode;
        public bool ShouldSerializeAlphaBlendMode() { return AlphaBlendMode == true; }

        [XmlElement]
        public int X;
        public bool ShouldSerializeX() { return X != 0; }
        
        [XmlElement]
        public int Y;
        public bool ShouldSerializeY() { return Y != 0; }

        [XmlElement]
        public int W;
        public bool ShouldSerializeW() { return W != -1; }

        [XmlElement]
        public int H;
        public bool ShouldSerializeH() { return H != -1; }
    }
}
