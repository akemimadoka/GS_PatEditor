using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor
{
    class Editor
    {
        public EditorUI UI { get; private set; }
        public Pat.Project Data { get; private set; }

        public Editor()
        {
            UI = new EditorUI(this);
        }
    }
}
