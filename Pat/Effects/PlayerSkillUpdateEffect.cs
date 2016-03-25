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
    public class PlayerSkillStopMovingEffect : Effect
    {
        [XmlElement]
        [EditorChildNode("ReduceSpeed")]
        public Value ReduceSpeed = new ConstValue { Value = 0.1f };

        public bool ShouldSerializeReduceSpeed()
        {
            return !(
                ReduceSpeed is ConstValue &&
                ((ConstValue)ReduceSpeed).Value == 0.1f);
        }

        public override void Run(Simulation.Actor actor)
        {
            var rs = ReduceSpeed.Get(actor);
            if (Math.Abs(actor.VX) < rs)
            {
                actor.VX = 0;
            }
            else
            {
                actor.VX -= actor.VX > 0 ? rs : -rs;
            }
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            var val = ReduceSpeed.ToString();
            return new SimpleBlock(new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.Abs(this.vx) <= " + val, new ILineObject[] {
                    new SimpleLineObject("this.vx = 0.0;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                    new SimpleLineObject("this.vx -= this.vx > 0.0 ? " + val + " : -(" + val + ")7;"),
                }).Statement(),
            }).Statement();
        }
    }

}
