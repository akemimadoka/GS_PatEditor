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

        public Pat.Action GetSystemAction(SystemAnimationType type)
        {
            switch (type)
            {
                case SystemAnimationType.Stand:
                    return GetSystemAction_0_Stant();
                case SystemAnimationType.Run:
                    return GetSystemAction_1_Run();
                case SystemAnimationType.JumpUp:
                    return GetSystemAction_3_JumpUp();
                case SystemAnimationType.JumpFront:
                    return GetSystemAction_4_JumpFront();
                case SystemAnimationType.Fall:
                    return GetSystemAction_8_Fall();
                case SystemAnimationType.FallAttack:
                    return GetSystemAction_9_FallAttack();
                default:
                    return null;
            }
        }

        public Pat.Action GetSystemAction_0_Stant()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "stand");
        }

        public Pat.Action GetSystemAction_1_Run()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "run");
        }

        public Pat.Action GetSystemAction_3_JumpUp()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "jump_up");
        }

        public Pat.Action GetSystemAction_4_JumpFront()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "jump_front");
        }

        public Pat.Action GetSystemAction_8_Fall()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "fall");
        }

        public Pat.Action GetSystemAction_9_FallAttack()
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == "fall_attack");
        }
    }
}
