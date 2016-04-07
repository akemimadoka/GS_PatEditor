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
    public class TimeStopEffect : Effect
    {
        [XmlAttribute]
        public int Time { get; set; }

        public override void Run(Simulation.Actor actor)
        {
        }

        public override ILineObject Generate(GenerationEnvironment env)
        {
            return new SimpleBlock(new ILineObject[] {
                ThisExpr.Instance.MakeIndex("game").MakeIndex("timeStop").Assign(new ConstNumberExpr(Time)).Statement(),
                ThisExpr.Instance.MakeIndex("Set_MonoColor").Call().Statement(),
            }).Statement();
        }
    }
}
