using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public abstract class Statement : ILineObject
    {
        public abstract void Write(TextWriter output, int indent);
    }

    public class BlockStatement : Statement
    {
        private readonly Block _Block;

        public BlockStatement(Block block)
        {
            _Block = block;
        }

        public override void Write(TextWriter output, int indent)
        {
            output.WriteIndent(indent);
            _Block.WriteHead(output, indent);
            _Block.WriteBody(output, indent);
            _Block.WriteTail(output, indent);
            output.WriteLine();
        }
    }

    public class ExprStatement : Statement
    {
        private readonly Expression _Expr;

        public ExprStatement(Expression expr)
        {
            _Expr = expr;
        }

        public override void Write(TextWriter output, int indent)
        {
            output.WriteIndent(indent);
            _Expr.Write(output, indent + 1);
            output.WriteLine(';');
        }
    }
}
