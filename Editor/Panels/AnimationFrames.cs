using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels
{
    class AnimationFrames
    {
        #region Frame Grid
        private const int FrameGridSize = 64;

        [Flags]
        enum KeyFrameFlags
        {
            IsKeyFrame = 1,
            HasDamage = 2,
            HasCancellable = 4,
        }

        abstract class AbstractGrid
        {
            public abstract void Render(Graphics g);
            public virtual int Width { get { return FrameGridSize; } }
        }
        class EmptyGrid : AbstractGrid
        {
            private Pen pen = new Pen(Color.Black);

            public override void Render(Graphics g)
            {
                g.DrawRectangle(pen, 0, 0, FrameGridSize, FrameGridSize);
            }
        }
        abstract class ImageGrid : AbstractGrid
        {
            private Image _Image;
            private PointF _Position;
            private string _Index;
            private Brush _TextBrush = new SolidBrush(Color.Black);

            public ImageGrid(Bitmap image, int? index)
            {
                float ratio = 1.0f * image.Width / image.Height;

                if (ratio > 1.0f)
                {
                    _Image = image.GetThumbnailImage(FrameGridSize, (int)(FrameGridSize / ratio), null, IntPtr.Zero);
                    _Position = new PointF(0, (FrameGridSize - (FrameGridSize / ratio)) / 2);
                }
                else
                {
                    _Image = image.GetThumbnailImage((int)(FrameGridSize * ratio), FrameGridSize, null, IntPtr.Zero);
                    _Position = new PointF((FrameGridSize - (FrameGridSize * ratio)) / 2, 0);
                }

                _Index = index.HasValue ? index.ToString() : null;
            }
            protected void DrawImage(Graphics g)
            {
                if (_Image != null)
                {
                    g.DrawImage(_Image, _Position);
                }
                if (_Index != null)
                {
                    g.DrawString(_Index, SystemFonts.DefaultFont, _TextBrush, new RectangleF(0, 0, FrameGridSize / 2, FrameGridSize / 2));
                }
            }
        }
        class KeyFrameGrid : ImageGrid
        {
            private Pen pen = new Pen(Color.Black);
            private Brush brush = new SolidBrush(Color.FromArgb(180, 180, 180));
            private Brush brushSelected = new SolidBrush(Color.FromArgb(120, 120, 120));
            private Brush brushKey = new SolidBrush(Color.FromArgb(222, 222, 222));
            private Brush brushDamage = new SolidBrush(Color.FromArgb(222, 36, 18));
            public readonly int Segment, Frame;
            private readonly KeyFrameFlags _Flags;

            public KeyFrameGrid(Bitmap bitmap, int index, int segIndex, int frameIndex, KeyFrameFlags flags)
                : base(bitmap, index)
            {
                Segment = segIndex;
                Frame = frameIndex;
                _Flags = flags;

                IsFolded = true;
            }

            public override void Render(Graphics g)
            {
                g.FillRectangle(IsSelected ? brushSelected : brush, 0, 0, FrameGridSize, FrameGridSize);
                g.DrawRectangle(pen, 0, 0, FrameGridSize, FrameGridSize);
                DrawImage(g);
                if (_Flags.HasFlag(KeyFrameFlags.IsKeyFrame))
                {
                    g.FillEllipse(brushKey,
                        FrameGridSize * 0.02f, FrameGridSize * 0.25f,
                        FrameGridSize * 0.10f, FrameGridSize * 0.10f);
                }
                if (_Flags.HasFlag(KeyFrameFlags.HasDamage))
                {
                    g.FillEllipse(brushDamage,
                        FrameGridSize * 0.02f, FrameGridSize * 0.40f,
                        FrameGridSize * 0.10f, FrameGridSize * 0.10f);
                }
            }

            public bool IsSelected { get; set; }
            public bool IsFolded { get; set; }
        }
        class NormalFrameGrid : ImageGrid
        {
            private Pen pen = new Pen(Color.Black);
            private Brush brush = new SolidBrush(Color.FromArgb(215, 215, 215));
            public readonly KeyFrameGrid KeyFrame;

            public NormalFrameGrid(KeyFrameGrid key, Bitmap bitmap, int index)
                : base(bitmap, index)
            {
                KeyFrame = key;
            }

            public override void Render(Graphics g)
            {
                if (Visible)
                {
                    g.FillRectangle(brush, 0, 0, FrameGridSize, FrameGridSize);
                    g.DrawRectangle(pen, 0, 0, FrameGridSize, FrameGridSize);
                    DrawImage(g);
                }
                else
                {
                    g.FillRectangle(brush, 1, 0, 2, FrameGridSize);
                    g.DrawLine(pen, 0, 0, 3, 0);
                    g.DrawLine(pen, 0, FrameGridSize, 3, FrameGridSize);
                }
            }

            public override int Width
            {
                get
                {
                    return Visible ? FrameGridSize : 3;
                }
            }

            public bool Visible { get { return !KeyFrame.IsFolded; } }
        }
        #endregion

        private readonly Editor _Parent;
        private Control _Control;
        private readonly List<AbstractGrid> _GridList = new List<AbstractGrid>();
        private KeyFrameGrid _LastSelected;

        public AnimationFrames(Editor parent)
        {
            _Parent = parent;

            parent.EditorNode.Animation.OnReset += Animation_OnReset;

            RefreshList();
        }

        public void Init(Control ctrl)
        {
            this._Control = ctrl;

            ctrl.Paint += ctrl_Paint;
            ctrl.Click += ctrl_Click;
            ctrl.DoubleClick += ctrl_DoubleClick;

            UpdateControlWidth();

            ctrl.FindForm().MouseWheel += _Control_MouseWheel;
            ctrl.MouseWheel += _Control_MouseWheel;
        }

        private void _Control_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!_Control.IsMouseInRange())
            {
                return;
            }
            ScrollableControl p = _Control.Parent as ScrollableControl;
            if (p != null)
            {
                var point = p.AutoScrollPosition;
                point.X = -point.X + (e.Delta / -6);
                p.AutoScrollPosition = point;
            }
        }

        void ctrl_DoubleClick(object sender, EventArgs e)
        {
            Point pos = _Control.PointToClient(Control.MousePosition);
            var grid = GetGridFromPoint(pos.X);
            if (grid == null)
            {
                return;
            }

            if (grid is KeyFrameGrid)
            {
                var keyGrid = (KeyFrameGrid)grid;
                keyGrid.IsFolded = !keyGrid.IsFolded;
                UpdateControlWidth();
                _Control.Invalidate();
            }
            else if (grid is NormalFrameGrid)
            {
                var normalGrid = (NormalFrameGrid)grid;
                normalGrid.KeyFrame.IsFolded = !normalGrid.KeyFrame.IsFolded;
                UpdateControlWidth();
                _Control.Invalidate();
            }
        }

        private void UpdateControlWidth()
        {
            if (_Control != null)
            {
                _Control.Width = _GridList.Sum(g => g.Width) + FrameGridSize * 0 + 1;
            }
        }

        private void ctrl_Click(object sender, EventArgs e)
        {
            Point pos = _Control.PointToClient(Control.MousePosition);
            var grid = GetGridFromPoint(pos.X);
            if (grid == null)
            {
                return;
            }

            if (grid is KeyFrameGrid)
            {
                var keyGrid = (KeyFrameGrid)grid;
                SelectKeyGrid(keyGrid);
            }
            else if (grid is NormalFrameGrid)
            {
                var normalGrid = (NormalFrameGrid)grid;
                SelectKeyGrid(normalGrid.KeyFrame);
            }
        }

        private void SelectKeyGrid(KeyFrameGrid grid)
        {
            if (_LastSelected == grid)
            {
                return;
            }
            if (_LastSelected != null)
            {
                _LastSelected.IsSelected = false;
            }
            if (grid != null)
            {
                grid.IsSelected = true;
                _Parent.EditorNode.Animation.SetSelectedFrame(grid.Segment, grid.Frame);
            }
            else
            {
                //clear selected
                _Parent.EditorNode.Animation.SetSelectedFrame(-1, -1);
            }
            _LastSelected = grid;

            if (_Control != null)
            {
                _Control.Invalidate();
            }
        }

        private AbstractGrid GetGridFromPoint(int x)
        {
            int sum = 0;
            foreach (var g in _GridList)
            {
                sum += g.Width;
                if (sum > x)
                {
                    return g;
                }
            }
            return null;
        }

        private void ctrl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (var g in _GridList)
            {
                g.Render(e.Graphics);
                e.Graphics.TranslateTransform(g.Width, 0);
                e.Graphics.DrawLine(Pens.Black, 0, 0, 0, FrameGridSize);
            }
        }

        private void Animation_OnReset()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            _GridList.Clear();

            var data = _Parent.EditorNode.Animation.Data;
            if (data == null)
            {
                UpdateControlWidth();
                SelectKeyGrid(null);
                return;
            }

            var imageList = _Parent.Data.ImageList;
            for (int i = 0; i < data.Segments.Count; ++i)
            {
                var seg = data.Segments[i];
                for (int j = 0; j < seg.Frames.Count; ++j)
                {
                    var frame = seg.Frames[j];
                    var image = imageList.GetImage(frame.ImageID);
                    KeyFrameFlags flags = 0;
                    if (j == 0)
                    {
                        flags |= KeyFrameFlags.IsKeyFrame;
                    }
                    if (seg.Damage != null)
                    {
                        flags |= KeyFrameFlags.HasDamage;
                    }
                    var keyFrame = new KeyFrameGrid(image, _GridList.Count, i, j, flags);
                    _GridList.Add(keyFrame);
                    for (int k = 0; k < frame.Duration - 1; ++k)
                    {
                        _GridList.Add(new NormalFrameGrid(keyFrame, image, _GridList.Count));
                    }
                }
            }

            UpdateControlWidth();

            if (_GridList.Count > 0 && _GridList[0] is KeyFrameGrid)
            {
                SelectKeyGrid((KeyFrameGrid)_GridList[0]);
            }
            else
            {
                SelectKeyGrid(null);
            }
        }

        public void CollapseAll()
        {
            foreach (var g in _GridList)
            {
                if (g is KeyFrameGrid)
                {
                    ((KeyFrameGrid)g).IsFolded = true;
                }
            }
            UpdateControlWidth();
            if (_Control != null)
            {
                _Control.Invalidate();
            }
        }

        public void ExpandAll()
        {
            foreach (var g in _GridList)
            {
                if (g is KeyFrameGrid)
                {
                    ((KeyFrameGrid)g).IsFolded = false;
                }
            }
            UpdateControlWidth();
            if (_Control != null)
            {
                _Control.Invalidate();
            }
        }

        #region ui event

        public void SetCurrentToKeyFrame()
        {
            if (_LastSelected != null && _LastSelected is KeyFrameGrid)
            {
                var grid = (KeyFrameGrid)_LastSelected;
                var segmentIndex = grid.Segment;
                var frameIndex = grid.Frame;
                if (frameIndex == 0)
                {
                    //already a key frame
                    return;
                }

                var animation = _Parent.EditorNode.Animation.Data;
                if (animation == null)
                {
                    return;
                }

                //split a segment
                var oldSegment = animation.Segments[segmentIndex];
                var newSegment = new Pat.AnimationSegment()
                {
                    IsLoop = oldSegment.IsLoop,
                    Frames = oldSegment.Frames.Skip(frameIndex).ToList(),
                };
                oldSegment.Frames = oldSegment.Frames.Take(frameIndex).ToList();

                //TODO check cancellable data in oldSegment

                animation.Segments.Insert(segmentIndex + 1, newSegment);

                RefreshList();
            }
        }

        public void SetCurrentToNormalFrame()
        {
            if (_LastSelected != null && _LastSelected is KeyFrameGrid)
            {
                var grid = (KeyFrameGrid)_LastSelected;
                var segmentIndex = grid.Segment;
                if (grid.Frame != 0)
                {
                    //not a key frame
                    return;
                }
                if (segmentIndex == 0)
                {
                    MessageBox.Show("The first frame must be key frame.", "AnimationEditor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var animation = _Parent.EditorNode.Animation.Data;
                if (animation == null)
                {
                    return;
                }

                //split a segment
                var lastSegment = animation.Segments[segmentIndex - 1];
                var theSegment = animation.Segments[segmentIndex];
                lastSegment.Frames.AddRange(theSegment.Frames);

                //TODO check cancellable data in oldSegment

                animation.Segments.RemoveAt(segmentIndex);

                RefreshList();
            }
        }

        public void SwitchCurrentLoop()
        {
            if (_LastSelected != null && _LastSelected is KeyFrameGrid)
            {
                var grid = (KeyFrameGrid)_LastSelected;
                var segmentIndex = grid.Segment;
                if (grid.Frame != 0)
                {
                    //not a key frame
                    return;
                }

                var animation = _Parent.EditorNode.Animation.Data;
                if (animation == null)
                {
                    return;
                }

                //split a segment
                var theSegment = animation.Segments[segmentIndex];
                theSegment.IsLoop = !theSegment.IsLoop;

                RefreshList();
            }
        }

        public void ShowEditFrameForm()
        {
            if (_LastSelected != null && _LastSelected is KeyFrameGrid)
            {
                var grid = (KeyFrameGrid)_LastSelected;

                var animation = _Parent.EditorNode.Animation.Data;
                if (animation == null)
                {
                    return;
                }

                var frame = animation.Segments[grid.Segment].Frames[grid.Frame];

                var dialog = new FrameEditForm()
                {
                    UseImage = frame.ImageID == animation.ImageID,
                    FrameCount = frame.Duration,
                    SetDuationForAllEnabled = grid.Frame == 0,
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SetDurationForAll)
                    {
                        foreach (var f in animation.Segments[grid.Segment].Frames)
                        {
                            f.Duration = dialog.FrameCount;
                        }
                    }
                    else
                    {
                        frame.Duration = dialog.FrameCount;
                    }
                    if (frame.ImageID == animation.ImageID && !dialog.UseImage)
                    {
                        animation.ImageID = null;
                    }
                    else if (dialog.UseImage)
                    {
                        animation.ImageID = frame.ImageID;
                    }
                }

                RefreshList();
            }
        }
        #endregion
    }
}
