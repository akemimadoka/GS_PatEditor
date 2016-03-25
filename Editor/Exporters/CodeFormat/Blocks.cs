using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public class FunctionBlock : SimpleBlock
    {
        private class FunctionBlockExpr : Expression
        {
            public FunctionBlock Function;

            public override void Write(TextWriter writer, int indent)
            {
                Function.WriteHead(writer, indent);
                Function.WriteBody(writer, indent);
                Function.WriteTail(writer, indent);
            }
        }
        public FunctionBlock(string name, string[] parameters, IEnumerable<ILineObject> body)
            : base(new StringWritable("function " + name + "(" + string.Join(", ", parameters) + ")"), body)
        {
        }

        public Expression AsExpression()
        {
            return new FunctionBlockExpr { Function = this };
        }
    }

    public enum ControlBlockType
    {
        If,
        ElseIf,
        Else,
        While,
        For,
    }

    public static class ControlBlockTypeExt
    {
        public static string GetKeyWord(this ControlBlockType type)
        {
            switch (type)
            {
                case ControlBlockType.If: return "if";
                case ControlBlockType.ElseIf: return "else if";
                case ControlBlockType.Else: return "else";
                case ControlBlockType.While: return "while";
                case ControlBlockType.For: return "for";
                default: return "";
            }
        }
    }

    public class ControlBlock : SimpleBlock
    {
        public ControlBlock(ControlBlockType type, IEnumerable<ILineObject> body)
            : base(new StringWritable(type.GetKeyWord()), body)
        {
        }

        public ControlBlock(ControlBlockType type, string expr, IEnumerable<ILineObject> body)
            : base(new StringWritable(type.GetKeyWord() + (expr == null ? "" : " (" + expr + ")")), body)
        {
        }

        public ControlBlock(ControlBlockType type, Expression expr, IEnumerable<ILineObject> body)
            : base(new MultipartWritable(new StringWritable(type.GetKeyWord() + " ("),
                expr, new StringWritable(")")), body)
        {
        }
    }
}
