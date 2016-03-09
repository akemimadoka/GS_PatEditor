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
        private AnimationListItem _SelectedItem;

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

            ctrl.Click += ctrl_Click;
        }

        private void ctrl_Click(object sender, EventArgs e)
        {
            var pos = _Control.PointToClient(Control.MousePosition);

            //find the item
            var item = FindItem(pos);
            if (_SelectedItem != null)
            {
                _SelectedItem.IsSelected = false;
            }
            _SelectedItem = item;
            if (item != null)
            {
                item.IsSelected = true;
            }
            OnSelectChange();
        }

        private void OnSelectChange()
        {
            if (SelectedChange != null)
            {
                SelectedChange();
            }

            _Control.Invalidate();
        }

        private AnimationListItem FindItem(Point pos)
        {
            var height = 0.0f;
            foreach (var a in _Items)
            {
                height += a.Height;
                if (pos.Y < height)
                {
                    return a;
                }
            }
            return null;
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            _Control.Invalidate();
        }

        private void ctrl_Paint(object sender, PaintEventArgs e)
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
            _Items.Clear();

            var list = _Parent.Data.Animations;
            for (int i = 0; i < list.Count; ++i)
            {
                var a = list[i];
                _Items.Add(CreateItemForAnimation(a,  i));
            }

            if (_Control != null)
            {
                _Control.Height = _Items.Sum(item => (int)item.Height) + 1;
                _Control.Invalidate();
            }
            if (SelectedChange != null)
            {
                SelectedChange();
            }
        }

        private AnimationListItem CreateItemForAnimation(Pat.Animation animation, int index)
        {
            var img = animation.ImageID != null ?
                _Parent.Data.ImageList.GetImage(animation.ImageID) :
                null;
            var frameCount = animation.Segments.Sum(s => s.Frames.Count);
            var desc = animation.Segments.Count.ToString() + " segment(s), " +
                frameCount.ToString() + " frame(s).";
            return new AnimationListItem(img, animation.AnimationID, desc, index);
        }

        public void Activate()
        {
            //if (SelectedChange != null)
            //{
            //    SelectedChange();
            //}
            //animation information might be modified, refresh
            RefreshList();
        }

        #region toolbar events

        public event Action SelectedChange;

        public bool HasSelected
        {
            get
            {
                return _SelectedItem != null;
            }
        }

        public void EditCurrent()
        {
            if (_SelectedItem != null)
            {
                _Parent.EditorNode.SelectedAnimationIndex = _SelectedItem.Index;
                _Parent.ShowAnimationUI();
            }
        }

        public void RemoveCurrent()
        {
            if (_SelectedItem != null)
            {
                _Parent.Data.Animations.RemoveAt(_SelectedItem.Index);
                _SelectedItem.IsSelected = false;
                _SelectedItem = null;

                RefreshList();
            }
        }

        public void AddNew()
        {
            var animation = new Pat.Animation();

            //find an available name
            int id = 1;
            {
                var list = _Parent.Data.Animations;
                while (list.Any(a => a.AnimationID == "New Animation " + id.ToString()))
                {
                    ++id;
                }
            }
            animation.AnimationID = "New Animation " + id.ToString();

            animation.Segments = new List<Pat.AnimationSegment>();

            _Parent.Data.Animations.Add(animation);

            if (_SelectedItem != null)
            {
                _SelectedItem.IsSelected = false;
                _SelectedItem = null;
            }

            RefreshList();

            _SelectedItem = _Items.Last();
            _SelectedItem.IsSelected = true;
            OnSelectChange();
        }

        public void EditProperty()
        {
            if (_SelectedItem != null)
            {
                var dialog = new AnimationPropertyFrom();
                
                var id = _SelectedItem.Index;

                var currentAnimation = _Parent.Data.Animations[id];
                dialog.AnimationID = currentAnimation.AnimationID;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentAnimation.AnimationID = dialog.AnimationID;

                    _SelectedItem.IsSelected = false;
                    _SelectedItem = null;

                    RefreshList();

                    _SelectedItem = _Items[id];
                    _SelectedItem.IsSelected = true;

                    OnSelectChange();
                }
            }
        }

        #endregion
    }
}
