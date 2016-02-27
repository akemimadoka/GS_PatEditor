using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class RootNode
    {
        public Pat.Project Data { get; private set; }
        public AnimationNode Animation { get; private set; }

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

        public event Action OnReset;

        public void Reset()
        {
            if (OnReset != null)
            {
                OnReset();
            }
            Animation.Reset(null);
        }

        public static RootNode CreateRootNode(Pat.Project proj, Editor parent)
        {
            return new RootNode
            {
                Data = proj,
                Animation = new AnimationNode(parent),
                SelectedAnimationIndex = proj.Animations.Count == 0 ? -1 : 0,
            };
        }
    }
}
