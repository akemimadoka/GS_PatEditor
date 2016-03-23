using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class FrameNode
    {

        private readonly Editor _Parent;

        public Pat.AnimationSegment SegmentData { get; private set; }
        public Pat.Frame FrameData { get; private set; }

        public FrameEditMode EditMode { get; private set; }
        public FramePreviewMode PreviewMode { get; private set; }

        //TODO event name should not use OnXxx
        public event Action OnReset;
        public event Action EditModeChanged;

        public FrameNode(Editor parent)
        {
            _Parent = parent;
        }

        public void Reset(Pat.AnimationSegment seg, Pat.Frame frame)
        {
            SegmentData = seg;
            FrameData = frame;

            if (OnReset != null)
            {
                OnReset();
            }
        }

        public void ResetAnimation()
        {
            EditMode = FrameEditMode.None;
            PreviewMode = FramePreviewMode.Pause;

            if (_Parent.PreviewWindowUI != null)
            {
                _Parent.PreviewWindowUI.UpdatePreviewMode();
            }
        }

        public void ChangeEditMode(FrameEditMode mode)
        {
            EditMode = mode;

            if (EditModeChanged != null)
            {
                EditModeChanged();
            }
        }

        public void ChangePreviewMode(FramePreviewMode mode)
        {
            PreviewMode = mode;
            if (_Parent.PreviewWindowUI != null)
            {
                _Parent.PreviewWindowUI.UpdatePreviewMode();
            }

            EditMode = FrameEditMode.None;
            if (EditModeChanged != null)
            {
                EditModeChanged();
            }
        }
    }
}
