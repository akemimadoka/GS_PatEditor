using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools
{
    interface EditingPoint
    {
        int OffsetX { get; }
        int OffsetY { get; }
    }

    class EmptyEditingPoint : EditingPoint
    {
        public int OffsetX
        {
            get { return 0; }
        }

        public int OffsetY
        {
            get { return 0; }
        }

        public static readonly EmptyEditingPoint Instance = new EmptyEditingPoint();
    }
}
