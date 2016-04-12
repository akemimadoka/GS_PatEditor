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
                    new SimpleLineObject("this.u.uu <- { uuu = null };"),
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

    [Serializable]
    public class BulletFollowingOwnerInitEffect : Effect
    {
        [XmlAttribute]
        public string CheckInstance { get; set; }

        public override void Run(Simulation.Actor actor)
        {
            actor.Variables.Add(CheckInstance, new Simulation.ActorVariable
            {
                Type = Simulation.ActorVariableType.Actor,
                Value = actor,
            });
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            List<ILineObject> ret = new List<ILineObject>();
            if (CheckInstance != null && CheckInstance.Length != 0)
            {
                //TODO fix this: set variable in owner, not this
                ret.Add(ActorVariableHelper.GenerateSet(CheckInstance, ThisExpr.Instance.MakeIndex("name")));
            }

            //get parent actor from table t
            ret.Add(ActorVariableHelper.GenerateSet("SYS_parent", new IdentifierExpr("t").MakeIndex("flag1")));
            return new SimpleBlock(ret).Statement();
        }
    }

    [Serializable]
    public class BulletFollowingOwnerUpdateEffect : Effect
    {
        [XmlAttribute]
        public string CheckInstance { get; set; }

        [XmlElement]
        [EditorChildNode("Position")]
        public PointProvider Position;

        public override void Run(Simulation.Actor actor)
        {
            var owner = actor.Owner;
            if (owner != null)
            {
                if (CheckInstance != null && CheckInstance.Length != 0)
                {
                    if (!owner.Variables.ContainsKey(CheckInstance) ||
                        owner.Variables[CheckInstance].Value as Simulation.Actor != actor)
                    {
                        actor.Release();
                        return;
                    }
                }

                actor.X = owner.X + owner.VX;
                actor.Y = owner.Y + owner.VY;
            }
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            List<ILineObject> ret = new List<ILineObject>();
            if (CheckInstance != null && CheckInstance.Length != 0)
            {
                ret.Add(new ControlBlock(ControlBlockType.If,
                    new BiOpExpr(ActorVariableHelper.GenerateGet(CheckInstance), ThisExpr.Instance.MakeIndex("name"), BiOpExpr.Op.NotEqual),
                    new ILineObject[] {
                        ThisExpr.Instance.MakeIndex("Release").Call().Statement(),
                    }).Statement());
            }

            var ownerActor = new IdentifierExpr("ownerActor");
            var x = Position.GenerateX(ownerActor, env);
            var y = Position.GenerateY(ownerActor, env);

            ret.AddRange(new ILineObject[] {
                new LocalVarStatement("ownerActor", ActorVariableHelper.GenerateGet("SYS_parent").MakeIndex("wr")),
                new ControlBlock(ControlBlockType.If, "ownerActor != null", new ILineObject[] {
                    ThisExpr.Instance.MakeIndex("x").Assign(x).Statement(),
                    ThisExpr.Instance.MakeIndex("y").Assign(y).Statement(),
                }).Statement(),
            });

            return new SimpleBlock(ret).Statement();
        }
    }
}
