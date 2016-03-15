using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    class PlayerActor : Actor
    {
        public PlayerActor(World world, SystemAnimationProvider animation)
            : base(world, animation)
        {
            ImmuneGravity = false;
            DefaultGravity = 1.0f;
            Priority = 100;

            CollisionEnabled = true;
        }
        public override void Update()
        {
            UpdateGravity();
            StepAnimation();
        }
    }
}
