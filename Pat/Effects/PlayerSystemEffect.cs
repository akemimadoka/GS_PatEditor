using GS_PatEditor.Editor.Exporters;
using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Effects
{
    [Serializable]
    public class PlayerBeginFallEffect : Effect
    {
        public static readonly PlayerBeginFallEffect Instance = new PlayerBeginFallEffect();

        public override void Run(Simulation.Actor actor)
        {
            PlayerChangeFreeMoveEffect.Instance.Run(actor);
            //TODO use child actions
            actor.SetMotion(Simulation.SystemAnimationType.Fall, 0);
            actor.SitLabel = PlayerBeginStandEffect.Instance.Run;
            actor.IsInAir = true;
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleLineObject("this.u.BeginFall.call(this);");
        }
    }

    [Serializable]
    public class PlayerBeginStandEffect : Effect
    {
        public static readonly PlayerBeginStandEffect Instance = new PlayerBeginStandEffect();

        public override void Run(Simulation.Actor actor)
        {
            PlayerChangeFreeMoveEffect.Instance.Run(actor);
            actor.SetMotion(Simulation.SystemAnimationType.Stand, 0);
            actor.IsInAir = false;
            actor.VX = 0;
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleLineObject("this.u.BeginStand.call(this);");
        }
    }

    [Serializable]
    public class PlayerBeginSitEffect : Effect
    {
        public static readonly PlayerBeginSitEffect Instance = new PlayerBeginSitEffect();

        public override void Run(Simulation.Actor actor)
        {
            PlayerChangeFreeMoveEffect.Instance.Run(actor);
            actor.SetMotion(Simulation.SystemAnimationType.Stand, 0);
            actor.IsInAir = false;
            actor.VX = 0;
            actor.VY = 0;
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleLineObject("this.u.BeginSit.call(this);");
        }
    }
}
