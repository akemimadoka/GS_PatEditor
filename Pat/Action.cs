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
        public List<Effect> InitEffects;
        [XmlArray]
        public List<Effect> UpdateEffects;
        [XmlArray]
        public List<Effect>[] KeyFrameEffects;
    }
}
