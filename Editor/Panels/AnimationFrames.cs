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
        abstract class AbstractGrid
        {
            public abstract void Render(Graphics g);
        }
        class EmptyGrid : AbstractGrid
        {
            private Pen pen = new Pen(Color.Black);

            public override void Render(Graphics g)
            {
                g.DrawRectangle(pen, 0, 0, 16, 16);
                g.TranslateTransform(16, 0);
            }
        }
        class KeyFrame : AbstractGrid
        {
            private Pen pen = new Pen(Color.Black);
            public Image Image;

            public override void Render(Graphics g)
            {
                g.DrawRectangle(pen, 0, 0, 16, 16);
                if (Image != null)
                {
                    g.DrawImage(Image, new PointF(0.0f, 0.0f));
                }

                g.TranslateTransform(16, 0);
            }
        }
        #endregion

        private readonly Editor _Parent;
        private readonly List<AbstractGrid> _GridList = new List<AbstractGrid>();

        public AnimationFrames(Editor parent)
        {
            _Parent = parent;

            parent.EditorNode.Animation.OnReset += Animation_OnReset;

            RefreshList();
        }

        public void Init(Control ctrl)
        {
            ctrl.Paint += ctrl_Paint;
        }

        private void ctrl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (var g in _GridList)
            {
                g.Render(e.Graphics);
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
                return;
            }
            var f0 = data.Segments[0].Frames[0];
            
            _GridList.Add(new EmptyGrid());
            _GridList.Add(new EmptyGrid());
        }
    }
}
