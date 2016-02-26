using GS_PatEditor.Editor.Nodes;
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
        public Pat.Project Data { get; private set; }
        public RootNode EditorNode { get; private set; }

        //ui

        public AnimationFrames AnimationFramesUI { get; private set; }

        public Editor(Pat.Project proj)
        {
            Data = proj;
            EditorNode = RootNode.CreateRootNode(proj);
            AnimationFramesUI = new AnimationFrames(this);
        }
    }
}
