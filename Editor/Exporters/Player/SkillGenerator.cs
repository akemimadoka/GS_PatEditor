﻿using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.Player
{
    class SkillGenerator
    {
        private class SkillGeneratorEnv : GenerationEnvironment
        {
            public PlayerExporter Exporter;
            public string CurrentActionName;
            public CodeGenerator Output;
            public string CurrentSkillKeyName;

            private Dictionary<string, string> _GeneratedActorInit = new Dictionary<string, string>();

            public int GetActionID(string name)
            {
                if (name == null || name.Length == 0)
                {
                    return Exporter.GetActionID(CurrentActionName);
                }
                return Exporter.GetActionID(name);
            }

            public string GenerateActionAsActorInit(string name)
            {
                string ret;
                if (_GeneratedActorInit.TryGetValue(name, out ret))
                {
                    return ret;
                }

                var action = Exporter.GetAction(name);

                var lastActionName = CurrentActionName;
                CurrentActionName = name;
                List<ILineObject> funcContent = new List<ILineObject>();
                funcContent.AddRange(GenerateNormalSkillFunction(Exporter, this, name, true));
                CurrentActionName = lastActionName;

                ret = "InitAction_" + name;
                var func = new FunctionBlock(ret, new string[] { "t" }, funcContent);

                Output.WriteStatement(func.Statement());

                _GeneratedActorInit.Add(name, ret);
                return ret;
            }

            public string GetCurrentSkillKeyName()
            {
                return CurrentSkillKeyName;
            }

            public string GetSegmentStartEventHandlerFunctionName()
            {
                return "SegmentStartEventHandler";
            }
        }

        public static ILineObject[] GenerateInputAttackFunction(PlayerExporter exporter)
        {
            //ILineObject[] ret = new ILineObject[exporter.Skills.Count];
            //for (int i = 0; i < exporter.Skills.Count; ++i)
            //{
            //    ret[i] = GenerateSkill(i == 0, exporter.Skills[i], "skill_" + i.ToString());
            //}
            //return ret;
            return Enumerable.Range(0, exporter.Skills.Count)
                .Select(i => new
                {
                    Code = GenerateSkill(exporter, i == 0, exporter.Skills[i], "skill_" + i.ToString()),
                    Priority = exporter.Skills[i].Priority,
                })
                .OrderByDescending(s => s.Priority)
                .Select(s => s.Code)
                .ToArray();
        }

        public static GenerationEnvironment CreateEnv(PlayerExporter exporter, CodeGenerator output)
        {
            return new SkillGeneratorEnv()
            {
                Exporter = exporter,
                Output = output,
            };
        }

        public static void GenerateSkills(PlayerExporter exporter, CodeGenerator output)
        {
            SkillGeneratorEnv env = (SkillGeneratorEnv)exporter.GenEnv;

            for (int i = 0; i < exporter.Skills.Count; ++i)
            {
                var skill = exporter.Skills[i];
                var skillFuncName = "skill_" + i.ToString();
                if (skill is NormalSkill)
                {
                    var cskill = (NormalSkill)skill;
                    env.CurrentSkillKeyName = cskill.Key.GetKeyName();
                    env.CurrentActionName = cskill.ActionID;

                    var functionContent = GenerateNormalSkillFunction(exporter, env, cskill.ActionID, false);
                    functionContent = new ILineObject[] {
                        new ControlBlock(ControlBlockType.If, "!(\"uu\" in this.u)", new ILineObject[] {
                            new SimpleLineObject("this.u.uu <- { uuu = this.u.weakref() };"),
                        }).Statement(),
                    }.Concat(functionContent);

                    var func = new FunctionBlock(skillFuncName, new string[0], functionContent);
                    output.WriteStatement(func.Statement());
                }
            }
        }

        public static void GenerateStartMotionFunction(SegmentStartEventRecorder r, CodeGenerator output)
        {
            output.WriteStatement(new FunctionBlock("SegmentStartEventHandler", new string[0],
                new ILineObject[] {
                    r.Generate(),
                    new SimpleLineObject("if (!(\"lastKeyTake\" in this.u)) this.u.lastKeyTake <- -1;"),
                    new SimpleLineObject("this.u.lastKeyTake = this.keyTake;"),
                }).Statement());
        }

        private static ILineObject GenerateNormalSkillUpdateSSEChecker()
        {
            return new SimpleBlock(new ILineObject[] {
                new SimpleLineObject("if (!(\"lastKeyTake\" in this.u)) this.u.lastKeyTake <- -1;"),
                new ControlBlock(ControlBlockType.If, "this.u.lastKeyTake != this.keyTake", new ILineObject[] {
                    new SimpleLineObject("this.u.uu.uuu.SegmentStartEventHandler.call(this);"),
                    new SimpleLineObject("this.u.lastKeyTake = this.keyTake;"),
                }).Statement(),
            }).Statement();
        }

        #region InputAttack

        private static ILineObject GenerateSkill(PlayerExporter exporter, bool isFirst, Skill skill, string name)
        {
            if (skill is NormalSkill)
            {
                return GenerateNormalSkill(exporter, isFirst, (NormalSkill)skill, name);
            }
            return new SimpleLineObject("");
        }

        private static ILineObject GenerateNormalSkill(PlayerExporter exporter, bool isFirst, NormalSkill skill, string name)
        {
            var u = ThisExpr.Instance.MakeIndex("u");
            var condition = ExpressionExt.AndAll(
                KeyCondition(skill.Key),
                CancelLevelCondition(skill.CancelLevel),
                AirCondition(skill.AirState),
                XCondition(skill.X),
                YCondition(skill.Y),
                MagicCondition(skill.MagicUse),
                RushCondition(exporter, skill));
            return new ControlBlock(isFirst ? ControlBlockType.If : ControlBlockType.ElseIf,
                condition, new ILineObject[] {
                u.MakeIndex("InputReset").MakeIndex("call").Call(ThisExpr.Instance).Statement(),
                u.MakeIndex(name).MakeIndex("call").Call(ThisExpr.Instance).Statement(),
            }).Statement();
        }

        private static Expression RushCondition(PlayerExporter exporter, NormalSkill skill)
        {
            if (skill.IsRushSkill)
            {
                var skillActionID = skill.ActionID;
                var indexes = exporter.Skills
                    .Where(s => s is NormalSkill)
                    .Select(s => (NormalSkill)s)
                    .Where(s => s.RushCancel == skillActionID)
                    .Select(s =>
                        new BiOpExpr(ThisExpr.Instance.MakeIndex("motion"),
                            new BiOpExpr(ThisExpr.Instance.MakeIndex("u").MakeIndex("CA"),
                                new ConstNumberExpr(exporter.GetActionID(s.ActionID)), BiOpExpr.Op.Add),
                            BiOpExpr.Op.Equal))
                    .ToArray();
                return ExpressionExt.OrAll(indexes);
            }
            return new ConstNumberExpr(1);
        }

        private static Expression KeyCondition(SkillKey key)
        {
            string index;
            switch (key)
            {
                case SkillKey.KeyA:
                    index = "inputCountA";
                    break;
                case SkillKey.KeyB:
                    index = "inputCountB";
                    break;
                case SkillKey.KeyC:
                    index = "inputCountC";
                    break;
                default:
                    return new ConstNumberExpr(1);
            }
            return ThisExpr.Instance.MakeIndex("u").MakeIndex(index).GreaterZero();
        }

        private static Expression CancelLevelCondition(Pat.CancelLevel value)
        {
            return ThisExpr.Instance
                .MakeIndex("C_Check")
                .Call(new ConstNumberExpr(ExportHelper.ExportCancelLevel(value)));
        }

        private static Expression MagicCondition(int value)
        {
            if (value == 0)
            {
                return new ConstNumberExpr(1);
            }
            return ThisExpr.Instance.MakeIndex("UseMagic").Call(new ConstNumberExpr(value));
        }

        private static Expression AirCondition(AirState value)
        {
            switch (value)
            {
                case AirState.AirOnly:
                    return ThisExpr.Instance.MakeIndex("isAir");
                case AirState.GroundOnly:
                    return new UnOpExpr(ThisExpr.Instance.MakeIndex("isAir"), UnOpExpr.Op.Not);
                case AirState.Any:
                default:
                    return new ConstNumberExpr(1);
            }
        }

        private static Expression XCondition(DirectionHorizontal value)
        {
            var x = ThisExpr.Instance.MakeIndex("input").MakeIndex("x");
            switch (value)
            {
                case DirectionHorizontal.Empty:
                    return x.IsZero();
                case DirectionHorizontal.Front:
                    return x.NotZero();
                case DirectionHorizontal.Any:
                default:
                    return new ConstNumberExpr(1);
            }
        }

        private static Expression YCondition(DirectionVertical value)
        {
            var y = ThisExpr.Instance.MakeIndex("input").MakeIndex("y");
            switch (value)
            {
                case DirectionVertical.Empty:
                    return y.IsZero();
                case DirectionVertical.DownOnly:
                    return y.GreaterZero();
                case DirectionVertical.UpOnly:
                    return y.LessZero();
                case DirectionVertical.Any:
                default:
                    return new ConstNumberExpr(1);
            }
        }

        #endregion

        #region Skill

        private static IEnumerable<ILineObject> GenerateNormalSkillFunction(PlayerExporter exporter,
            SkillGeneratorEnv env, string id, bool stateLabelAsUpdate)
        {
            List<ILineObject> ret = new List<ILineObject>();

            var action = exporter.GetAction(id);
            var ae = new Pat.ActionEffects(action);
            foreach (var b in action.Behaviors)
            {
                b.MakeEffects(ae);
            }

            exporter.SSERecorder.AddAction(ae, env.GetActionID(id), env);

            ret.AddRange(ae.InitEffects.Select(e => e.Generate(env)));

            var list2 = ae.UpdateEffects.Select(e => e.Generate(env));
            list2 = new ILineObject[] { GenerateNormalSkillUpdateSSEChecker() }.Concat(list2);
            if (stateLabelAsUpdate)
            {
                list2 = list2.Concat(new ILineObject[] { new SimpleLineObject("return true;") });
            }

            var updateFunc = new FunctionBlock("", new string[0], list2).AsExpression();
            ILineObject setUpdate;
            if (stateLabelAsUpdate)
            {
                setUpdate = ThisExpr.Instance.MakeIndex("SetUpdateFunction").Call(updateFunc).Statement();
            }
            else
            {
                setUpdate = ThisExpr.Instance.MakeIndex("stateLabel").Assign(updateFunc).Statement();
            }
            ret.Add(setUpdate);

            var keys = ae.SegmentFinishEffects.Select(
                keyEffect => new FunctionBlock("", new string[0], keyEffect.Select(e => e.Generate(env))).AsExpression());
            var keyCount = action.Segments.Count - 1;
            if (ae.SegmentFinishEffects.Count < keyCount)
            {
                keyCount = ae.SegmentFinishEffects.Count;
            }
            var arrayObj = new ArrayExpr(keys.Take(keyCount).ToArray());
            var setKey = ThisExpr.Instance.MakeIndex("keyAction").Assign(arrayObj).Statement();
            ret.Add(new SimpleLineObject("this.SetEndTakeCallbackFunction(this.KeyActionCheck);"));
            ret.Add(setKey);

            if (ae.SegmentFinishEffects.Count >= action.Segments.Count)
            {
                var effects = ae.SegmentFinishEffects[action.Segments.Count - 1].Select(e => e.Generate(env));
                var funcEndMotion = new FunctionBlock("", new string[0], effects).AsExpression();
                var setEndMotion = ThisExpr.Instance.MakeIndex("SetEndMotionCallbackFunction").Call(funcEndMotion).Statement();
                ret.Add(setEndMotion);
            }

            return ret;
        }

        #endregion
    }
}
