using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public interface IWritable
    {
        void Write(TextWriter writer, int indent);
    }

    public class StringWritable : IWritable
    {
        private readonly string _Value;

        public StringWritable(string val)
        {
            _Value = val;
        }

        public void Write(TextWriter writer, int indent)
        {
            writer.Write(_Value);
        }
    }

    public class MultipartWritable : IWritable
    {
        private readonly IWritable[] _List;

        public MultipartWritable(params IWritable[] list)
        {
            _List = list;
        }

        public void Write(TextWriter writer, int indent)
        {
            foreach (var i in _List)
            {
                i.Write(writer, indent);
            }
        }
    }
}
