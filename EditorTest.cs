using GS_PatEditor.Editor;
using GS_PatEditor.Pat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor
{
    class EditorTest
    {
        [STAThread]
        private static void Main()
        {
            var proj = ProjectGenerater.Generate();
            EditorForm.ShowEditorForm(proj);
        }
    }
}
