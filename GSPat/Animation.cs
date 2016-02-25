using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.GSPat
{
    enum AnimationType
    {
        Normal,
        Clone,
    }

    class Animation
    {
        public int AnimationID;
        public AnimationType Type;

        public short CloneFrom;
        public short CloneTo;

        public short AttackLevel;
        public short CancelLevel;
        public bool IsLoop;
        public List<Frame> Frames;
    }
}
