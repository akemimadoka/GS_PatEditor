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
        //TODO cache LoadedFrameImage

        [XmlElement]
        public int ScaleX;
        public bool ShouldSerializeScaleX() { return ScaleX != 100; }

        [XmlElement]
        public int ScaleY;
        public bool ShouldSerializeScaleY() { return ScaleY != 100; }

        [XmlElement]
        public int Rotation;
        public bool ShouldSerializeRotation() { return Rotation != 0; }

        [XmlElement]
        public int OriginX;
        [XmlElement]
        public int OriginY;

        [XmlElement]
        public int Duration;

        [XmlArray]
        public List<FramePoint> Points;

        [XmlElement]
        public PhysicalBox PhysicalBox;

        [XmlArray]
        public List<Box> HitBoxes;

        [XmlArray]
        public List<Box> AttackBoxes;
    }
}
