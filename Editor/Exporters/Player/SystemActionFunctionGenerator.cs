using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.Player
{
    class SystemActionFunctionGenerator
    {
        public static void Generate(PlayerExporter exporter, Pat.Project proj, CodeGenerator output)
        {
            output.WriteStatement(GenerateBeginAirJump(exporter, proj).Statement());
            output.WriteStatement(GenerateBeginAirSlide(exporter, proj).Statement());
            output.WriteStatement(GenerateBeginJump(exporter, proj).Statement());
            output.WriteStatement(GenerateBeginWalk(exporter, proj).Statement());
            output.WriteStatement(GenerateInputCancelMove(exporter, proj).Statement());
            output.WriteStatement(GenerateInputAttack(exporter, proj).Statement());
        }

        private static FunctionBlock GenerateBeginAirJump(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("BeginAirJump", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.u.jumpCount == 0", new ILineObject[] {
                    new SimpleLineObject("this.ChangeFreeMove();"),
                    new SimpleLineObject("this.sitLabel = this.u.BeginSit;"),
                    new SimpleLineObject("this.u.jumpCount++;"),
                    new SimpleLineObject("this.isAir = true;"),
                    new SimpleLineObject("this.collisionMask = 8 | 16;"),
                    new SimpleLineObject("this.vy = -15.0;"),
                    new SimpleLineObject("this.PlaySE(1000);"),
                    new ControlBlock(ControlBlockType.If, "this.input.x * this.input.x >= 0.01", new ILineObject[] {
                        new SimpleLineObject("this.vx = this.input.x >= 0.1 ? 4.0 : -4.0;"),
                        new SimpleLineObject("this.direction = this.input.x >= 0.1 ? 1.0 : -1.0;"),
                        new SimpleLineObject("this.SetMotion(this.u.CA + 4, 0);"),
                        new SimpleLineObject("return;"),
                    }).Statement(),
                    new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                        new SimpleLineObject("this.vx = 0.0;"),
                        new SimpleLineObject("this.SetMotion(this.u.CA + 3, 0);"),
                        new SimpleLineObject("return;"),
                    }).Statement(),
                }).Statement(),
            });
        }

        private static FunctionBlock GenerateBeginAirSlide(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("BeginAirSlide", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.input.x", new ILineObject[] {
                    new SimpleLineObject("this.vx += this.input.x > 0 ? 0.2 : -0.2;"),
                    new ControlBlock(ControlBlockType.If, "this.Abs(this.vx) > 4.0", new ILineObject[] {
                        new SimpleLineObject("this.vx = this.vx > 0.0 ? 4.0 : -4.0;"),
                    }).Statement(),
                }).Statement(),
            });
        }

        private static FunctionBlock GenerateBeginJump(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("BeginJump", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "!this.isAir", new ILineObject[] {
                    new SimpleLineObject("this.ChangeFreeMove();"),
                    new SimpleLineObject("this.sitLabel = this.u.BeginSit;"),
                    new SimpleLineObject("this.isAir = true;"),
                    new SimpleLineObject("this.collisionMask = 8 | 16;"),
                    new SimpleLineObject("this.vy = -15.0;"),
                    new SimpleLineObject("this.PlaySE(1000);"),

                    new ControlBlock(ControlBlockType.If, "this.input.x * this.input.x >= 0.01", new ILineObject[] {
                        new SimpleLineObject("this.vx = this.input.x >= 0.1 ? 4.0 : -4.0;"),
                        new SimpleLineObject("this.direction = this.input.x >= 0.1 ? 1.0 : -1.0;"),
                        new SimpleLineObject("this.SetMotion(this.u.CA + 4, 0);"),
                    }).Statement(),
                    new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                        new SimpleLineObject("this.vx = 0.0;"),
                        new SimpleLineObject("this.SetMotion(this.u.CA + 3, 0);"),
                    }).Statement(),
                }).Statement(),
            });
        }

        private static FunctionBlock GenerateBeginWalk(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("BeginWalk", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.input.x > 0", new ILineObject[] {
                    new SimpleLineObject("this.direction = 1.0;"),
                    new SimpleLineObject("this.vx = 4.0;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                    new SimpleLineObject("this.direction = -1.0;"),
                    new SimpleLineObject("this.vx = -4.0;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.If, "this.motion != this.u.CA + 1", new ILineObject[] {
                    new SimpleLineObject("this.ChangeFreeMove();"),
                    new SimpleLineObject("this.fallLabel = this.u.BeginFall;"),
                    new SimpleLineObject("this.SetMotion(this.u.CA + 1, 0);"),
                }).Statement(),
            });
        }

        private static FunctionBlock GenerateInputCancelMove(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("InputCancelMove", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.flagState & 2097152 && this.input.b2 > 0 && this.input.b2 < 10", new ILineObject[] {
                    new ControlBlock(ControlBlockType.If, "this.isAir", new ILineObject[] {
                        new SimpleLineObject("this.u.BeginAirJump.call(this);"),
                    }).Statement(),
                    new ControlBlock(ControlBlockType.Else, new ILineObject[] {
                        new SimpleLineObject("this.u.BeginJump.call(this);"),
                    }).Statement(),
                }).Statement(),
            });
        }

        private static FunctionBlock GenerateInputAttack(PlayerExporter exporter, Pat.Project proj)
        {
            return new FunctionBlock("InputAttack", new string[0], new ILineObject[] {
                new ControlBlock(ControlBlockType.If, "this.input.b0 == 1", new ILineObject[] {
                    new SimpleLineObject("this.u.inputCountA = 10;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.If, "this.input.b1 == 1", new ILineObject[] {
                    new SimpleLineObject("this.u.inputCountB = 10;"),
                }).Statement(),
                new ControlBlock(ControlBlockType.If, "this.input.b3 == 1", new ILineObject[] {
                    new SimpleLineObject("this.u.inputCountC = 10;"),
                }).Statement(),
                new SimpleLineObject("this.u.inputCountA--;"),
                new SimpleLineObject("this.u.inputCountB--;"),
                new SimpleLineObject("this.u.inputCountC--;"),
            }.Concat(SkillGenerator.GenerateInputAttackFunction(exporter)));
        }
    }
}
