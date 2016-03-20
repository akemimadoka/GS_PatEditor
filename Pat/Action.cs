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

        [XmlArray]
        public EffectList InitEffects;
        [XmlArray]
        public EffectList UpdateEffects;
        [XmlArray]
        //[XmlIgnore]
        public List<EffectList> KeyFrameEffects;
    }
}
