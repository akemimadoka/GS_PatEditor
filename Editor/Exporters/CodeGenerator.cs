using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters
{
    public class CodeGenerator
    {
        private TextWriter _Writer;
        private static UTF8Encoding _Encoding = new UTF8Encoding(false);

        public CodeGenerator(string filename)
        {
            _Writer = new StreamWriter(filename, false, _Encoding);
        }

        public void Finish()
        {
            _Writer.Close();
        }

        public void WriteStatement(ILineObject s)
        {
            s.Write(_Writer, 0);
        }
    }
}
