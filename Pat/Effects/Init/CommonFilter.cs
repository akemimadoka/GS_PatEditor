using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class AnimationSegmentFilter : Filter
    {
        [XmlAttribute]
        public int Segment;

        public override bool Test(Simulation.Actor actor)
        {
            return actor.CurrentSegmentIndex == Segment;
        }
    }

    [Serializable]
    public class AnimationCountAfterFilter : Filter
    {
        [XmlAttribute]
        public int Count;

        public override bool Test(Simulation.Actor actor)
        {
            return actor.ActionCount > Count;
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
        [XmlAttribute]
        public int Divisor;
        [XmlAttribute]
        public int Mod;

        public override bool Test(Simulation.Actor actor)
        {
            return actor.ActionCount % Divisor == Mod;
        }
    }

}
