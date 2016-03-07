using GS_PatEditor.Editor;
using GS_PatEditor.Pat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor
{
    class EditorTest
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var proj = ProjectGenerater.Generate();
            if (proj != null)
            {
                EditorForm.ShowEditorForm(proj);
            }
        }
    }
}
