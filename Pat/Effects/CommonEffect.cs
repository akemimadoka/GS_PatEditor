using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Editor.Exporters;
using GS_PatEditor.Editor.Exporters.CodeFormat;
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
            actor.SetMotion(actor.CurrentAction, actor.CurrentSegmentIndex + 1);
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return ThisExpr.Instance.MakeIndex("SetMotion").Call(
                ThisExpr.Instance.MakeIndex("motion"),
                new BiOpExpr(ThisExpr.Instance.MakeIndex("keyTake"), new ConstNumberExpr(1), BiOpExpr.Op.Add)
                ).Statement();
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var funcName = env.GenerateActionAsActorInit(ActionName);

            return new SimpleBlock(new ILineObject[] {
                //create t
                new SimpleLineObject("local t = this.DefaultShotTable();"),
                //TODO setup t

                //create actor
                ThisExpr.Instance.MakeIndex("world2D").MakeIndex("CreateActor").Call(
                    Position.GenerateX(env),
                    Position.GenerateY(env),
                    ThisExpr.Instance.MakeIndex("direction"),
                    ThisExpr.Instance.MakeIndex(funcName),
                    new IdentifierExpr("t")
                ).Statement(),
            }).Statement();
        }

        [XmlIgnore]
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return ThisExpr.Instance.MakeIndex("Release").Call().Statement();
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var id = env.GetActionID(Animation);
            return ThisExpr.Instance.MakeIndex("SetMotion").Call(
                new ConstNumberExpr(id),
                new ConstNumberExpr(Segment)).Statement();
        }

        [XmlIgnore]
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var id = env.GetActionID(Animation);
            var segment = new BiOpExpr(ThisExpr.Instance.MakeIndex("rand").Call(),
                new ConstNumberExpr(SegmentCount), BiOpExpr.Op.Mod);
            return ThisExpr.Instance.MakeIndex("SetMotion").Call(new ConstNumberExpr(id), segment).Statement();
        }

        [XmlIgnore]
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleLineObject("this.count = 0;");
        }
    }

    [Serializable]
    public class IncreaseCountEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.ActionCount += 1;
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleLineObject("this.count++;");
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

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var func = new FunctionBlock("", new string[0], new ILineObject[] { Effect.Generate(env) });
            switch (Label)
            {
                case Simulation.ActorLabelType.Fall:
                    return ThisExpr.Instance.MakeIndex("fallLabel").Assign(func.AsExpression()).Statement();
                case Simulation.ActorLabelType.Sit:
                    return ThisExpr.Instance.MakeIndex("sitLabel").Assign(func.AsExpression()).Statement();
                default:
                    throw new Exception();
            }
        }
    }
}
