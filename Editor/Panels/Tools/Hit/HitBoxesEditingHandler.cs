using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Hit
{
    class HitBoxesEditingHandler
    {
        public HitBoxListDataProvider HitBoxData { get; private set; }

        public HitBoxesEditingHandler(Editor editor)
        {
            HitBoxData = new HitBoxListDataProvider(editor);
        }
    }
}
