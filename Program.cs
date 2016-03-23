using GS_PatEditor.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor
{
    class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var proj = ProjectGenerater.GenerateEmpty("", new List<string>());
            proj.IsEmptyProject = true;

            if (proj != null)
            {
                EditorForm.ShowEditorForm(proj);
            }
        }
    }
}
