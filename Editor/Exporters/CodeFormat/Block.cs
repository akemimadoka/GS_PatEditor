using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    //the first line of head is write directly
    //the last line of tail does not write newline
    public class Block
    {
        public readonly List<IWritable> Head = new List<IWritable>();
        public readonly List<IWritable> Tail = new List<IWritable>();

        public readonly List<ILineObject> BodyList = new List<ILineObject>();

        public void WriteHead(TextWriter output, int lastIndent)
        {
            if (Head.Count > 0)
            {
                Head[0].Write(output, lastIndent + 1);
                output.WriteLine();
            }
            for (int i = 1; i < Head.Count; ++i)
            {
                output.WriteIndent(lastIndent);
                Head[i].Write(output, lastIndent + 1);
                output.WriteLine();
            }
        }

        public void WriteTail(TextWriter output, int lastIndent)
        {
            for (int i = 0; i < Tail.Count - 1; ++i)
            {
                output.WriteIndent(lastIndent);
                Tail[i].Write(output, lastIndent + 1);
                output.WriteLine();
            }
            if (Tail.Count > 0)
            {
                output.WriteIndent(lastIndent);
                Tail[Tail.Count - 1].Write(output, lastIndent + 1);
            }
        }

        public void WriteBody(TextWriter output, int lastIndent)
        {
            foreach (var obj in BodyList)
            {
                obj.Write(output, lastIndent + 1);
            }
        }
    }

    public static class BlockExt
    {
        public static ILineObject Statement(this Block block)
        {
            return new BlockStatement(block);
        }
    }
}
