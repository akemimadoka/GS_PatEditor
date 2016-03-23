using GS_PatEditor.Editor.Nodes;
using GS_PatEditor.Editor.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor
{
    enum EditorUI
    {
        AnimationList,
        Animation,
    }

    struct FrameIndex
    {
        public readonly int Segment;
        public readonly int Frame;

        public FrameIndex(int segment, int frame)
        {
            Segment = segment;
            Frame = frame;
        }
    }

    class Editor : IDisposable
    {

        //ui

        public AnimationFrames AnimationFramesUI { get; private set; }
        public PreviewWindow PreviewWindowUI { get; private set; }

        public AnimationList AnimationListUI { get; private set; }

        public Editor(Pat.Project proj)
        {
            ProjectReset += ResetAnimationIndex;

            //Animation = new AnimationNode(this);
            Frame = new FrameNode(this);

            Project = proj;

            AnimationFramesUI = new AnimationFrames(this);
            PreviewWindowUI = new PreviewWindow(this);
            AnimationListUI = new AnimationList(this);

        }

        public void Dispose()
        {
            PreviewWindowUI.Dispose();
            PreviewWindowUI = null;
        }

        //public AnimationNode Animation { get; private set; }
        public FrameNode Frame;

        #region Main UI

        public event Action CurrentUISwitched;

        private EditorUI _CurrentUI;
        public EditorUI CurrentUI
        {
            get
            {
                return _CurrentUI;
            }
            set
            {
                _CurrentUI = value;
                if (value == EditorUI.AnimationList)
                {
                    AnimationListUI.Activate();
                }
                if (CurrentUISwitched != null)
                {
                    CurrentUISwitched();
                }
            }
        }

        #endregion

        #region Project Object Access

        public event Action ProjectReset;

        private Pat.Project _Project;
        public Pat.Project Project
        {
            get
            {
                return _Project;
            }
            set
            {
                _Project = value;
                if (ProjectReset != null)
                {
                    ProjectReset();
                }
            }
        }

        #endregion

        #region AnimationIndex

        public event Action AnimationReset;

        public Pat.Animation CurrentAnimation { get; private set; }

        private int _SelectedAnimationIndex;
        public int SelectedAnimationIndex
        {
            get
            {
                return _SelectedAnimationIndex;
            }
            set
            {
                _SelectedAnimationIndex = value;
                if (value == -1)
                {
                    CurrentAnimation = null;
                }
                else
                {
                    CurrentAnimation = Project.Animations[value];
                }

                //Animation.Reset(CurrentAnimation);
                Frame.ResetAnimation();
                SelectedFrameIndex = new FrameIndex(0, 0);

                if (AnimationReset != null)
                {
                    AnimationReset();
                }
            }
        }

        private void ResetAnimationIndex()
        {
            SelectedAnimationIndex = Project.Animations.Count == 0 ? -1 : 0;
        }

        #endregion

        #region FrameIndex

        private int _SelectedFrameIndex, _SelectedSegmentIndex;
        public FrameIndex SelectedFrameIndex
        {
            get
            {
                return new FrameIndex(_SelectedSegmentIndex, _SelectedFrameIndex);
            }
            set
            {
                var segment = value.Segment;
                var frame = value.Frame;

                if (CurrentAnimation == null)
                {
                    Frame.Reset(null, null);
                    return;
                }

                if (segment == -1 || frame == -1)
                {
                    Frame.Reset(null, null);
                    return;
                }

                if (segment >= CurrentAnimation.Segments.Count)
                {
                    if (CurrentAnimation.Segments.Count == 0)
                    {
                        _SelectedSegmentIndex = -1;
                        _SelectedFrameIndex = -1;
                        Frame.Reset(null, null);
                        return;
                    }
                    else
                    {
                        segment = 0;
                    }
                }

                _SelectedSegmentIndex = segment;
                var seg = CurrentAnimation.Segments[segment];

                if (frame >= seg.Frames.Count)
                {
                    if (seg.Frames.Count == 0)
                    {
                        _SelectedFrameIndex = -1;
                        Frame.Reset(seg, null);
                        return;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
                _SelectedFrameIndex = frame;
                Frame.Reset(seg, seg.Frames[frame]);
            }
        }

        #endregion

        #region Show Forms

        public void ShowActionEditForm()
        {
            if (CurrentAnimation != null && CurrentAnimation.ActionID != null)
            {
                var action = Project.Actions.FirstOrDefault(a => a.ActionID == CurrentAnimation.ActionID);
                if (action != null)
                {
                    var dialog = new ActionEditForm(action);
                    dialog.ShowDialog();
                }
            }
        }

        #endregion
    }
}
