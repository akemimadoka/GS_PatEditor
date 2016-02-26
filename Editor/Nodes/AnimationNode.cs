using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class AnimationNode
    {
        public Pat.Animation Data { get; private set; }

        public FrameNode Frame;

        public int SelectedSegmentIndex;
        public int SelectedFrameIndex;

        public event Action OnReset;

        public AnimationNode()
        {
            Frame = new FrameNode();
        }

        public void Reset(Pat.Animation data)
        {
            Data = data;

            Frame.Reset();
            if (OnReset != null)
            {
                OnReset();
            }
        }
    }
}
