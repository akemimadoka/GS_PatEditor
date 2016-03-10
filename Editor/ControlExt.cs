using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    static class ControlExt
    {
        public static void SetIntegerMode(this TextBox txt, int def)
        {
            txt.Leave += delegate(object sender, EventArgs e)
            {
                int val;
                if (!Int32.TryParse(txt.Text, out val))
                {
                    txt.Text = def.ToString();
                    SystemSounds.Beep.Play();
                }
            };
        }

        public static int GetIntegerValue(this TextBox txt, int def)
        {
            int ret;
            if (Int32.TryParse(txt.Text, out ret))
            {
                return ret;
            }
            return def;
        }
    }
}
