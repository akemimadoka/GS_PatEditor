using System;
using System.Collections.Generic;
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
        public bool AutoCancel { get; set; }
        public bool ShouldSerializeAutoCancel() { return !AutoCancel; }

        public PlayerSkillGroundBehavior()
        {
            AutoCancel = true;
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
        }
    }
}
