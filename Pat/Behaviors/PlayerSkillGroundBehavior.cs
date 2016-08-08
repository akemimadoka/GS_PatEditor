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
    public class PlayerSkillGroundBehavior : Behavior
    {
        [XmlAttribute]
        [DefaultValue(true)]
        public bool AutoCancel { get; set; }

        [XmlElement]
        [DefaultValue(true)]
        public bool ResetHit { get; set; }

        public PlayerSkillGroundBehavior()
        {
            AutoCancel = true;
            ResetHit = true;
        }

        public override void MakeEffects(ActionEffects effects)
        {
            effects.InitEffects.Add(new Effects.PlayerSkillInitEffect
            {
                AutoCancel = AutoCancel,
                IsInAir = false,
            });
            effects.UpdateEffects.Effects.Add(new Effects.IncreaseCountEffect());
            effects.SegmentFinishEffects.AddEffectToList(effects.SegmentCount - 1,
                new Effects.PlayerEndToFreeMoveEffect());
            if (ResetHit)
            {
                effects.InitEffects.Add(Effects.ResetHitEffect.Instance);
            }
        }
    }
}
