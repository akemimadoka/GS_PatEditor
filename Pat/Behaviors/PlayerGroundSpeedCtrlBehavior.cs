using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Behaviors
{
    [Serializable]
    [SerializationBaseClassAttribute]
    public abstract class PlayerGroundSpeedCtrlBehaviorEntry
    {
        [XmlElement]
        public SegmentSelector Segments { get; set; }
        public bool ShouldSerializeSegments()
        {
            return !(Segments != null && Segments.Index == "*");
        }

        [XmlIgnore]
        [Browsable(false)]
        public abstract Effect Effect { get; }

        public PlayerGroundSpeedCtrlBehaviorEntry()
        {
            Segments = new SegmentSelector { Index = "*" };
        }
    }

    [Serializable]
    [DisplayName("Friction")]
    public class PlayerGroundSpeedCtrlBehaviorEntryFriction : PlayerGroundSpeedCtrlBehaviorEntry
    {
        [XmlAttribute]
        [DefaultValue(0.2f)]
        public float Value { get; set; }

        public PlayerGroundSpeedCtrlBehaviorEntryFriction()
        {
            Value = 0.2f;
        }

        public override Effect Effect
        {
            get
            {
                return new Effects.PlayerSkillStopMovingEffect { ReduceSpeed = Value };
            }
        }
    }

    [Serializable]
    public class PlayerGroundSpeedCtrlBehavior : Behavior
    {
        [XmlAttribute]
        [DefaultValue(true)]
        public bool ReduceInitialSpeed { get; set; }

        [XmlArray]
        [EditorChildNode(null)]
        public List<PlayerGroundSpeedCtrlBehaviorEntry> Entries = new List<PlayerGroundSpeedCtrlBehaviorEntry>();

        public PlayerGroundSpeedCtrlBehavior()
        {
            ReduceInitialSpeed = true;
        }

        public override void MakeEffects(ActionEffects effects)
        {
        }
    }
}
