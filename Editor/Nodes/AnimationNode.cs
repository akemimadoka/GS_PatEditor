using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class AnimationNode
    {
        public Pat.Animation Data;
        public FrameNode Frame;

        public event Action OnReset;

        public void Reset()
        {
            Frame.Reset();
            if (OnReset != null)
            {
                OnReset();
            }
        }
    }
}
