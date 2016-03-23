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
        public Pat.Project Data { get; private set; }
        //public RootNode EditorNode { get; private set; }

        //ui

        public AnimationFrames AnimationFramesUI { get; private set; }
        public PreviewWindow PreviewWindowUI { get; private set; }

        public AnimationList AnimationListUI { get; private set; }

        public Editor(Pat.Project proj)
        {
            //Data = proj;
            //EditorNode = RootNode.CreateRootNode(proj, this);
            AnimationFramesUI = new AnimationFrames(this);
            PreviewWindowUI = new PreviewWindow(this);
            AnimationListUI = new AnimationList(this);

            Animation = new AnimationNode(this);
            SwitchProject(proj);
        }


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

        #region Root Node

        public AnimationNode Animation { get; private set; }

        public event Action ProjectReset;

        public void SwitchProject(Pat.Project proj)
        {
            Data = proj;
            //EditorNode.Reset(proj);
            SelectedAnimationIndex = proj.Animations.Count == 0 ? -1 : 0;
            if (ProjectReset != null)
            {
                ProjectReset();
            }
        }

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
                    Animation.Reset(Data.Animations[value]);
                }
            }
        }

        #endregion

        public void Dispose()
        {
            PreviewWindowUI.Dispose();
            PreviewWindowUI = null;
        }
    }
}
