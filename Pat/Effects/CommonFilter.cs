using GS_PatEditor.Editor.Editable;
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
    }

    [Serializable]
    public class PlayerKeyReleasedFilter : Filter
    {
        //TODO allow control in editor
        public override bool Test(Simulation.Actor actor)
        {
            return false;
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
    }

}
