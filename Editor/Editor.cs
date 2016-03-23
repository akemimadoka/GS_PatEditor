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

    class Editor : IDisposable
    {

        //ui

        public AnimationFrames AnimationFramesUI { get; private set; }
        public PreviewWindow PreviewWindowUI { get; private set; }

        public AnimationList AnimationListUI { get; private set; }

        public Editor(Pat.Project proj)
        {
            ProjectReset += ResetAnimationIndex;

            Animation = new AnimationNode(this);
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

        public AnimationNode Animation { get; private set; }

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
                    Animation.Reset(null);
                }
                else
                {
                    Animation.Reset(Project.Animations[value]);
                }
            }
        }

        private void ResetAnimationIndex()
        {
            SelectedAnimationIndex = Project.Animations.Count == 0 ? -1 : 0;
        }

        #endregion
    }
}
