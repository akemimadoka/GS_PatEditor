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

        public Pat.Action GetSystemAnimation(SystemAnimationType type)
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

        public Pat.Action GetSystemAnimation_0_Stant()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "stand");
        }

        public Pat.Action GetSystemAnimation_1_Run()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "run");
        }

        public Pat.Action GetSystemAnimation_3_JumpUp()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "jump_up");
        }

        public Pat.Action GetSystemAnimation_4_JumpFront()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "jump_front");
        }

        public Pat.Action GetSystemAnimation_8_Fall()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "fall");
        }

        public Pat.Action GetSystemAnimation_9_FallAttack()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "fall_attack");
        }
    }
}
