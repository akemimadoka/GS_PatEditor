﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class FrameNode
    {
        public enum FrameEditMode
        {
            None,
            Move,
            Physical,
            Hit,
            Attack,
        }
        public enum FramePreviewMode
        {
            Pause,
            Play,
        }

        private readonly Editor _Parent;

        public Pat.AnimationSegment SegmentData { get; private set; }
        public Pat.Frame FrameData { get; private set; }

        public FrameEditMode EditMode { get; private set; }
        public FramePreviewMode PreviewMode { get; private set; }

        public bool PhysicalBoxVisible = true;

        public List<int> EditingBoxes = new List<int>();

        public event Action OnReset;

        public FrameNode(Editor parent)
        {
            _Parent = parent;
        }

        public void Reset(Pat.AnimationSegment seg, Pat.Frame frame)
        {
            SegmentData = seg;
            FrameData = frame;

            //should preserve edit mode
            //EditMode = FrameEditMode.None;
            //PreviewMode = FramePreviewMode.Pause;
            if (_Parent.PreviewWindowUI != null)
            {
                _Parent.PreviewWindowUI.UpdatePreviewMode();
            }

            if (OnReset != null)
            {
                OnReset();
            }
        }

        public bool ChangeEditMode(FrameEditMode mode)
        {
            EditMode = mode;
            return true;
        }

        public void AddHitBox(Pat.Box box)
        {

        }

        public void AddAttackBox(Pat.Box box)
        {

        }

        public void RemoveHitBox(int index)
        {

        }

        public void RemoveAttackBox(int index)
        {

        }
    }
}
