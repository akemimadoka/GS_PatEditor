using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS_PatEditor.Pat;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.HitAttack
{
    class HitBoxesEditingHandler : HitAttackBoxesEditingHandler
    {
        public HitBoxesEditingHandler(Editor editor, Control ctrl)
            : base(editor, ctrl)
        {
        }

        protected override List<Box> GetBoxListFromFrame(Frame frame)
        {
            return frame.HitBoxes;
        }

        protected override FrameEditMode GetEditMode()
        {
            return FrameEditMode.Hit;
        }
    }
}
