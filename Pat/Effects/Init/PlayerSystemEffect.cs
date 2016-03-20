using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class PlayerBeginFallEffect : Effect
    {
        public static readonly PlayerBeginFallEffect Instance = new PlayerBeginFallEffect();

        private PlayerChangeFreeMoveEffect _ChangeToFreeMove = new PlayerChangeFreeMoveEffect();

        public override void Run(Simulation.Actor actor)
        {
            _ChangeToFreeMove.Run(actor);
            //TODO use child actions
            actor.SetMotion(Simulation.SystemAnimationType.Fall, 0);
            actor.SitLabel = PlayerBeginStandEffect.Instance.Run;
            actor.IsInAir = true;
        }
    }

    [Serializable]
    public class PlayerBeginStandEffect : Effect
    {
        public static readonly PlayerBeginStandEffect Instance = new PlayerBeginStandEffect();

        private PlayerChangeFreeMoveEffect _ChangeToFreeMove = new PlayerChangeFreeMoveEffect();

        public override void Run(Simulation.Actor actor)
        {
            _ChangeToFreeMove.Run(actor);
            actor.SetMotion(Simulation.SystemAnimationType.Stand, 0);
            actor.IsInAir = false;
            actor.VX = 0;
            actor.VY = 0;
        }
    }

}
