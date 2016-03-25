using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.CodeFormat
{
    public static class TextWriterExt
    {
        private static char[] _Indent = new string(' ', 64).ToCharArray();

        public static void WriteIndent(this TextWriter writer, int indent)
        {
            if (_Indent.Length < indent * 4)
            {
                _Indent = new string(' ', indent * 4).ToCharArray();
            }
            writer.Write(_Indent, 0, indent * 4);
        }
    }
}
