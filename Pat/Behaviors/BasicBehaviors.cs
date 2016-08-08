using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Behaviors
{
    public enum EffectBehaviorPriority
    {
        First,
        Last,
    }

    [Serializable]
    public class InitEffectBehavior : Behavior, IHideFromEditor
    {
        [XmlElement]
        [EditorChildNode("Effect")]
        public Effect Effect;

        [XmlAttribute]
        public EffectBehaviorPriority Priority;

        public override void MakeEffects(ActionEffects effects)
        {
            if (Priority == EffectBehaviorPriority.First)
            {
                effects.InitEffects.Insert(0, Effect);
            }
            else if (Priority == EffectBehaviorPriority.Last)
            {
                effects.InitEffects.Add(Effect);
            }
        }
    }

    [Serializable]
    public class UpdateEffectBehavior : Behavior, IHideFromEditor
    {
        [XmlElement]
        [EditorChildNode("Effect")]
        public Effect Effect;

        [XmlElement]
        public SegmentSelector Segments { get; set; }

        [XmlAttribute]
        public EffectBehaviorPriority Priority;

        public UpdateEffectBehavior()
        {
            Segments = new SegmentSelector { Index = "*" };
        }

        public override void MakeEffects(ActionEffects effects)
        {
            var effectFiltered = new SimpleListEffect();
            effectFiltered.EffectList.Effects.AddRange(Segments.IndexList.Select(x =>
                (Effect)new FilteredEffect
                {
                    Filter = new Effects.AnimationSegmentFilter { Segment = x },
                    Effect = Effect,
                }));
            if (Priority == EffectBehaviorPriority.First)
            {
                effects.UpdateEffects.Insert(0, effectFiltered);
            }
            else if (Priority == EffectBehaviorPriority.Last)
            {
                effects.InitEffects.Add(effectFiltered);
            }
        }
    }

    [Serializable]
    public class EndSegmentEffectBehavior : Behavior, IHideFromEditor
    {
        [XmlElement]
        [EditorChildNode("Effect")]
        public Effect Effect;

        [XmlElement]
        public SegmentSelector Segments { get; set; }

        [XmlAttribute]
        public EffectBehaviorPriority Priority;

        public EndSegmentEffectBehavior()
        {
            Segments = new SegmentSelector { Index = "0" };
        }

        public override void MakeEffects(ActionEffects effects)
        {
            foreach (var i in Segments.IndexList)
            {
                if (Priority == EffectBehaviorPriority.First)
                {
                    effects.SegmentFinishEffects.InsertEffectToList(i, Effect);
                }
                else if (Priority == EffectBehaviorPriority.Last)
                {
                    effects.SegmentFinishEffects.AddEffectToList(i, Effect);
                }
            }
        }
    }

    [Serializable]
    public class StartSegmentEffectBehavior : Behavior, IHideFromEditor
    {
        [XmlElement]
        [EditorChildNode("Effect")]
        public Effect Effect;

        [XmlElement]
        public SegmentSelector Segments { get; set; }

        [XmlAttribute]
        public EffectBehaviorPriority Priority;

        public StartSegmentEffectBehavior()
        {
            Segments = new SegmentSelector { Index = "0" };
        }

        public override void MakeEffects(ActionEffects effects)
        {
            foreach (var i in Segments.IndexList)
            {
                if (Priority == EffectBehaviorPriority.First)
                {
                    effects.SegmentStartEffects.InsertEffectToList(i, Effect);
                }
                else if (Priority == EffectBehaviorPriority.Last)
                {
                    effects.SegmentStartEffects.AddEffectToList(i, Effect);
                }
            }
        }
    }
}
