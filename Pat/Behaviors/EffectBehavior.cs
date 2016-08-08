using GS_PatEditor.Editor.Editable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Behaviors
{
    [Serializable]
    [DisplayName("ManualBehavior")]
    public class EffectBehavior : Behavior
    {
        [XmlElement]
        [EditorChildNode("Time")]
        public Time Time;

        [XmlElement]
        [EditorChildNode("Effect")]
        public Effect Effect;

        public override void MakeEffects(ActionEffects effects)
        {
            Time.MakeEffects(effects, Effect);
        }
    }
}
