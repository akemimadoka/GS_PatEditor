using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class Action
    {
        [XmlAttribute]
        public string ActionID;

        [XmlAttribute]
        public string ImageID;

        [XmlArray]
        public List<AnimationSegment> Segments;

        [XmlArray]
        public EffectList InitEffects;

        [XmlArray]
        public EffectList UpdateEffects;

        [XmlArray]
        public List<EffectList> KeyFrameEffects;
    }
}
