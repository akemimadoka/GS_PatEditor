using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools
{
    interface IRectDataProvider
    {
        float Left { get; set; }
        float Top { get; set; }
        float Right { get; set; }
        float Bottom { get; set; }
    }

    class MouseRectEditable
    {
        private readonly Control _Control;
        private readonly IRectDataProvider _Rect;

        public event EventFilter Filter;

        public MouseRectEditable(Control ctrl, IRectDataProvider rect)
        {
            _Control = ctrl;
            _Rect = rect;

            ctrl.MouseMove += _Control_MouseMove;
            ctrl.MouseLeave += _Control_MouseLeave;
        }

        void _Control_MouseLeave(object sender, EventArgs e)
        {
            if (TestFilter())
            {
                _Control.Cursor = Cursors.Arrow;
            }
        }

        private void _Control_MouseMove(object sender, MouseEventArgs e)
        {
            _UpdateMouseCursor(e.X, e.Y);
        }

        private bool TestFilter()
        {
            if (Filter == null)
            {
                return true;
            }

            bool result = true;
            Filter(ref result);
            return result;
        }

        private void _UpdateMouseCursor(int x, int y)
        {
            if (!TestFilter())
            {
                return;
            }

            _Control.Cursor = Cursors.Arrow;
            {
                var xdiff = x - _Rect.Left;
                if (xdiff > -2 && xdiff < 2)
                {
                    _Control.Cursor = Cursors.SizeWE;
                }
            }
            {
                var xdiff = x - _Rect.Right;
                if (xdiff > -2 && xdiff < 2)
                {
                    _Control.Cursor = Cursors.SizeWE;
                }
            }
            {
                var ydiff = y - _Rect.Top;
                if (ydiff > -2 && ydiff < 2)
                {
                    _Control.Cursor = Cursors.SizeNS;
                }
            }
            {
                var ydiff = y - _Rect.Bottom;
                if (ydiff > -2 && ydiff < 2)
                {
                    _Control.Cursor = Cursors.SizeNS;
                }
            }
        }

        public void UpdateMouseCursor()
        {
            var pos = _Control.PointToClient(Control.MousePosition);
            _UpdateMouseCursor(pos.X, pos.Y);
        }
    }
}
