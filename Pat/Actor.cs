using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    public enum ActorType
    {
        Bullet,
    }

    [Serializable]
    public class Actor_
    {
        [XmlAttribute]
        public string ActorID;
        [XmlAttribute]
        public string Animation;
        [XmlAttribute]
        public ActorType Type;
    }
}
