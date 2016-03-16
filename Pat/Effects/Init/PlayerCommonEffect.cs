using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class PlayerSetLabelEffect : Effect
    {
        [XmlAttribute]
        public Simulation.ActorLabelType Label;
        [XmlAttribute]
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
    public class PlayerClearLabelEffect : Effect
    {
        public static readonly PlayerClearLabelEffect Instance = new PlayerClearLabelEffect();

        public override void Run(Simulation.Actor actor)
        {
            actor.UpdateLabel = null;
            actor.SitLabel = null;
            actor.FallLabel = null;
            actor.EndKeyFrameLabel = null;
            actor.HitEvent = null;

            actor.Variables.Clear();
        }
    }

    [Serializable]
    public class PlayerChangeToFreeMoveEffect : Effect
    {
        private PlayerClearLabelEffect _ClearLabel = PlayerClearLabelEffect.Instance;

        public override void Run(Simulation.Actor actor)
        {
            //TODO freeCancel
            //TODO warikomi
            _ClearLabel.Run(actor);
        }
    }

}
