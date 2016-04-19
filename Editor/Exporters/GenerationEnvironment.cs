using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters
{
    public interface GenerationEnvironment
    {
        int GetActionID(string name); //null or "" -> current action
        string GenerateActionAsActorInit(string name);
        string GetCurrentSkillKeyName(); //"b1"
        string GetSegmentStartEventHandlerFunctionName(); //"SegmentStartEventHandler" or null (not generated)
    }
}
