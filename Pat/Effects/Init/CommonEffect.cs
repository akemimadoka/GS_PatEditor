using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class AnimationContinueEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(actor.CurrentAnimation, actor.CurrentSegmentIndex + 1);
        }
    }

    [Serializable]
    public class CreateBulletEffect : Effect
    {
        [XmlElement]
        public string ActionName { get; set; }

        //TODO edit provider
        [XmlElement]
        public PointProvider Position;

        public override void Run(Simulation.Actor actor)
        {
            var bullet = new Simulation.BulletActor(actor.World,
                actor.Animations, null, actor.Actions);
            var point = Position.GetPointForActor(actor);

            bullet.X = point.X;
            bullet.Y = point.Y;

            var action = actor.Actions.GetActionByID(ActionName);
            if (action != null)
            {
                Simulation.ActionSetup.SetupActorForAction(bullet, action);
                actor.World.Add(bullet);
            }
        }
    }

    [Serializable]
    public class ReleaseActorEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.Release();
        }
    }

    [Serializable]
    public class SetMotionEffect : Effect
    {
        [XmlAttribute]
        public string Animation { get; set; }

        [XmlAttribute]
        public int Segment { get; set; }

        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(Animation == "" ? null : Animation, Segment);
        }
    }

    [Serializable]
    public class InitCountEffect : Effect
    {
        public static readonly InitCountEffect Instance = new InitCountEffect();

        public override void Run(Simulation.Actor actor)
        {
            actor.ActionCount = 0;
        }
    }

    [Serializable]
    public class IncreaseCountEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.ActionCount += 1;
        }
    }
}
