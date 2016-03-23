using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class Animation
    {
        //[XmlAttribute]
        //public string AnimationID;

        [XmlAttribute]
        public string ImageID;

        //[XmlAttribute]
        //public string SystemActionID;

        [XmlAttribute]
        public string ActionID;

        [XmlElement(ElementName = "Segment")]
        public List<AnimationSegment> Segments;
    }
}
