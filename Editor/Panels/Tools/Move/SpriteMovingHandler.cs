using GS_PatEditor.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.Move
{
    class SpriteMovingHandler : EditingPoint
    {
        private int SpriteMovingX, SpriteMovingY;

        public SpriteMovingHandler(Editor editor, Control ctrl)
        {
            var window = editor.PreviewWindowUI;

            var move = new MouseMovable(ctrl, MouseButtons.Left, 0, 0);
            move.FilterMouseDown += window.GetFilterForEditMode(FrameEditMode.Move);
            move.OnMovedDiff += delegate(int x, int y)
            {
                SpriteMovingX = (int)(-x / window.PreviewMoving.PreviewScale);
                SpriteMovingY = (int)(-y / window.PreviewMoving.PreviewScale);
            };
            move.OnMoveFinished += delegate()
            {
                var node = editor.EditorNode.Animation.Frame;
                node.FrameData.OriginX += SpriteMovingX;
                node.FrameData.OriginY += SpriteMovingY;
                SpriteMovingX = 0;
                SpriteMovingY = 0;
            };
        }

        public int OffsetX
        {
            get { return SpriteMovingX; }
        }

        public int OffsetY
        {
            get { return SpriteMovingY; }
        }
    }
}
