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
    public class AnimationSegmentFilter : Filter
    {
        [XmlAttribute]
        public int Segment { get; set; }

        public override bool Test(Simulation.Actor actor)
        {
            return actor.CurrentSegmentIndex == Segment;
        }

        public override Expression Generate(GenerationEnvironment env)
        {
            return new BiOpExpr(ThisExpr.Instance.MakeIndex("keyTake"), new ConstNumberExpr(Segment), BiOpExpr.Op.Equal);
        }
    }

    [Serializable]
    public class AnimationCountAfterFilter : Filter
    {
        [XmlElement]
        [EditorChildNode("Count")]
        public Value Count;

        public override bool Test(Simulation.Actor actor)
        {
            return actor.ActionCount > Count.GetInt(actor);
        }

        public override Expression Generate(GenerationEnvironment env)
        {
            return new BiOpExpr(ThisExpr.Instance.MakeIndex("count"), Count.Generate(env), BiOpExpr.Op.Greater);
        }
    }

    [Serializable]
    public class PlayerKeyReleasedFilter : Filter
    {
        //TODO allow control in editor
        public override bool Test(Simulation.Actor actor)
        {
            return false;
        }

        public override Expression Generate(GenerationEnvironment env)
        {
            return ThisExpr.Instance.MakeIndex("input").MakeIndex(env.GetCurrentSkillKeyName()).IsZero();
        }
    }

    [Serializable]
    public class AnimationCountModFilter : Filter
    {
        [XmlElement]
        [EditorChildNode("Divisor")]
        public Value Divisor;
        [XmlElement]
        [EditorChildNode("Mod")]
        public Value Mod;

        public override bool Test(Simulation.Actor actor)
        {
            return (actor.ActionCount % Divisor.GetInt(actor)) == Mod.GetInt(actor);
        }

        public override Expression Generate(GenerationEnvironment env)
        {
            var count = ThisExpr.Instance.MakeIndex("count");
            return new BiOpExpr(
                new BiOpExpr(count, Divisor.Generate(env), BiOpExpr.Op.Mod),
                Mod.Generate(env), BiOpExpr.Op.Equal);
        }
    }

}
