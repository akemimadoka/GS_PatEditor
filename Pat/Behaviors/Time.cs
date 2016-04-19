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
    [SerializationBaseClassAttribute]
    public abstract class Time
    {
        public abstract void MakeEffects(ActionEffects dest, Effect effect);
    }

    [Serializable]
    [DisplayName("Start")]
    public class TimeStart : Time
    {
        public override void MakeEffects(ActionEffects dest, Effect effect)
        {
            dest.InitEffects.Add(effect);
        }
    }

    [Serializable]
    [DisplayName("EndOfSegment")]
    public class TimeEndSegment : Time
    {
        [XmlAttribute]
        public int Segment { get; set; }

        public override void MakeEffects(ActionEffects dest, Effect effect)
        {
            dest.SegmentFinishEffects.AddEffectToList(Segment, effect);
        }
    }

    [Serializable]
    [DisplayName("Repeat")]
    public class TimeRepeat : Time
    {
        public int Segment { get; set; }
        public int Interval { get; set; }

        public override void MakeEffects(ActionEffects dest, Effect effect)
        {
            var interval = new ConstValue { Value = Interval };
            dest.UpdateEffects.Add(new FilteredEffect()
            {
                Filter = new SimpleListFilter(
                    new Effects.AnimationSegmentFilter { Segment = Segment },
                    new Effects.AnimationCountModFilter { Divisor = interval, Mod = new ConstValue { Value = 0 } }
                ),
                Effect = effect,
            });
        }
    }

}
