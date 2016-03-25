using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public class SimpleBlock : Block
    {
        public SimpleBlock(IWritable head, IEnumerable<ILineObject> body)
        {
            base.Head.Add(head);
            base.Head.Add(new StringWritable("{"));
            base.BodyList.AddRange(body);
            base.Tail.Add(new StringWritable("}"));
        }

        public SimpleBlock(IEnumerable<ILineObject> body)
        {
            base.Head.Add(new StringWritable("{"));
            base.BodyList.AddRange(body);
            base.Tail.Add(new StringWritable("}"));
        }
    }
}
