using GS_PatEditor.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels.Tools.Point
{
    class PointEditingHandler : EditingPoint
    {
        private readonly Editor _Editor;
        private readonly Control _Control;

        public event EventFilter Filter;

        public int _DownMouseX, _DownMouseY;

        public PointEditingHandler(Editor editor, Control ctrl)
        {
            _Editor = editor;
            _Control = ctrl;

            _Control.MouseDown += _Control_MouseDown;
            _Control.MouseUp += _Control_MouseUp;
            _Control.MouseMove += _Control_MouseMove;

            EditingPoint = -1;

            Filter += editor.PreviewWindowUI.GetFilterForEditMode(FrameEditMode.Point);
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

        private void _Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (EditingPoint != -1)
            {
                OffsetX = (int)Math.Round((e.X - _DownMouseX) / _Editor.PreviewWindowUI.PreviewMoving.PreviewScale);
                OffsetY = (int)Math.Round((e.Y - _DownMouseY) / _Editor.PreviewWindowUI.PreviewMoving.PreviewScale);
            }
            else if (CheckFilter())
            {
                var p = FindPointAt(e.X, e.Y);
                if (p != -1)
                {
                    _Control.Cursor = Cursors.Cross;
                }
                else
                {
                    _Control.Cursor = Cursors.Arrow;
                }
            }
        }

        private void _Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left) && EditingPoint != -1)
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                ValidatePoints(frame);
                frame.Points[EditingPoint].X += OffsetX;
                frame.Points[EditingPoint].Y += OffsetY;

                EditingPoint = -1;
            }
        }

        private void _Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CheckFilter())
            {
                EditingPoint = FindPointAt(e.X, e.Y);
                _DownMouseX = e.X;
                _DownMouseY = e.Y;
                OffsetX = 0;
                OffsetY = 0;
            }
        }

        private int FindPointAt(int x, int y)
        {
            const int Margin = 3;
            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            ValidatePoints(frame);
            for (int i = 0; i < 3; ++i)
            {
                var dx = _Editor.PreviewWindowUI.PreviewMoving.TransformXSpriteToClient(frame.Points[i].X) - x;
                var dy = _Editor.PreviewWindowUI.PreviewMoving.TransformYSpriteToClient(frame.Points[i].Y) - y;
                if (dx * dx + dy * dy <= Margin * Margin)
                {
                    return i;
                }
            }
            return -1;
        }

        public int EditingPoint
        {
            get;
            private set;
        }

        public int OffsetX
        {
            get;
            private set;
        }

        public int OffsetY
        {
            get;
            private set;
        }

        public IEnumerable<Pat.FramePoint> Points()
        {
            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            if (frame != null)
            {
                ValidatePoints(frame);
                yield return GetOutputPoint(frame, 0);
                yield return GetOutputPoint(frame, 1);
                yield return GetOutputPoint(frame, 2);
            }
            yield break;
        }

        private Pat.FramePoint GetOutputPoint(Pat.Frame frame, int i)
        {
            if (EditingPoint == i)
            {
                return new Pat.FramePoint
                {
                    X = frame.Points[i].X + OffsetX,
                    Y = frame.Points[i].Y + OffsetY,
                };
            }
            return frame.Points[i];
        }

        private void ValidatePoints(Pat.Frame frame)
        {
            if (frame.Points == null)
            {
                frame.Points = new List<Pat.FramePoint>();
            }
            while (frame.Points.Count < 3)
            {
                frame.Points.Add(new Pat.FramePoint());
            }
            if (frame.Points.Count > 3)
            {
                frame.Points.RemoveRange(3, frame.Points.Count - 3);
            }
        }
    }
}
