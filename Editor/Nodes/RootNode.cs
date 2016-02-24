using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class RootNode
    {
        public Pat.Project Data;
        public AnimationNode Animation;

        public event Action OnReset;

        public void Reset()
        {
            if (OnReset != null)
            {
                OnReset();
            }
            Animation.Reset();
        }
    }
}
