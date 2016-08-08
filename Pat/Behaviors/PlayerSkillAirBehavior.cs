﻿using GS_PatEditor.Pat.Effects;
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
    public class PlayerSkillAirBehavior : Behavior
    {
        [XmlAttribute]
        [DefaultValue(true)]
        public bool AutoCancel { get; set; }

        [XmlElement]
        public int? SitCancelSegment { get; set; }

        [XmlElement]
        [DefaultValue(true)]
        public bool ResetHit { get; set; }

        public PlayerSkillAirBehavior()
        {
            AutoCancel = true;
            ResetHit = true;
        }

        public override void MakeEffects(ActionEffects effects)
        {
            effects.InitEffects.Add(new Effects.PlayerSkillInitEffect
            {
                AutoCancel = AutoCancel,
                IsInAir = true,
            });
            effects.UpdateEffects.Effects.Add(new IncreaseCountEffect());
            effects.SegmentFinishEffects.AddEffectToList(effects.SegmentCount - 1,
                new PlayerEndToFreeMoveEffect());
            if (SitCancelSegment.HasValue)
            {
                effects.SegmentStartEffects.AddEffectToList(SitCancelSegment.Value,
                    new SetLabelEffect
                    {
                        Label = Simulation.ActorLabelType.Sit,
                        Effect = PlayerBeginSitEffect.Instance,
                    });
            }
            if (ResetHit)
            {
                effects.InitEffects.Add(ResetHitEffect.Instance);
            }
        }
    }
}
