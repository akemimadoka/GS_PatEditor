using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels
{
    static class ControlExt
    {
        public static bool IsMouseInRange(this Control ctrl)
        {
            return ctrl.IsMouseInRangeNoRec() && ctrl.Parent.IsMouseInRangeNoRec();
        }

        private static bool IsMouseInRangeNoRec(this Control ctrl)
        {
            var point = ctrl.PointToClient(Control.MousePosition);
            if (!ctrl.ClientRectangle.Contains(point))
            {
                return false;
            }
            return true;
        }
    }
}
