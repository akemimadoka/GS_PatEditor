using GS_PatEditor.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.Hit
{
    enum RectPoint
    {
        None,
        LT,
        RT,
        LB,
        RB,
    }

    class HitBoxesEditingHandler
    {
        private Editor _Editor;
        private Control _Control;

        public event EventFilter Filter;

        public HitBoxListDataProvider HitBoxData { get; private set; }

        private HitBoxDataProvider _SelectedSingle;
        private List<HitBoxDataProvider> _SelectedMultiple = new List<HitBoxDataProvider>();

        private RectPoint _EditingSingleRectPoint = RectPoint.None;

        public HitBoxesEditingHandler(Editor editor, Control ctrl)
        {
            _Editor = editor;
            _Control = ctrl;

            Filter += editor.PreviewWindowUI.GetFilterForEditMode(FrameEditMode.Hit);

            HitBoxData = new HitBoxListDataProvider(editor);

            editor.EditorNode.Animation.Frame.EditModeChanged += ToolSwitched;

            _Control.MouseMove += _Control_MouseMove;
            _Control.MouseDown += _Control_MouseDown;
            _Control.MouseUp += _Control_MouseUp;
        }

        //called when tool is closed
        public void ToolSwitched()
        {
            ClearSelected();
        }

        private void ClearSelected()
        {
            //clear previous selected
            foreach (var prev in _SelectedMultiple)
            {
                prev.IsSelected = false;
            }

            _SelectedSingle = null;
            _SelectedMultiple.Clear();
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

        #region Find Rect

        private bool IsInPointRange(Point p, int x, int y)
        {
            const float MaxDistance = 3;

            var dx = p.X - x;
            var dy = p.Y - y;
            return (dx * dx + dy * dy < MaxDistance * MaxDistance);
        }

        private bool IsInBorderRange(Point p1, Point p2, int x, int y)
        {
            if (Math.Abs(p1.X - p2.X) > Math.Abs(p1.Y - p2.Y))
            {
                return IsInBorderRange_Checked(p1.X, p2.X, p1.Y, p2.Y, x, y);
            }
            else
            {
                return IsInBorderRange_Checked(p1.Y, p2.Y, p1.X, p2.X, y, x);
            }
        }

        private bool IsInBorderRange_Checked(float x1, float x2, float y1, float y2, int x, int y)
        {
            const float MaxDistance = 3;
            if (x < x1 - MaxDistance && x < x2 - MaxDistance)
            {
                return false;
            }
            if (x > x1 + MaxDistance && x > x2 + MaxDistance)
            {
                return false;
            }
            if (Math.Abs(x1 - x2) < MaxDistance)
            {
                return IsInPointRange(new Point(x1, y1), x, y) ||
                    IsInPointRange(new Point(x2, y2), x, y);
            }
            var y0 = (x - x1) / (x2 - x1) * (y2 - y1) + y1;
            return Math.Abs(y0 - y) < MaxDistance;
        }

        private HitBoxDataProvider FindBoxAt(int x, int y)
        {
            foreach (var box in HitBoxData.DataList)
            {
                if (IsInPointRange(box.LeftTop, x, y) ||
                    IsInPointRange(box.LeftBottom, x, y) ||
                    IsInPointRange(box.RightTop, x, y) ||
                    IsInPointRange(box.RightBottom, x, y))
                {
                    return box;
                }
            }
            return null;
        }

        private HitBoxDataProvider FindBoxAtEdge(int x, int y)
        {
            foreach (var box in HitBoxData.DataList)
            {
                if (IsInBorderRange(box.LeftTop, box.RightTop, x, y) ||
                    IsInBorderRange(box.RightBottom, box.RightTop, x, y) ||
                    IsInBorderRange(box.RightBottom, box.LeftBottom, x, y) ||
                    IsInBorderRange(box.LeftTop, box.LeftBottom, x, y))
                {
                    return box;
                }
            }
            return null;
        }

        private RectPoint FindPointAt(HitBoxDataProvider box, int x, int y)
        {
            if (IsInPointRange(box.LeftTop, x, y))
            {
                return RectPoint.LT;
            }
            if (IsInPointRange(box.LeftBottom, x, y))
            {
                return RectPoint.LB;
            }
            if (IsInPointRange(box.RightTop, x, y))
            {
                return RectPoint.RT;
            }
            if (IsInPointRange(box.RightBottom, x, y))
            {
                return RectPoint.RB;
            }
            return RectPoint.None;
        }

        #endregion
        
        private void UpdateMouseCursor(int x, int y)
        {
            if (_SelectedSingle == null && _SelectedMultiple.Count == 0)
            {
                if (FindBoxAtEdge(x, y) == null)
                {
                    //TODO check default cursor: use Cursors.Default
                    _Control.Cursor = Cursors.Arrow;
                }
                else
                {
                    _Control.Cursor = Cursors.Hand;
                }
            }
            else if (_SelectedSingle != null && _SelectedMultiple.Count == 1)
            {
                var box = FindBoxAtEdge(x, y);
                if (Control.ModifierKeys.HasFlag(Keys.Control) && box != null)
                {
                    _Control.Cursor = Cursors.Hand;
                }
                else if (box == _SelectedSingle)
                {
                    if (FindPointAt(_SelectedSingle, x, y) != RectPoint.None)
                    {
                        _Control.Cursor = Cursors.Cross;
                    }
                    else
                    {
                    _Control.Cursor = Cursors.SizeAll;
                    }
                }
                else
                {
                    _Control.Cursor = Cursors.Arrow;
                }
            }
            else
            {
                var box = FindBoxAtEdge(x, y);
                if (Control.ModifierKeys.HasFlag(Keys.Control) && box != null)
                {
                    _Control.Cursor = Cursors.Hand;
                }
                else if (box != null && box.IsSelected)
                {
                    _Control.Cursor = Cursors.SizeAll;
                }
                else
                {
                    _Control.Cursor = Cursors.Arrow;
                }
            }
        }

        private void SetSingleSelected(HitBoxDataProvider box)
        {
            ClearSelected();

            _SelectedSingle = box;

            _SelectedMultiple.Clear();
            _SelectedMultiple.Add(box);

            box.IsSelected = true;
        }

        private void AppendSelected(HitBoxDataProvider box)
        {
            if (box.IsSelected)
            {
                //remove
                box.IsSelected = false;
                _SelectedMultiple.Remove(box);
                _SelectedSingle = _SelectedMultiple.LastOrDefault();
            }
            else
            {
                //add
                box.IsSelected = true;
                _SelectedMultiple.Add(box);
                _SelectedSingle = box;
            }
        }

        void _Control_MouseMove(object sender, MouseEventArgs e)
        {
            //TODO process edit
            if (_EditingSingleRectPoint != RectPoint.None)
            {
                var p = new Point(e.X, e.Y);
                switch (_EditingSingleRectPoint)
                {
                    case RectPoint.LT:
                        _SelectedSingle.LeftTop = p;
                        break;
                    case RectPoint.LB:
                        _SelectedSingle.LeftBottom = p;
                        break;
                    case RectPoint.RT:
                        _SelectedSingle.RightTop = p;
                        break;
                    case RectPoint.RB:
                        _SelectedSingle.RightBottom = p;
                        break;
                    default:
                        break;
                }
                return;
            }

            //check filter
            if (!CheckFilter())
            {
                return;
            }

            //update cursor
            UpdateMouseCursor(e.X, e.Y);
        }

        void _Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CheckFilter() || e.Button != MouseButtons.Left)
            {
                return;
            }
            
            if (Control.ModifierKeys.HasFlag(Keys.Control))
            {
                var box = FindBoxAtEdge(e.X, e.Y);
                if (box != null)
                {
                    AppendSelected(box);
                }
                return;
            }

            if (_SelectedSingle == null && _SelectedMultiple.Count == 0)
            {
                var box = FindBoxAtEdge(e.X, e.Y);
                if (box != null)
                {
                    SetSingleSelected(box);
                }
            }
            else if (_SelectedSingle != null && _SelectedMultiple.Count == 1)
            {
                var box = FindBoxAtEdge(e.X, e.Y);
                var boxpoint = FindPointAt(_SelectedSingle, e.X, e.Y);
                if (box == _SelectedSingle && boxpoint == RectPoint.None)
                {
                    //begin move
                }
                switch (boxpoint)
                {
                    case RectPoint.None:
                        ClearSelected();
                        break;
                    default:
                        _EditingSingleRectPoint = boxpoint;
                        _SelectedSingle.IsEditing = true;
                        break;
                }
            }
            else
            {
                var box = FindBoxAtEdge(e.X, e.Y);
                if (box == null)
                {
                    ClearSelected();
                }
                else if (box != null && box.IsSelected)
                {
                    //TODO begin move
                }
                else
                {
                    ClearSelected();
                    //SetSingleSelected(box);
                }
            }
        }

        void _Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (_EditingSingleRectPoint != RectPoint.None)
                {
                    _EditingSingleRectPoint = RectPoint.None;
                    _SelectedSingle.IsEditing = false;
                }
            }
        }
    }
}
