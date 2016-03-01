using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools
{
    interface EditingHitAttackBox
    {
        float Left { get; }
        float Top { get; }
        float Width { get; }
        float Height { get; }
        float Rotation { get; }
    }
}
