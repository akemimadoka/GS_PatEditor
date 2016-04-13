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
        public bool ShouldSerializeImageID() { return ImageID != null; }
        //TODO cache LoadedFrameImage

        [XmlElement]
        public int ScaleX = 100;
        public bool ShouldSerializeScaleX() { return ScaleX != 100; }

        [XmlElement]
        public int ScaleY = 100;
        public bool ShouldSerializeScaleY() { return ScaleY != 100; }

        [XmlElement]
        public int Rotation;
        public bool ShouldSerializeRotation() { return Rotation != 0; }

        [XmlElement]
        public float Alpha = 1.0f;
        public bool ShouldSerializeAlpha() { return Alpha != 1.0f; }

        [XmlElement]
        public float Red = 1.0f;
        public bool ShouldSerializeRed() { return Red != 1.0f; }

        [XmlElement]
        public float Green = 1.0f;
        public bool ShouldSerializeGreen() { return Green != 1.0f; }

        [XmlElement]
        public float Blue = 1.0f;
        public bool ShouldSerializeBlue() { return Blue != 1.0f; }

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

        public static readonly Frame EmptyFrame = new Frame
        {
            AttackBoxes = new List<Box>(),
            Duration = 1,
            HitBoxes = new List<Box>(),
            ImageID = "",
            Points = new List<FramePoint>(),
            ScaleX = 1,
            ScaleY = 1,
        };
    }
}
