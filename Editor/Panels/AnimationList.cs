using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels
{
    class AnimationList
    {
        private readonly Editor _Parent;
        private Control _Control;

        private readonly List<AnimationListItem> _Items = new List<AnimationListItem>();

        public AnimationList(Editor parent)
        {
            _Parent = parent;

            RefreshList();
        }

        public void Init(Control ctrl)
        {
            _Control = ctrl;

            ctrl.Paint += ctrl_Paint;
            ctrl.Parent.Resize += Parent_Resize;

            ctrl.Height = _Items.Sum(item => (int)item.Height) + 1;
        }

        void Parent_Resize(object sender, EventArgs e)
        {
            _Control.Invalidate();
        }

        void ctrl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            foreach (var a in _Items)
            {
                a.Render(e.Graphics);
                e.Graphics.TranslateTransform(0, a.Height);
            }
        }

        private void RefreshList()
        {
            foreach (var a in _Parent.Data.Animations)
            {
                _Items.Add(CreateItemForAnimation(a));
            }
            if (_Control != null)
            {
                _Control.Height = _Items.Sum(item => (int)item.Height) + 1;
            }
        }

        private AnimationListItem CreateItemForAnimation(Pat.Animation animation)
        {
            var img = _Parent.Data.ImageList.GetImage(animation.ImageID);
            var frameCount = animation.Segments.Sum(s => s.Frames.Count);
            var desc = animation.Segments.Count.ToString() + " segment(s), " +
                frameCount.ToString() + " frame(s).";
            return new AnimationListItem(img, animation.AnimationID, desc);
        }
    }
}
