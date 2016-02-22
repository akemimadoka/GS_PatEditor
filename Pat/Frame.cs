using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class FramePoint
    {
        [XmlAttribute]
        public int X;
        [XmlAttribute]
        public int Y;
    }

    [Serializable]
    public class PhysicalBox
    {
        [XmlAttribute]
        public int X;
        [XmlAttribute]
        public int Y;
        [XmlAttribute]
        public int W;
        [XmlAttribute]
        public int H;
    }

    [Serializable]
    public class Box
    {
        [XmlAttribute]
        public int X;
        [XmlAttribute]
        public int Y;
        [XmlAttribute]
        public int W;
        [XmlAttribute]
        public int H;
        [XmlAttribute]
        public int R;
    }

    [Serializable]
    public class Frame
    {
        [XmlElement]
        public string ImageID;
        [XmlElement]
        public int ScaleX;
        [XmlElement]
        public int ScaleY;
        [XmlElement]
        public int Rotate;

        [XmlElement]
        public int OriginX;
        [XmlElement]
        public int OriginY;

        [XmlElement]
        public int Duration;

        [XmlElement]
        public List<FramePoint> Points;

        [XmlElement]
        public PhysicalBox PhysicalBox;

        [XmlArray]
        public List<Box> HitBoxes;

        [XmlArray]
        public List<Box> AttackBoxes;
    }
}
