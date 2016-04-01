using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Editor.Exporters;
using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects
{
    [Serializable]
    public class BulletInitEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleBlock(new ILineObject[] {
                new SimpleLineObject("this.ShotInit(t);"),
                new ControlBlock(ControlBlockType.If, "t.owner in this.actor", new ILineObject[] {
                    new SimpleLineObject("this.u.uu <- this.actor[t.owner].u.uu;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                    new SimpleLineObject("this.u.uu <- {};"),
                }).Statement(),
            }).Statement();
        }
    }

    [Serializable]
    public class TimeStopCheckEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new ControlBlock(ControlBlockType.If, "this.UpdateStopCheck()", new ILineObject[] {
                new SimpleLineObject("return false;"),
            }).Statement();
        }
    }

    [Serializable]
    public class BulletSetSpeedEffect : Effect
    {
        [XmlElement]
        [EditorChildNode("Speed")]
        public Value Speed;

        [XmlElement]
        [EditorChildNode("Rotation")]
        public Value Rotation;

        public override void Run(Simulation.Actor actor)
        {
            var s = Speed.Get(actor);
            var r = Rotation.Get(actor);

            actor.Rotation = r;
            actor.VX = (float)Math.Cos(r) * s;
            actor.VY = (float)Math.Sin(r) * s;
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var r = new IdentifierExpr("r");
            var s = new IdentifierExpr("s");
            var vx = new BiOpExpr(ThisExpr.Instance.MakeIndex("cos").Call(r), s, BiOpExpr.Op.Multiply);
            vx = new BiOpExpr(vx, ThisExpr.Instance.MakeIndex("direction"), BiOpExpr.Op.Multiply);
            var vy = new BiOpExpr(ThisExpr.Instance.MakeIndex("sin").Call(r), s, BiOpExpr.Op.Multiply);

            return new SimpleBlock(new ILineObject[] {
                new LocalVarStatement("r", Rotation.Generate(env)),
                new LocalVarStatement("s", Speed.Generate(env)),
                ThisExpr.Instance.MakeIndex("rz").Assign(r).Statement(),
                ThisExpr.Instance.MakeIndex("vx").Assign(vx).Statement(),
                ThisExpr.Instance.MakeIndex("vy").Assign(vy).Statement(),
                new SimpleLineObject("this.SetCollisionRotation(0.0, 0.0, this.rz);"),
            }).Statement();
        }
    }
}
