using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Effects
{
    public class EditableEnvironment
    {
        public EditableEnvironment(Project proj)
        {
            Project = proj;
        }

        public Project Project { get; private set; }
        //add others (Frame, EffectList, etc.) here
    }

    interface IEditableEnvironment
    {
        EditableEnvironment Environment { get; set; }
    }
}
