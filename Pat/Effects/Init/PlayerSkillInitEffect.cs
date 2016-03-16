using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class PlayerSkillInitPositionEffect : Effect
    {
        [XmlAttribute]
        public bool IsInAir;

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
    public class PlayerSkillInitCountEffect : Effect
    {
        public static readonly PlayerSkillInitCountEffect Instance = new PlayerSkillInitCountEffect();

        public override void Run(Simulation.Actor actor)
        {
            actor.ActionCount = 0;
        }
    }

    [Serializable]
    public class PlayerSkillSetMotionEffect : Effect
    {
        [XmlAttribute]
        public string Animation;

        [XmlAttribute]
        public int Segment;

        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(Animation, Segment);
        }
    }

    [Serializable]
    public class PlayerSkillFallCancelEffect : PlayerSetLabelEffect
    {
        public static readonly PlayerSkillFallCancelEffect Instance = new PlayerSkillFallCancelEffect();

        public PlayerSkillFallCancelEffect()
        {
            base.Label = Simulation.ActorLabelType.Fall;
            base.Effect = PlayerBeginFallEffect.Instance;
        }
    }

    [Serializable]
    public class PlayerSkillSitCancelEffect : PlayerSetLabelEffect
    {
        public static readonly PlayerSkillSitCancelEffect Instance = new PlayerSkillSitCancelEffect();

        public PlayerSkillSitCancelEffect()
        {
            base.Label = Simulation.ActorLabelType.Sit;
            base.Effect = PlayerBeginStandEffect.Instance;
        }
    }

    [Serializable]
    public class PlayerAdjustDirectionEffect : Effect
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
        public bool IsInAir;

        [XmlAttribute]
        public bool AutoCancel;

        private PlayerSkillInitPositionEffect _Position = new PlayerSkillInitPositionEffect();
        private PlayerClearLabelEffect _ClearLabel = PlayerClearLabelEffect.Instance;
        private PlayerSkillInitCountEffect _InitCount = PlayerSkillInitCountEffect.Instance;
        private PlayerSkillSetMotionEffect _SetMotion = new PlayerSkillSetMotionEffect();
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
