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

        public int SelectedSegmentIndex { get; private set; }
        public int SelectedFrameIndex { get; private set; }

        public void SetSelectedFrame(int segment, int frame)
        {
            SelectedSegmentIndex = segment;
            SelectedFrameIndex = frame;
            //TODO refresh node
        }

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
