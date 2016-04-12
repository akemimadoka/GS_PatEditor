using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat.Effects
{
    static class ActorVariableHelper
    {
        public static ILineObject GenerateSet(string name, Expression expr)
        {
            var nname = name.Replace("\\", "\\\\").Replace("\"", "\\\\");
            return new SimpleBlock(new ILineObject[] {
                new SimpleLineObject("if (!(\"variables\" in this.u)) this.u.variables <- {};"),
                new SimpleLineObject("if (!(\"" + nname + "\" in this.u.variables)) this.u.variables[\"" + nname + "\"] <- 0;"),
                ThisExpr.Instance.MakeIndex("u").MakeIndex("variables").MakeIndex(name).Assign(expr).Statement(),
            }).Statement();
        }

        public static Expression GenerateGet(string name)
        {
            return ThisExpr.Instance.MakeIndex("u").MakeIndex("variables").MakeIndex(name);
        }
    }
}
