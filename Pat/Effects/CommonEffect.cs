using GS_PatEditor.Editor.EffectEditable;
using GS_PatEditor.Pat.Effects.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects
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
    public class CreateBulletEffect : Effect, IEditableEnvironment
    {
        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string ActionName { get; set; }

        [XmlElement]
        [EditorChildNode("Position")]
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

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
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
    public class SetMotionEffect : Effect, IEditableEnvironment
    {
        [XmlAttribute]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Animation { get; set; }

        [XmlAttribute]
        public int Segment { get; set; }

        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(Animation == "" ? null : Animation, Segment);
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }

    [Serializable]
    public class SetMotionRandomSegmentEffect : Effect, IEditableEnvironment
    {
        [XmlAttribute]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Animation { get; set; }

        [XmlAttribute]
        public int SegmentCount { get; set; }

        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(Animation == "" ? null : Animation, actor.World.Random.Next(SegmentCount));
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
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

    [Serializable]
    public class SetLabelEffect : Effect
    {
        [XmlAttribute]
        public Simulation.ActorLabelType Label { get; set; }

        [XmlElement]
        [EditorChildNode("Effect")]
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
}
