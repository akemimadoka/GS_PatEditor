using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    public partial class EditorForm : Form
    {
        public static void ShowEditorForm(Pat.Project proj)
        {
            EditorForm frm = new EditorForm();
            Editor editor = new Editor(proj);
            editor.AnimationFramesUI.Init(frm.animationFrames);

            Application.Run(frm);
        }

        public EditorForm()
        {
            InitializeComponent();
        }
    }
}
