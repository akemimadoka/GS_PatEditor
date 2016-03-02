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

        public HitBoxesEditingHandler(Editor editor, Control ctrl)
        {
            _Editor = editor;
            _Control = ctrl;

            Filter += editor.PreviewWindowUI.GetFilterForEditMode(FrameEditMode.Hit);

            HitBoxData = new HitBoxListDataProvider(editor);

            editor.EditorNode.Animation.Frame.EditModeChanged += ToolSwitched;

            _Control.MouseMove += _Control_MouseMove;
            _Control.MouseDown += _Control_MouseDown;
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

        private bool IsInPointRange(Point p, int x, int y)
        {
            const float MaxDistance = 3;

            var dx = p.X - x;
            var dy = p.Y - y;
            return (dx * dx + dy * dy < MaxDistance * MaxDistance);
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

        private void UpdateMouseCursor(int x, int y)
        {
            if (_SelectedSingle == null && _SelectedMultiple.Count == 0)
            {
                if (FindBoxAt(x, y) == null)
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
                if (Control.ModifierKeys.HasFlag(Keys.Control) && FindBoxAt(x, y) != null)
                {
                    _Control.Cursor = Cursors.Hand;
                }
                else if (FindPointAt(_SelectedSingle, x, y) != RectPoint.None)
                {
                    _Control.Cursor = Cursors.SizeAll;
                }
                else
                {
                    _Control.Cursor = Cursors.Arrow;
                }
            }
            else
            {
                if (Control.ModifierKeys.HasFlag(Keys.Control) && FindBoxAt(x, y) != null)
                {
                    _Control.Cursor = Cursors.Hand;
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
            if (!CheckFilter())
            {
                return;
            }
            
            if (Control.ModifierKeys.HasFlag(Keys.Control))
            {
                var box = FindBoxAt(e.X, e.Y);
                if (box != null)
                {
                    AppendSelected(box);
                }
                return;
            }

            if (_SelectedSingle == null && _SelectedMultiple.Count == 0)
            {
                var box = FindBoxAt(e.X, e.Y);
                if (box != null)
                {
                    SetSingleSelected(box);
                }
            }
            else if (_SelectedSingle != null && _SelectedMultiple.Count == 1)
            {
                var box = _SelectedSingle;
                switch (FindPointAt(box, e.X, e.Y))
                {
                    case RectPoint.None:
                        ClearSelected();
                        break;
                    //TODO set down state
                }
            }
            else
            {
                var box = FindBoxAt(e.X, e.Y);
                if (box == null)
                {
                    ClearSelected();
                }
                else if (box.IsSelected)
                {
                    //TODO begin move
                }
                else
                {
                    SetSingleSelected(box);
                }
            }
        }
    }
}
