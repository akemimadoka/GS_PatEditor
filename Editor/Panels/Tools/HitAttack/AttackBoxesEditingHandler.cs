using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS_PatEditor.Pat;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.HitAttack
{
    class AttackBoxesEditingHandler : HitAttackBoxesEditingHandler
    {
        public AttackBoxesEditingHandler(Editor editor, Control ctrl)
            : base(editor, ctrl)
        {
        }

        protected override List<Box> GetBoxListFromFrame(Frame frame)
        {
            return frame.AttackBoxes;
        }

        protected override FrameEditMode GetEditMode()
        {
            return FrameEditMode.Attack;
        }
    }
}
