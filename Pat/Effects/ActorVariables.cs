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
    public class ActorFloatVariableValue : Value
    {
        [XmlAttribute]
        public string Name { get; set; }

        public override float Get(Simulation.Actor actor)
        {
            var val = actor.Variables[Name];
            if (val.Type == Simulation.ActorVariableType.Float)
            {
                return (float)val.Value;
            }
            return 0;
        }

        public override Expression Generate(GenerationEnvironment env)
        {
            return ThisExpr.Instance.MakeIndex("u").MakeIndex("variables").MakeIndex(Name);
        }
    }

    [Serializable]
    public class ActorSetFloatVariableEffect : Effect
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        [EditorChildNode("Value")]
        public Value Value;

        public override void Run(Simulation.Actor actor)
        {
            actor.Variables[Name] = new Simulation.ActorVariable()
            {
                Type = Simulation.ActorVariableType.Float,
                Value = Value.Get(actor),
            };
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var nname = Name.Replace("\\", "\\\\").Replace("\"", "\\\\");
            return new SimpleBlock(new ILineObject[] {
                new SimpleLineObject("if (!(\"variables\" in this.u)) this.u.variables <- {};"),
                new SimpleLineObject("if (!(\"" + nname + "\" in this.u.variables)) this.u.variables[\"" + nname + "\"] <- 0;"),
                new BiOpExpr(ThisExpr.Instance.MakeIndex("u").MakeIndex("variables").MakeIndex(Name),
                    Value.Generate(env), BiOpExpr.Op.Assign).Statement(),
            }).Statement();
        }
    }
}
