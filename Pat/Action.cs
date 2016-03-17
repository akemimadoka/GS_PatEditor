using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class EffectList : IEnumerable<Effect>
    {
        [XmlElement(ElementName = "Effect")]
        public List<Effect> Effects = new List<Effect>();

        public IEnumerator<Effect> GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        public void Add(Effect effect)
        {
            Effects.Add(effect);
        }
    }

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
