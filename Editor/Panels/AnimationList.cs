﻿using System;
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
            _Parent.ProjectReset += RefreshList;
        }

        public void Init(Control ctrl)
        {
            _Control = ctrl;

            ctrl.Paint += ctrl_Paint;
            ctrl.Parent.Resize += Parent_Resize;

            ctrl.Height = _Items.Sum(item => (int)item.Height) + 1;

            ctrl.Click += ctrl_Click;

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
                point.Y = -point.Y + (e.Delta / -6);
                p.AutoScrollPosition = point;
            }
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
                a.Render(e.Graphics, _Control.ClientSize.Width);
                e.Graphics.TranslateTransform(0, a.Height);
            }
        }

        private void RefreshList()
        {
            var lastSelectedAction = _SelectedItem == null ? null : _SelectedItem.Object;

            _Items.Clear();

            var list = _Parent.Project.Actions;
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

            _SelectedItem = _Items.FirstOrDefault(i => i.Object == lastSelectedAction);
            if (_SelectedItem != null)
            {
                _SelectedItem.IsSelected = true;
            }

            if (SelectedChange != null)
            {
                SelectedChange();
            }
        }

        private AnimationListItem CreateItemForAnimation(Pat.Action action, int index)
        {
            var img = action.ImageID != null ?
                _Parent.Project.ImageList.GetImage(action.ImageID) :
                null;
            var frameCount = action.Segments.Sum(s => s.Frames.Count);
            var desc = action.Segments.Count.ToString() + " segment(s), " +
                frameCount.ToString() + " frame(s).";
            return new AnimationListItem(img, action.ActionID, desc, index, action);
        }

        public void Activate()
        {
            if (SelectedChange != null)
            {
                SelectedChange();
            }
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
                _Parent.SelectedActionIndex = _SelectedItem.Index;
                _Parent.CurrentUI = EditorUI.Animation;
            }
        }

        public void RemoveCurrent()
        {
            if (_SelectedItem != null)
            {
                _Parent.Project.Actions.RemoveAt(_SelectedItem.Index);
                _SelectedItem.IsSelected = false;
                _SelectedItem = null;

                RefreshList();
            }
        }

        public void AddNew()
        {
            //find an available name
            int id = 1;
            {
                var list = _Parent.Project.Actions;
                while (list.Any(a => a.ActionID == "New Action " + id.ToString()))
                {
                    ++id;
                }
            }

            var action = new Pat.Action()
            {
                ActionID = "New Action " + id.ToString(),
                Segments = new List<Pat.AnimationSegment>(),
                InitEffects = new Pat.EffectList(),
                UpdateEffects = new Pat.EffectList(),
                KeyFrameEffects = new List<Pat.EffectList>(),
            };

            _Parent.Project.Actions.Add(action);

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

                var currentAnimation = _Parent.Project.Actions[id];
                dialog.AnimationID = currentAnimation.ActionID;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentAnimation.ActionID = dialog.AnimationID;

                    RefreshList();

                    OnSelectChange();
                }
            }
        }

        #endregion
    }
}
