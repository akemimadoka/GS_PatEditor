using GS_PatEditor.Editor.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor
{
    class Editor
    {
        public readonly AnimationFrames AnimationFramesUI;
        public Pat.Project Data { get; private set; }

        public Editor(Pat.Project proj)
        {
            AnimationFramesUI = new AnimationFrames(this);
            Data = proj;
        }
    }
}
