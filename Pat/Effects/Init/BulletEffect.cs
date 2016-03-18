using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class BulletInitEffect : Effect
    {
        [XmlAttribute]
        public string AnimationID;

        [XmlAttribute]
        public int Segment;

        public override void Run(Simulation.Actor actor)
        {
            actor.SetMotion(AnimationID, Segment);
        }
    }
}
