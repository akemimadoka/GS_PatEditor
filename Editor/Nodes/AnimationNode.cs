using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class AnimationNode
    {
        private readonly Editor _Parent;

        public Pat.Animation Data { get; private set; }

        public FrameNode Frame;

        public int SelectedSegmentIndex { get; private set; }
        public int SelectedFrameIndex { get; private set; }

        public void SetSelectedFrame(int segment, int frame)
        {
            //TODO handle empty animation
            SelectedSegmentIndex = segment;
            SelectedFrameIndex = frame;

            var seg = Data.Segments[SelectedSegmentIndex];
            Frame.Reset(seg, seg.Frames[SelectedFrameIndex]);
        }

        public event Action OnReset;

        public AnimationNode(Editor parent)
        {
            _Parent = parent;

            Frame = new FrameNode(parent);
        }

        public void Reset(Pat.Animation data)
        {
            Data = data;

            SetSelectedFrame(0, 0);

            if (OnReset != null)
            {
                OnReset();
            }
        }
    }
}
