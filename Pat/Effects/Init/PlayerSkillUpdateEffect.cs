using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects.Init
{
    [Serializable]
    public class PlayerSkillIncreaseCountEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.ActionCount += 1;
        }
    }

    [Serializable]
    public class PlayerSkillStopMovingEffect : Effect
    {
        [XmlAttribute]
        public float ReduceSpeed = 0.1f;
        public bool ShouldSerializeReduceSpeed() { return ReduceSpeed != 0.1f; }

        public override void Run(Simulation.Actor actor)
        {
            if (Math.Abs(actor.VX) < ReduceSpeed)
            {
                actor.VX = 0;
            }
            else
            {
                actor.VX -= actor.VX > 0 ? ReduceSpeed : -ReduceSpeed;
            }
        }
    }

}
