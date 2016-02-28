using GS_PatEditor.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.Physical
{
    interface IRectDataProvider
    {
        float Left { get; set; }
        float Top { get; set; }
        float Right { get; set; }
        float Bottom { get; set; }
    }

    enum RectPoint
    {
        None,
        LT,
        RT,
        LB,
        RB,
    }

    class PhysicalEditingHandler
    {
        private const int EditMargin = 3;
        private readonly Editor _Editor;
        private readonly Control _Control;

        private IRectDataProvider _Rect;

        public event EventFilter Filter;

        public PhysicalEditingHandler(Editor editor, Control ctrl)
        {
            _Editor = editor;
            _Control = ctrl;

            _Rect = new PhysicalDataProvider(editor);

            Filter += editor.PreviewWindowUI.GetFilterForEditMode(FrameEditMode.Physical);

            _Control.MouseMove += _Control_MouseMove;
        }

        private void _Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CheckFilter())
            {
                return;
            }
            _UpdateMouseCursor(e.X, e.Y);
        }

        private bool CheckFilter()
        {
            if (Filter == null)
            {
                return true;
            }

            bool ret = true;
            Filter(ref ret);
            return ret;
        }

        private bool CheckMargin(float diff)
        {
            return diff > -EditMargin && diff < EditMargin;
        }

        private RectPoint FindMovablePoint(int x, int y)
        {
            int result = 0;
            if (CheckMargin(x - _Rect.Left))
            {
                result += 1;
            }
            else if (CheckMargin(x - _Rect.Right))
            {
                result += 2;
            }
            if (CheckMargin(y - _Rect.Top))
            {
                result += 4;
            }
            else if (CheckMargin(y - _Rect.Bottom))
            {
                result += 8;
            }
            switch (result)
            {
                case 1 + 4: return RectPoint.LT;
                case 2 + 4: return RectPoint.RT;
                case 1 + 8: return RectPoint.LB;
                case 2 + 8: return RectPoint.RB;
                default: return RectPoint.None;
            }
        }

        private void _UpdateMouseCursor(int x, int y)
        {
            var dir = FindMovablePoint(x, y);
            switch (dir)
            {
                case RectPoint.None:
                    _Control.Cursor = Cursors.Arrow;
                    break;
                case RectPoint.LT:
                case RectPoint.RB:
                    _Control.Cursor = Cursors.SizeNWSE;
                    break;
                case RectPoint.LB:
                case RectPoint.RT:
                    _Control.Cursor = Cursors.SizeNESW;
                    break;
            }
        }

        public void UpdateMouseCursor()
        {
            if (!CheckFilter())
            {
                return;
            }
            var pos = _Control.PointToClient(Control.MousePosition);
            _UpdateMouseCursor(pos.X, pos.Y);
        }
    }
}
