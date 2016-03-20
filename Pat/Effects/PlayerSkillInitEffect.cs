using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects
{
    [Serializable]
    public class PlayerSkillInitPositionEffect : Effect, IHideFromEditor
    {
        [XmlAttribute]
        public bool IsInAir { get; set; }

        public override void Run(Simulation.Actor actor)
        {
            if (IsInAir)
            {
                actor.Y = -150.0f;
            }
            else
            {
                actor.Y = 0.0f;
            }
        }
    }

    [Serializable]
    public class PlayerSkillFallCancelEffect : PlayerSetLabelEffect, IHideFromEditor
    {
        public static readonly PlayerSkillFallCancelEffect Instance = new PlayerSkillFallCancelEffect();

        public PlayerSkillFallCancelEffect()
        {
            base.Label = Simulation.ActorLabelType.Fall;
            base.Effect = PlayerBeginFallEffect.Instance;
        }
    }

    [Serializable]
    public class PlayerSkillSitCancelEffect : PlayerSetLabelEffect, IHideFromEditor
    {
        public static readonly PlayerSkillSitCancelEffect Instance = new PlayerSkillSitCancelEffect();

        public PlayerSkillSitCancelEffect()
        {
            base.Label = Simulation.ActorLabelType.Sit;
            base.Effect = PlayerBeginStandEffect.Instance;
        }
    }

    [Serializable]
    public class PlayerAdjustDirectionEffect : Effect, IHideFromEditor
    {
        public static readonly PlayerAdjustDirectionEffect Instance = new PlayerAdjustDirectionEffect();

        public override void Run(Simulation.Actor actor)
        {
        }
    }


    [Serializable]
    public class PlayerSkillInitEffect : EffectListEffect
    {
        [XmlAttribute]
        public bool IsInAir { get; set; }

        [XmlAttribute]
        public bool AutoCancel { get; set; }

        private PlayerSkillInitPositionEffect _Position = new PlayerSkillInitPositionEffect();
        private PlayerClearLabelEffect _ClearLabel = PlayerClearLabelEffect.Instance;
        private InitCountEffect _InitCount = InitCountEffect.Instance;
        private SetMotionEffect _SetMotion = new SetMotionEffect();
        private PlayerSkillFallCancelEffect _FallCancel = PlayerSkillFallCancelEffect.Instance;
        private PlayerSkillSitCancelEffect _SitCancel = PlayerSkillSitCancelEffect.Instance;
        private PlayerAdjustDirectionEffect _AdjustDirection = PlayerAdjustDirectionEffect.Instance;

        protected override IEnumerable<Effect> Effects
        {
            get
            {
                yield return _Position;
                yield return _ClearLabel;
                yield return _InitCount;
                yield return _SetMotion;
                if (AutoCancel)
                {
                    if (IsInAir)
                    {
                        yield return _SitCancel;
                    }
                    else
                    {
                        yield return _FallCancel;
                    }
                }
                yield return _AdjustDirection;
                yield break;
            }
        }
    }
}
