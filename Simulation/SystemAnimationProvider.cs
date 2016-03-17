using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    public enum SystemAnimationType
    {
        Stand,
        Run,
        JumpUp,
        JumpFront,
        Fall,
        FallAttack,
    }

    public class SystemAnimationProvider
    {
        private readonly Pat.Project _Project;

        public SystemAnimationProvider(Pat.Project proj)
        {
            _Project = proj;
        }

        public Pat.Animation GetSystemAnimation(SystemAnimationType type)
        {
            switch (type)
            {
                case SystemAnimationType.Stand:
                    return GetSystemAnimation_0_Stant();
                case SystemAnimationType.Run:
                    return GetSystemAnimation_1_Run();
                case SystemAnimationType.JumpUp:
                    return GetSystemAnimation_3_JumpUp();
                case SystemAnimationType.JumpFront:
                    return GetSystemAnimation_4_JumpFront();
                case SystemAnimationType.Fall:
                    return GetSystemAnimation_8_Fall();
                case SystemAnimationType.FallAttack:
                    return GetSystemAnimation_9_FallAttack();
                default:
                    return null;
            }
        }

        public Pat.Animation GetSystemAnimation_0_Stant()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "stand");
        }

        public Pat.Animation GetSystemAnimation_1_Run()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "run");
        }

        public Pat.Animation GetSystemAnimation_3_JumpUp()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "jump_up");
        }

        public Pat.Animation GetSystemAnimation_4_JumpFront()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "jump_front");
        }

        public Pat.Animation GetSystemAnimation_8_Fall()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "fall");
        }

        public Pat.Animation GetSystemAnimation_9_FallAttack()
        {
            return _Project.Animations.FirstOrDefault(a => a.AnimationID == "fall_attack");
        }
    }
}
