using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects
{
    [Serializable]
    public class PlayerSetLabelEffect : Effect, IHideFromEditor
    {
        [XmlAttribute]
        public Simulation.ActorLabelType Label { get; set; }

        [XmlElement]
        public Effect Effect;

        public override void Run(Simulation.Actor actor)
        {
            switch (Label)
            {
                case Simulation.ActorLabelType.Fall:
                    actor.FallLabel = Effect.Run;
                    break;
                case Simulation.ActorLabelType.Sit:
                    actor.SitLabel = Effect.Run;
                    break;
            }
        }
    }

    [Serializable]
    public class PlayerClearLabelEffect : Effect, IHideFromEditor
    {
        public static readonly PlayerClearLabelEffect Instance = new PlayerClearLabelEffect();

        public override void Run(Simulation.Actor actor)
        {
            actor.UpdateLabel = null;
            actor.SitLabel = null;
            actor.FallLabel = null;
            actor.EndKeyFrameLabel = new Simulation.ActorLabel[0];
            actor.HitEvent = null;

            actor.Variables.Clear();
        }
    }

    [Serializable]
    public class PlayerChangeFreeMoveEffect : Effect
    {
        public static readonly PlayerChangeFreeMoveEffect Instance = new PlayerChangeFreeMoveEffect();

        public override void Run(Simulation.Actor actor)
        {
            PlayerClearLabelEffect.Instance.Run(actor);
            //TODO freeCancel = true
        }
    }

    [Serializable]
    public class PlayerEndToFreeMoveEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            PlayerChangeFreeMoveEffect.Instance.Run(actor);
            //TODO warikomi
            if (actor.IsInAir)
            {
                PlayerBeginFallEffect.Instance.Run(actor);
            }
            else
            {
                PlayerBeginStandEffect.Instance.Run(actor);
            }
        }
    }

}
